using Eventor.Common.Enums;
using Eventor.Database.Core;
using Eventor.Services.Exceptions;
using Microsoft.Extensions.Logging;
using Day = Eventor.Common.Core.Day;
namespace Eventor.Services;

public class EconomyService : IEconomyService
{
    private readonly IEventRepository _eventRepository;
    private readonly IDayRepository _dayRepository;
    private readonly IItemRepository _itemRepository;
    private readonly IMenuRepository _menuRepository;
    private readonly IPersonRepository _personRepository;
    private readonly ILogger<EconomyService> _logger;

    public EconomyService(
        IEventRepository eventRepository,
        IDayRepository dayRepository,
        IItemRepository itemRepository,
        IMenuRepository menuRepository,
        IPersonRepository personRepository,
        ILogger<EconomyService> logger)
    {
        _eventRepository = eventRepository;
        _dayRepository = dayRepository;
        _itemRepository = itemRepository;
        _menuRepository = menuRepository;
        _personRepository = personRepository;
        _logger = logger;
    }

    // ------------------------------------
    // Функции стоимости
    // ------------------------------------

    /// <summary>
    /// Стоимость предмета (C_O)
    /// </summary>
    public async Task<double> GetItemCostAsync(Guid itemId)
    {
        try
        {
            var item = await _itemRepository.GetItemByIdAsync(itemId);

            if (item == null)
            {
                _logger.LogWarning($"Предмет с ID {itemId} не найден.");
                return 0;
            }

            return item.Price;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Ошибка при получении стоимости предмета {itemId}.");
            return 0;
        }
    }

    /// <summary>
    /// Стоимость меню (C_M)
    /// </summary>
    public async Task<double> GetMenuCostAsync(Guid menuId)
    {
        try
        {
            var items = await _itemRepository.GetAllItemsByMenuAsync(menuId);

            // Проверка на null или пустую коллекцию
            if (items == null || !items.Any())
            {
                _logger.LogWarning("No items found for menu {MenuId}", menuId);
                return 0;
            }

            double totalCost = 0;

            foreach (var item in items)
            {
                // Пропуск null-элементов
                if (item == null)
                {
                    _logger.LogWarning("Null item encountered in menu {MenuId}", menuId);
                    continue;
                }

                var amount = await _menuRepository.GetAmountItemAsync(menuId, item.Id);

                totalCost += item.Price * amount;
            }

            return totalCost;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error calculating cost for menu {MenuId}", menuId);
            throw new EconomyServiceException(
                $"Failed to calculate menu cost for {menuId}", ex);
        }
    }

    /// <summary>
    /// Стоимость набора дней n-го порядка (C_D)
    /// </summary>
    public async Task<double> GetDaysCostAsync(IEnumerable<Guid> daysId)
    {
        if (daysId == null || !daysId.Any())
        {
            _logger.LogWarning("Empty days list provided");
            return 0;
        }

        try
        {
            double totalCost = 0;
            var distinctDays = daysId.Distinct().ToList();

            foreach (var dayId in distinctDays)
            {
                var day = await _dayRepository.GetDayByIdAsync(dayId);
                if (day == null)
                {
                    _logger.LogWarning("Day {DayId} not found", dayId);
                    continue;
                }

                var dayCost = await GetMenuCostAsync(day.MenuId);
                totalCost += dayCost;
            }

            return totalCost;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error calculating cost for days: {Days}",
                string.Join(", ", daysId));
            throw new EconomyServiceException("Failed to calculate days cost", ex);
        }
    }

    /// <summary>
    /// Стоимость мероприятия (C_E)
    /// </summary>
    public async Task<double> GetEventCostAsync(Guid eventId)
    {
        try
        {
            var eventDays = await _dayRepository.GetAllDaysByEventAsync(eventId);

            if (!eventDays.Any())
            {
                _logger.LogInformation("Event {EventId} has no days", eventId);
                return 0;
            }

            double totalCost = 0;
            foreach (var day in eventDays)
            {
                try
                {
                    var dayCost = await GetMenuCostAsync(day.MenuId);
                    totalCost += dayCost;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error calculating cost for day {DayId} in event {EventId}",
                        day.Id, eventId);
                }
            }

            _logger.LogDebug("Total cost {Cost} calculated for event {EventId}", totalCost, eventId);
            return totalCost;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error calculating total cost for event {EventId}", eventId);
            throw new EconomyServiceException(
                $"Failed to calculate total cost for event {eventId}", ex);
        }
    }

    // ------------------------------------
    // Функции цены
    // ------------------------------------

    /// <summary>
    /// Цена дня первого порядка (P_D)
    /// </summary>
    /// <remarks>
    /// Формула: 
    /// P_D = P₀ * A(d)
    /// где:
    /// - P₀ — фундаментальная цена,
    /// - A(d) — коэффициент дня.
    /// </remarks>
    public async Task<double> GetDayPriceAsync(Guid dayId)
    {
        try
        {
            // Получаем фундаментальную цену для мероприятия
            var currentEvent = await _eventRepository.GetEventByDayAsync(dayId);
            var fundamentalPrice = await CalculateFundamentalPrice1DAsync(currentEvent.Id);

            // Получаем коэффициент дня
            var coefficient = await GetDayCoefficientAsync(new[] { dayId });

            return (1 + currentEvent.Percent / 100) * fundamentalPrice * coefficient;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Ошибка расчета цены дня {dayId}");
            throw new EconomyServiceException("Не удалось вычислить цену дня", ex);
        }
    }

    public async Task<double> GetDayPriceWithPrivilegesAsync(Guid dayId)
    {
        try
        {
            // Получаем фундаментальную цену для мероприятия
            var currentEvent = await _eventRepository.GetEventByDayAsync(dayId);
            var fundamentalPrice = await CalculateFundamentalPriceWithPrivileges1DAsync(currentEvent.Id);

            // Получаем коэффициент дня
            var coefficient = await GetDayCoefficientAsync(new[] { dayId });

            return (1 + currentEvent.Percent / 100) * fundamentalPrice * coefficient;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Ошибка расчета цены дня {dayId}");
            throw new EconomyServiceException("Не удалось вычислить цену дня", ex);
        }
    }

    /// <summary>
    /// Цена набора дней n-го порядка (P_D)
    /// </summary>
    /// <remarks>
    /// Формула:
    /// P_D = P₀ * A(c)
    /// где:
    /// - P₀ — фундаментальная цена мероприятия,
    /// - A(c) — коэффициент комбинации дней.
    /// </remarks>
    public async Task<double> GetDaysPriceAsync(IEnumerable<Guid> daysId)
    {
        if (daysId == null || !daysId.Any())
        {
            _logger.LogWarning("Empty days collection provided");
            return 0;
        }

        try
        {
            // 1. Проверяем что все дни принадлежат одному мероприятию
            if (!await AreDaysFromSameEventAsync(daysId))
            {
                _logger.LogError("Days belong to different events: {DayIds}",
                    string.Join(", ", daysId));
                throw new EconomyServiceException("All days must belong to the same event");
            }

            // 2. Получаем первый день для определения мероприятия
            var firstDay = await _dayRepository.GetDayByIdAsync(daysId.First());
            if (firstDay == null)
            {
                _logger.LogError("First day not found");
                throw new EconomyServiceException("Invalid day IDs provided");
            }

            // 3. Получаем фундаментальную цену мероприятия
            var eventObj = await _eventRepository.GetEventByDayAsync(firstDay.Id);
            var fundamentalPrice = await CalculateFundamentalPriceNDAsync(eventObj.Id);

            // 4. Рассчитываем коэффициент комбинации дней
            var coefficient = await GetDayCoefficientAsync(daysId);

            return (1 + eventObj.Percent / 100) * fundamentalPrice * coefficient;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error calculating price for days: {DayIds}",
                string.Join(", ", daysId));
            throw new EconomyServiceException("Failed to calculate days price", ex);
        }
    }

    public async Task<double> GetDaysPriceWithPrivilegesAsync(IEnumerable<Guid> daysId)
    {
        if (daysId == null || !daysId.Any())
        {
            _logger.LogWarning("Empty days collection provided");
            return 0;
        }

        try
        {
            // 1. Проверяем что все дни принадлежат одному мероприятию
            if (!await AreDaysFromSameEventAsync(daysId))
            {
                _logger.LogError("Days belong to different events: {DayIds}",
                    string.Join(", ", daysId));
                throw new EconomyServiceException("All days must belong to the same event");
            }

            // 2. Получаем первый день для определения мероприятия
            var firstDay = await _dayRepository.GetDayByIdAsync(daysId.First());
            if (firstDay == null)
            {
                _logger.LogError("First day not found");
                throw new EconomyServiceException("Invalid day IDs provided");
            }

            // 3. Получаем фундаментальную цену мероприятия
            var eventObj = await _eventRepository.GetEventByDayAsync(firstDay.Id);
            var fundamentalPrice = await CalculateFundamentalPriceWithPrivilegesNDAsync(eventObj.Id);

            // 4. Рассчитываем коэффициент комбинации дней
            var coefficient = await GetDayCoefficientAsync(daysId);

            return (1 + eventObj.Percent / 100) * fundamentalPrice * coefficient;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error calculating price for days: {DayIds}",
                string.Join(", ", daysId));
            throw new EconomyServiceException("Failed to calculate days price", ex);
        }
    }

    /// <summary>
    /// Цена мероприятия (P_E)
    /// </summary>
    /// <remarks>
    /// Формула:
    /// P_E = P₀ * A(D)
    /// где:
    /// - P₀ — фундаментальная цена мероприятия,
    /// - A(D) — коэффициент всех дней мероприятия.
    /// 
    /// Альтернативно:
    /// P_E = Σ C(d) — сумма стоимостей всех дней
    /// </remarks>
    public async Task<double> GetEventPriceAsync(Guid eventId)
    {
        try
        {
            var days = await _dayRepository.GetAllDaysByEventAsync(eventId);
            if (!days.Any())
            {
                _logger.LogWarning("Event {EventId} has no days", eventId);
                return 0;
            }

            var allDayIds = days.Select(d => d.Id);
            var eventObj = await _eventRepository.GetEventByDayAsync(eventId);
            var fundamentalPrice = await CalculateFundamentalPriceNDAsync(eventId);
            var allDaysCoefficient = await GetDayCoefficientAsync(allDayIds);
            var price = fundamentalPrice * allDaysCoefficient;

            return (1 + eventObj.Percent / 100) * fundamentalPrice * allDaysCoefficient;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error calculating price for event {EventId}", eventId);
            throw new EconomyServiceException("Failed to calculate event price", ex);
        }
    }

    // ------------------------------------
    // Коэффициенты и фундаментальная цена
    // ------------------------------------

    /// <summary>
    /// Рассчитывает коэффициент дня/набора дней (A) согласно теории мероприятий
    /// </summary>
    /// <param name="daysId">Коллекция идентификаторов дней</param>
    /// <returns>Значение коэффициента</returns>
    /// <remarks>
    /// Формула расчета: 
    /// A(d1..dn) = сумма_стоимостей_выбранных_дней / минимальная_стоимость_среди_всех_дней_мероприятия
    /// </remarks>
    public async Task<double> GetDayCoefficientAsync(IEnumerable<Guid> daysId)
    {
        if (daysId == null || !daysId.Any())
        {
            _logger.LogWarning("Empty days collection provided for coefficient calculation");
            return 0;
        }

        try
        {
            // 1. Получаем мероприятие по первому дню 
            var firstDay = await _dayRepository.GetDayByIdAsync(daysId.First());
            if (firstDay == null)
            {
                _logger.LogError("First day not found");
                throw new EconomyServiceException("Invalid day IDs provided");
            }

            var currentEvent = await _eventRepository.GetEventByDayAsync(firstDay.Id);
            if (currentEvent == null)
            {
                _logger.LogError("Event not found for day {DayId}", firstDay.Id);
                throw new EconomyServiceException("Associated event not found");
            }

            // 2. Получаем ВСЕ дни мероприятия
            var allEventDays = await _dayRepository.GetAllDaysByEventAsync(currentEvent.Id);
            if (!allEventDays.Any())
            {
                _logger.LogError("Event {EventId} has no days", currentEvent.Id);
                throw new EconomyServiceException("Event has no days");
            }

            // 3. Рассчитываем стоимости и проверяем что все положительные
            var eventDayCosts = new List<double>();
            foreach (var day in allEventDays)
            {
                var cost = await GetMenuCostAsync(day.MenuId);
                if (cost <= 0)
                {
                    _logger.LogError("Day {DayId} has non-positive cost: {Cost}", day.Id, cost);
                    throw new EconomyServiceException($"Day {day.Id} has invalid cost value");
                }
                eventDayCosts.Add(cost);
            }

            // 4. Проверяем что запрошенные дни принадлежат мероприятию
            var invalidDays = daysId.Except(allEventDays.Select(d => d.Id)).ToList();
            if (invalidDays.Any())
            {
                _logger.LogError("Invalid days provided: {Days}", invalidDays);
                throw new EconomyServiceException("Some days don't belong to the event");
            }

            // 5. Вычисляем коэффициент
            var minCost = eventDayCosts.Min();
            var sumCost = allEventDays
                .Where(d => daysId.Contains(d.Id))
                .Sum(d => eventDayCosts[allEventDays.IndexOf(d)]); // Используем кэшированные значения

            return sumCost / minCost;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error calculating coefficient for days: {Days}",
                string.Join(", ", daysId));
            throw new EconomyServiceException("Coefficient calculation failed", ex);
        }
    }

    /// <summary>
    /// Фундаментальная цена для общего одномерного случая (P₀)
    /// </summary>
    /// <remarks>
    /// Формула: 
    /// P₀ = C_E(E) / Σ [A(dᵢ) * N(dᵢ)] 
    /// где:
    /// - C_E(E) — стоимость мероприятия,
    /// - A(dᵢ) — коэффициент дня dᵢ,
    /// - N(dᵢ) — количество участников дня dᵢ.
    /// </remarks>
    public async Task<double> CalculateFundamentalPrice1DAsync(Guid eventId)
    {
        try
        {
            // Получаем стоимость мероприятия
            var eventCost = await GetEventCostAsync(eventId);
            if (eventCost < 0)
            {
                _logger.LogError($"Стоимость мероприятия {eventId} отрицательна: {eventCost}");
                throw new EconomyServiceException("Стоимость мероприятия не может быть отрицательной");
            }

            // Получаем все дни мероприятия
            var days = await _dayRepository.GetAllDaysByEventAsync(eventId);
            if (!days.Any())
            {
                _logger.LogError($"Мероприятие {eventId} не содержит дней");
                throw new EconomyServiceException("Мероприятие должно содержать хотя бы один день");
            }

            // Рассчитываем сумму A(dᵢ) * N(dᵢ)
            double sum = 0;
            foreach (var day in days)
            {
                var coefficient = await GetDayCoefficientAsync(new[] { day.Id });
                var participants = await GetPersonCountAsync(day.Id);
                sum += coefficient * participants;
            }

            if (sum <= 0)
            {
                _logger.LogError($"Сумма коэффициентов и участников для мероприятия {eventId} равна {sum}");
                throw new EconomyServiceException("Знаменатель формулы P₀ должен быть положительным");
            }

            return eventCost / sum;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Ошибка расчета P₀ для мероприятия {eventId}");
            throw new EconomyServiceException("Не удалось вычислить фундаментальную цену", ex);
        }
    }

    /// <summary>
    /// Фундаментальная цена для общего n-мерного случая (P₀)
    /// </summary>
    /// <remarks>
    /// Формула:
    /// P₀ = C_E(E) / Σ [A(cⱼ) * N(cⱼ)]
    /// где:
    /// - C_E(E) — стоимость мероприятия,
    /// - cⱼ — уникальная комбинация дней,
    /// - A(cⱼ) — коэффициент комбинации дней,
    /// - N(cⱼ) — количество участников, выбравших комбинацию cⱼ.
    /// </remarks>
    public async Task<double> CalculateFundamentalPriceNDAsync(Guid eventId)
    {
        try
        {
            // Получаем стоимость мероприятия
            var eventCost = await GetEventCostAsync(eventId);
            if (eventCost < 0)
            {
                _logger.LogError($"Стоимость мероприятия {eventId} отрицательна: {eventCost}");
                throw new EconomyServiceException("Стоимость мероприятия не может быть отрицательной");
            }

            // Получаем все уникальные комбинации дней
            var dayCombinations = await GetCurrentDayCombinationsAsync(eventId);
            if (!dayCombinations.Any())
            {
                _logger.LogError($"Мероприятие {eventId} не имеет комбинаций дней");
                throw new EconomyServiceException("Нет данных о выбранных комбинациях дней");
            }

            // Рассчитываем сумму A(cⱼ) * N(cⱼ)
            double sum = 0;
            foreach (var combo in dayCombinations)
            {
                var dayIds = combo.Select(d => d.Id).ToList();

                // Коэффициент комбинации дней
                var coefficient = await GetDayCoefficientAsync(dayIds);
                if (coefficient <= 0)
                {
                    _logger.LogWarning($"Комбинация дней имеет неположительный коэффициент: {coefficient}");
                    continue;
                }

                // Количество участников для комбинации
                var participantsCount = await GetPersonCountAsync(dayIds);
                if (participantsCount == 0)
                {
                    _logger.LogWarning("Комбинация дней не имеет участников");
                    continue;
                }

                sum += coefficient * participantsCount;
            }

            if (sum <= 0)
            {
                _logger.LogError($"Сумма коэффициентов и участников для мероприятия {eventId} равна {sum}");
                throw new EconomyServiceException("Знаменатель формулы P₀ должен быть положительным");
            }

            // Рассчитываем P₀
            return eventCost / sum;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Ошибка расчета P₀ (n-мерный случай) для мероприятия {eventId}");
            throw new EconomyServiceException("Не удалось вычислить многомерную фундаментальную цену", ex);
        }
    }

    /// <summary>
    /// Фундаментальная цена для общего одномерного случая с учётом привилегий (P0)
    /// </summary>
    public async Task<double> CalculateFundamentalPriceWithPrivileges1DAsync(Guid eventId)
    {
        try
        {
            var eventCost = await GetEventCostAsync(eventId);
            if (eventCost < 0)
            {
                _logger.LogError($"Стоимость мероприятия {eventId} отрицательна: {eventCost}");
                throw new EconomyServiceException("Недопустимая стоимость мероприятия");
            }

            var days = await _dayRepository.GetAllDaysByEventAsync(eventId);
            if (!days.Any())
            {
                _logger.LogError($"Мероприятие {eventId} не содержит дней");
                throw new EconomyServiceException("Мероприятие должно содержать дни");
            }

            double sum = 0;
            foreach (var day in days)
            {
                var coefficient = await GetDayCoefficientAsync(new[] { day.Id });
                var participants = await GetPersonCountExcludingPrivilegesAsync(day.Id);
                sum += coefficient * participants;
            }

            if (sum <= 0)
            {
                _logger.LogError($"Сумма коэффициентов для мероприятия {eventId} равна {sum}");
                throw new EconomyServiceException("Невозможно вычислить фундаментальную цену");
            }

            return eventCost / sum;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Ошибка расчета P₀ с привилегиями для мероприятия {eventId}");
            throw new EconomyServiceException("Ошибка расчета с учетом привилегий", ex);
        }
    }

    /// <summary>
    /// Фундаментальная цена для общего n-мерного случая с учётом привилегий (P0)
    /// </summary>
    public async Task<double> CalculateFundamentalPriceWithPrivilegesNDAsync(Guid eventId)
    {
        try
        {
            var eventCost = await GetEventCostAsync(eventId);
            if (eventCost < 0)
            {
                _logger.LogError($"Стоимость мероприятия {eventId} отрицательна: {eventCost}");
                throw new EconomyServiceException("Недопустимая стоимость мероприятия");
            }

            var dayCombinations = await GetCurrentDayCombinationsWithPrivilegesAsync(eventId);
            if (!dayCombinations.Any())
            {
                _logger.LogError($"Мероприятие {eventId} не имеет комбинаций дней");
                throw new EconomyServiceException("Нет данных о комбинациях дней");
            }

            double sum = 0;
            foreach (var combo in dayCombinations)
            {
                var dayIds = combo.Select(d => d.Id).ToList();
                var coefficient = await GetDayCoefficientAsync(dayIds);
                var participants = await GetPersonCountExcludingPrivilegesAsync(dayIds);
                sum += coefficient * participants;
            }

            if (sum <= 0)
            {
                _logger.LogError($"Сумма коэффициентов для мероприятия {eventId} равна {sum}");
                throw new EconomyServiceException("Невозможно вычислить фундаментальную цену");
            }

            return eventCost / sum;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Ошибка расчета P₀ (nD с привилегиями) для мероприятия {eventId}");
            throw new EconomyServiceException("Ошибка многомерного расчета с привилегиями", ex);
        }
    }

    // ------------------------------------
    // Уравнения баланса
    // ------------------------------------

    /// <summary>
    /// Проверка баланса для общего одномерного случая
    /// </summary>
    /// <remarks>
    /// Уравнение баланса: C_E(E) = Σ P_D(d_i) * N(d_i)
    /// </remarks>
    public async Task<bool> CheckBalance1DAsync(Guid eventId)
    {
        try
        {
            // 1. Получаем все дни мероприятия
            var days = await _dayRepository.GetAllDaysByEventAsync(eventId);
            if (!days.Any())
            {
                _logger.LogWarning("Event {EventId} has no days", eventId);
                return false;
            }

            // 2. Рассчитываем общую стоимость мероприятия
            var totalCost = await GetEventCostAsync(eventId);

            // 3. Рассчитываем сумму (цена дня * количество участников)
            double incomeSum = 0;
            foreach (var day in days)
            {
                var dayPrice = await GetDayPriceAsync(day.Id);
                var participantsCount = await GetPersonCountAsync(day.Id);
                incomeSum += dayPrice * participantsCount;
            }

            return incomeSum - totalCost >= 0;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error checking 1D balance for event {EventId}", eventId);
            throw new EconomyServiceException("Failed to check 1D balance", ex);
        }
    }

    /// <summary>
    /// Проверка баланса для общего n-мерного случая
    /// </summary>
    /// <remarks>
    /// Уравнение баланса: C_E(E) = Σ P_D(c_i) * N(c_i)
    /// где c_i - комбинации дней
    /// </remarks>
    public async Task<bool> CheckBalanceNDAsync(Guid eventId)
    {
        try
        {
            // 1. Получаем все комбинации дней
            var dayCombinations = await GetCurrentDayCombinationsAsync(eventId);
            if (!dayCombinations.Any())
            {
                _logger.LogWarning("Event {EventId} has no day combinations", eventId);
                return false;
            }

            // 2. Рассчитываем общую стоимость мероприятия
            var totalCost = await GetEventCostAsync(eventId);

            // 3. Рассчитываем сумму (цена комбинации * количество участников)
            double incomeSum = 0;
            foreach (var combination in dayCombinations)
            {
                var combinationPrice = await GetDaysPriceAsync(combination.Select(d => d.Id));
                var participantsCount = await GetPersonCountAsync(combination.Select(d => d.Id));
                incomeSum += combinationPrice * participantsCount;
            }

            return incomeSum - totalCost >= 0;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error checking ND balance for event {EventId}", eventId);
            throw new EconomyServiceException("Failed to check ND balance", ex);
        }
    }

    // ------------------------------------
    // Вспомогательные методы
    // ------------------------------------

    /// <summary>
    /// Получает текущие комбинации дней (H), выбранные участниками мероприятия
    /// </summary>
    /// <param name="eventId">Идентификатор мероприятия</param>
    /// <returns>Коллекция уникальных комбинаций дней</returns>
    /// <remarks>
    /// Формула: 
    /// H(E) = {c | ∃! p ∈ P: (p, c) ∈ PD}
    /// где:
    /// - c ⊆ D - комбинация дней
    /// - PD - множество связей участников с выбранными днями
    /// </remarks>
    public async Task<IEnumerable<IEnumerable<Day>>> GetCurrentDayCombinationsAsync(Guid eventId)
    {
        try
        {
            // Получаем всех участников мероприятия
            var persons = await _personRepository.GetAllPersonsByEventAsync(eventId);

            // Для каждого участника получаем выбранные дни
            var combinations = new List<HashSet<Guid>>();
            foreach (var person in persons)
            {
                var days = await _dayRepository.GetAllDaysByPersonAsync(person.Id);
                var dayIds = days.Select(d => d.Id).ToHashSet();

                if (dayIds.Any())
                {
                    combinations.Add(dayIds);
                }
            }

            // Группируем уникальные комбинации
            var uniqueCombinations = combinations
                .GroupBy(c => c, HashSet<Guid>.CreateSetComparer())
                .Select(g => g.Key);

            // Преобразуем в коллекцию дней
            var result = new List<IEnumerable<Day>>();
            foreach (var combo in uniqueCombinations)
            {
                var days = await Task.WhenAll(combo.Select(id => _dayRepository.GetDayByIdAsync(id)));
                result.Add(days.Where(d => d != null)!);
            }

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting day combinations for event {EventId}", eventId);
            throw new EconomyServiceException("Failed to retrieve current day combinations", ex);
        }
    }

    /// <summary>
    /// Получает текущие комбинации дней с учетом исключения привилегированных участников
    /// </summary>
    /// <remarks>
    /// Исключаются участники с типами: Organizer, VIP
    /// </remarks>
    public async Task<IEnumerable<IEnumerable<Day>>> GetCurrentDayCombinationsWithPrivilegesAsync(Guid eventId)
    {
        try
        {
            // 1. Получаем всех участников мероприятия, исключая привилегированные типы
            var allPersons = await _personRepository.GetAllPersonsByEventAsync(eventId);
            var filteredPersons = allPersons
                .Where(p => p.Type != PersonType.Organizer && p.Type != PersonType.VIP)
                .ToList();

            // 2. Для каждого участника получаем выбранные дни
            var combinations = new List<HashSet<Guid>>();
            foreach (var person in filteredPersons)
            {
                var days = await _dayRepository.GetAllDaysByPersonAsync(person.Id);
                var dayIds = days.Select(d => d.Id).ToHashSet();

                if (dayIds.Any())
                {
                    combinations.Add(dayIds);
                }
            }

            // 3. Группируем уникальные комбинации
            var uniqueCombinations = combinations
                .GroupBy(c => c, HashSet<Guid>.CreateSetComparer())
                .Select(g => g.Key);

            // 4. Преобразуем в коллекцию дней
            var result = new List<IEnumerable<Day>>();
            foreach (var combo in uniqueCombinations)
            {
                var days = await Task.WhenAll(combo.Select(id => _dayRepository.GetDayByIdAsync(id)));
                result.Add(days.Where(d => d != null)!);
            }

            _logger.LogInformation($"Найдено {result.Count} комбинаций с учетом привилегий");
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ошибка получения комбинаций с привилегиями");
            throw new EconomyServiceException("Ошибка обработки привилегий", ex);
        }
    }

    /// <summary>
    /// Возвращает количество участников для конкретного дня (N_D)
    /// </summary>
    /// <param name="dayId">Идентификатор дня</param>
    /// <returns>Количество участников дня</returns>
    /// <remarks>
    /// Формула: 
    /// N_D(d) = |{ p ∈ P | (p, d) ∈ PD }|
    /// где:
    /// - PD - множество связей участников с днями
    /// - d ∈ D - запрашиваемый день
    /// </remarks>
    public async Task<int> GetPersonCountAsync(Guid dayId)
    {
        try
        {
            // Проверка существования дня
            var day = await _dayRepository.GetDayByIdAsync(dayId);
            if (day == null)
            {
                _logger.LogWarning($"День {dayId} не найден");
                return 0;
            }

            // Получение участников дня
            var persons = await _personRepository.GetAllPersonsByDayAsync(dayId);

            // Подсчет уникальных участников
            return persons.Distinct().Count();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Ошибка получения количества участников для дня {dayId}");
            throw new EconomyServiceException("Ошибка расчета количества участников", ex);
        }
    }

    /// <summary>
    /// Возвращает количество участников для набора дней (N_D^n)
    /// </summary>
    /// <param name="daysId">Коллекция идентификаторов дней</param>
    /// <returns>Количество участников всех указанных дней</returns>
    /// <remarks>
    /// Формула: 
    /// N_D(d1..dn) = |{ p ∈ P | ∀d ∈ {d1..dn}: (p, d) ∈ PD }|
    /// </remarks>
    public async Task<int> GetPersonCountAsync(IEnumerable<Guid> daysId)
    {
        if (daysId == null || !daysId.Any())
        {
            _logger.LogWarning("Пустой список дней");
            return 0;
        }

        try
        {
            var allPersons = await _personRepository.GetAllPersonsAsync();
            if (allPersons == null) return 0; 

            var result = 0;
            foreach (var person in allPersons)
            {
                var personDays = await _dayRepository.GetAllDaysByPersonAsync(person.Id);
                if (personDays == null) continue; 

                if (daysId.All(d => personDays.Any(pd => pd.Id == d)))
                {
                    result++;
                }
            }
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ошибка расчета для дней: {Days}", string.Join(", ", daysId));
            throw new EconomyServiceException("Ошибка расчета количества участников", ex);
        }
    }

    /// <summary>
    /// Возвращает общее количество участников мероприятия (N_E)
    /// </summary>
    /// <param name="eventId">Идентификатор мероприятия</param>
    /// <returns>Общее количество участников</returns>
    /// <remarks>
    /// Формула: 
    /// N_E(E) = |P| 
    /// где:
    /// - P = {p₁, p₂, ..., pₘ} - множество участников
    /// </remarks>
    public async Task<int> GetAllPersonsCountAsync(Guid eventId)
    {
        try
        {
            // Получение всех участников мероприятия
            var persons = await _personRepository.GetAllPersonsByEventAsync(eventId);

            // Проверка на null и пустую коллекцию
            if (persons == null || !persons.Any())
            {
                _logger.LogInformation($"Мероприятие {eventId} не имеет участников");
                return 0;
            }

            // Возврат мощности множества участников
            return persons
                .DistinctBy(p => p.Id) 
                .Count();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Ошибка получения участников для мероприятия {eventId}");
            throw new EconomyServiceException("Ошибка расчета общего количества участников", ex);
        }
    }

    /// <summary>
    /// Проверяет, что все дни принадлежат одному мероприятию
    /// </summary>
    /// <param name="daysId">Коллекция идентификаторов дней</param>
    /// <returns>True если все дни принадлежат одному мероприятию, иначе False</returns>
     public async Task<bool> AreDaysFromSameEventAsync(IEnumerable<Guid> daysId)
    {
        if (daysId == null || !daysId.Any())
            return false;

        // Получаем все уникальные идентификаторы мероприятий для указанных дней
        var eventIds = new HashSet<Guid>();

        foreach (var dayId in daysId.Distinct())
        {
            var day = await _dayRepository.GetDayByIdAsync(dayId);
            if (day == null) return false;

            var eventObj = await _eventRepository.GetEventByDayAsync(day.Id);
            if (eventObj == null) return false;

            eventIds.Add(eventObj.Id);

            // Если нашли более одного мероприятия - сразу возвращаем false
            if (eventIds.Count > 1)
                return false;
        }

        return eventIds.Count == 1;
    }

    /// <summary>
    /// Возвращает N-мерность мероприятия как максимальный размер комбинации выбранных дней
    /// </summary>
    public async Task<int> GetEventNAsync(Guid eventId)
    {
        try
        {
            // Получаем все текущие комбинации дней
            var combinations = await GetCurrentDayCombinationsAsync(eventId);

            if (!combinations.Any())
            {
                _logger.LogInformation($"Мероприятие {eventId} не имеет комбинаций дней");
                return 0;
            }

            // Находим максимальный размер комбинации
            var maxN = combinations.Max(c => c.Count());

            _logger.LogDebug($"N-мерность мероприятия {eventId}: {maxN}");
            return maxN;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Ошибка определения N-мерности мероприятия {eventId}");
            throw new EconomyServiceException("Не удалось определить размерность мероприятия", ex);
        }
    }

    public async Task<int> GetDaysCountBeforeEvent(Guid eventId)
    {
        try
        {
            // Получаем мероприятие по ID
            var eventObj = await _eventRepository.GetEventByIdAsync(eventId);

            if (eventObj == null)
            {
                _logger.LogWarning("Event {EventId} not found", eventId);
                return 0;
            }

            // Получаем текущую дату и дату мероприятия
            var today = DateOnly.FromDateTime(DateTime.Now);
            var eventDate = eventObj.Date;

            // Рассчитываем разницу в днях
            int daysDifference = eventDate.DayNumber - today.DayNumber;

            // Возвращаем 0 если мероприятие уже прошло
            return daysDifference > 0 ? daysDifference : 0;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error calculating days before event {EventId}", eventId);
            throw new EconomyServiceException("Failed to calculate days before event", ex);
        }
    }

    /// <summary>
    /// Получает количество участников без учета привилегированных ролей
    /// </summary>
    private async Task<int> GetPersonCountExcludingPrivilegesAsync(Guid dayId)
    {
        try
        {
            var persons = await _personRepository.GetAllPersonsByDayExcludingTypesAsync(
                dayId,
                new List<PersonType> { PersonType.Organizer, PersonType.VIP });

            return persons?.Count ?? 0;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Ошибка получения участников дня {dayId}");
            throw new EconomyServiceException("Ошибка подсчета участников", ex);
        }
    }

    /// <summary>
    /// Получает количество участников комбинации дней без учета привилегированных ролей
    /// </summary>
    private async Task<int> GetPersonCountExcludingPrivilegesAsync(List<Guid> dayIds)
    {
        try
        {
            var persons = await _personRepository.GetAllPersonsByDaysExcludingTypesAsync(
                dayIds,
                new List<PersonType> { PersonType.Organizer, PersonType.VIP });

            return persons?.Count ?? 0;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Ошибка получения участников дней: {string.Join(", ", dayIds)}");
            throw new EconomyServiceException("Ошибка подсчета участников", ex);
        }
    }

    /// <summary>
/// Рассчитывает текущие доходы мероприятия на основе выбранных комбинаций дней
/// </summary>
/// <remarks>
/// Формула: Σ [P_D(c_i) * N_D(c_i)] для всех c_i ∈ H(E)
/// </remarks>
public async Task<double> CalculateCurrentIncomeAsync(Guid eventId)
{
    try
    {
        double totalIncome = 0;
        
        // 1. Получаем все текущие комбинации дней
        var combinations = await GetCurrentDayCombinationsWithPrivilegesAsync(eventId);
        
        if (!combinations.Any())
        {
            _logger.LogWarning($"Мероприятие {eventId} не имеет комбинаций дней");
            return 0;
        }

        // 2. Для каждой комбинации считаем вклад в доход
        foreach (var combo in combinations)
        {
            var dayIds = combo.Select(d => d.Id).ToList();
            
            try
            {
                // 2.1. Получаем цену комбинации
                var price = await GetDaysPriceWithPrivilegesAsync(dayIds);

                // 2.2. Получаем количество участников
                var participants = await GetPersonCountExcludingPrivilegesAsync(dayIds);
                
                // 2.3. Добавляем к общему доходу
                totalIncome += price * participants;
                
                _logger.LogDebug($"Комбинация дней: {string.Join(",", dayIds)} " +
                                $"Вклад: {price} * {participants} = {price * participants}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Ошибка расчета для комбинации дней: {string.Join(",", dayIds)}");
            }
        }

        _logger.LogInformation($"Общий доход мероприятия {eventId}: {totalIncome}");
        return totalIncome;
    }
    catch (Exception ex)
    {
        _logger.LogError(ex, $"Ошибка расчета общего дохода для мероприятия {eventId}");
        throw new EconomyServiceException("Не удалось рассчитать текущие доходы", ex);
    }
}

    // ------------------------------------
    // Теоремы
    // ------------------------------------

    /// <summary>
    /// Проверяет базовые условия существования решения для мероприятия
    /// </summary>
    /// <remarks>
    /// Условия существования решения:
    /// 1. Мероприятие должно иметь хотя бы один день
    /// 2. Все дни должны иметь положительную стоимость
    /// 3. Должен быть хотя бы один участник
    /// </remarks>
    public async Task<bool> CheckSolutionExistenceAsync(Guid eventId)
    {
        try
        {
            // 1. Проверка наличия дней мероприятия
            var days = await _dayRepository.GetAllDaysByEventAsync(eventId);
            if (!days.Any())
            {
                _logger.LogWarning($"Мероприятие {eventId} не содержит дней");
                return false;
            }

            // 2. Проверка положительности стоимости ВСЕХ дней
            foreach (var day in days)
            {
                var dayCost = await GetDaysCostAsync(new[] { day.Id });
                if (dayCost <= 0)
                {
                    _logger.LogWarning($"День {day.Id} имеет неположительную стоимость: {dayCost}");
                    return false;
                }
            }

            // 3. Проверка что КАЖДЫЙ день выбран хотя бы одним участником
            foreach (var day in days)
            {
                var participantsCount = await GetPersonCountAsync(day.Id);
                if (participantsCount <= 0)
                {
                    _logger.LogWarning($"День {day.Id} не имеет участников");
                    return false;
                }
            }

            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Ошибка проверки условий существования решения для мероприятия {eventId}");
            throw new EconomyServiceException("Не удалось проверить условия существования решения", ex);
        }
    }

    public async Task<bool> CheckSolutionExistenceWithPrivilegesAsync(Guid eventId)
    {
        try
        {
            // 1. Проверка наличия дней мероприятия
            var days = await _dayRepository.GetAllDaysByEventAsync(eventId);
            if (!days.Any())
            {
                _logger.LogWarning($"Мероприятие {eventId} не содержит дней");
                return false;
            }

            // 2. Проверка положительности стоимости ВСЕХ дней
            foreach (var day in days)
            {
                var dayCost = await GetDaysCostAsync(new[] { day.Id });
                if (dayCost <= 0)
                {
                    _logger.LogWarning($"День {day.Id} имеет неположительную стоимость: {dayCost}");
                    return false;
                }
            }

            // 3. Проверка что КАЖДЫЙ день выбран хотя бы одним участником без привилегий
            foreach (var day in days)
            {
                var participantsCount = await GetPersonCountExcludingPrivilegesAsync(day.Id);
                if (participantsCount <= 0)
                {
                    _logger.LogWarning($"День {day.Id} не имеет участников");
                    return false;
                }
            }

            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Ошибка проверки условий существования решения для мероприятия {eventId}");
            throw new EconomyServiceException("Не удалось проверить условия существования решения", ex);
        }
    }

    /// <summary>
    /// Рассчитывает минимальное целое количество участников, необходимое для покрытия расходов мероприятия 
    /// при заданной максимальной цене посещения.
    /// </summary>
    /// <param name="eventId">Идентификатор мероприятия</param>
    /// <param name="maxPrice">Максимальная допустимая цена посещения</param>
    /// <returns>
    /// Минимальное количество участников (целое число, округленное вверх).
    /// Если мероприятие имеет неположительную стоимость, возвращает 0.
    /// </returns>
    /// <remarks>
    /// Формула: N_critical = ⎡C_E(E) / P_max⎤
    /// где:
    /// - C_E(E) — стоимость мероприятия,
    /// - P_max — максимальная цена посещения,
    /// - ⎡x⎤ — округление вверх до ближайшего целого.
    /// </remarks>
    public async Task<int> CalculateCriticalParticipantsCountAsync(Guid eventId, double maxPrice)
    {
        try
        {
            // Проверка корректности maxPrice
            if (maxPrice <= 0)
            {
                _logger.LogError($"Некорректная максимальная цена: {maxPrice}");
                throw new ArgumentException("Максимальная цена должна быть положительной.");
            }

            // Получаем стоимость мероприятия
            var eventCost = await GetEventCostAsync(eventId);

            // Рассчитываем критическое количество участников (округление вверх)
            var criticalCount = Math.Ceiling(eventCost / maxPrice);

            // Преобразуем в целое число
            return (int)criticalCount;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Ошибка расчета критического количества участников для мероприятия {eventId}");
            throw new EconomyServiceException("Не удалось провести расчет", ex);
        }
    }

    /// <summary>
    /// Рассчитывает максимально допустимую наценку для мероприятия при заданной максимальной цене посещения.
    /// </summary>
    /// <param name="eventId">Идентификатор мероприятия</param>
    /// <param name="maxPrice">Максимальная допустимая цена посещения</param>
    /// <returns>
    /// Максимальный коэффициент наценки (α ≥ 0). 
    /// Возвращает -1, если наценка невозможна.
    /// </returns>
    /// <remarks>
    /// Формула: α = min_{c_i ∈ H(E)} [ P_max / (A(c_i) * P_0) - 1 ]
    /// где:
    /// - P_max — максимальная цена,
    /// - A(c_i) — коэффициент комбинации дней,
    /// - P_0 — фундаментальная цена.
    /// </remarks>
    public async Task<double> CalculateMaxMarkupAsync(Guid eventId, double maxPrice)
    {
        try
        {
            // Проверка корректности maxPrice
            if (maxPrice <= 0)
            {
                _logger.LogError($"Некорректная максимальная цена: {maxPrice}");
                throw new ArgumentException("Максимальная цена должна быть положительной.");
            }

            // Получаем фундаментальную цену мероприятия
            var fundamentalPrice = await CalculateFundamentalPriceWithPrivilegesNDAsync(eventId);
            if (fundamentalPrice <= 0)
            {
                _logger.LogWarning($"Фундаментальная цена мероприятия {eventId} неположительна");
                return -1;
            }

            // Получаем текущие комбинации дней
            var dayCombinations = await GetCurrentDayCombinationsWithPrivilegesAsync(eventId);
            if (!dayCombinations.Any())
            {
                _logger.LogWarning($"Мероприятие {eventId} не имеет комбинаций дней");
                return -1;
            }

            double minMarkup = double.MaxValue;

            foreach (var combination in dayCombinations)
            {
                var dayIds = combination.Select(d => d.Id).ToList();

                // Рассчитываем коэффициент комбинации дней
                var coefficient = await GetDayCoefficientAsync(dayIds);
                if (coefficient <= 0)
                {
                    _logger.LogWarning($"Комбинация дней имеет неположительный коэффициент");
                    continue;
                }

                // Вычисляем допустимую наценку для комбинации
                var markup = (maxPrice / (coefficient * fundamentalPrice)) - 1;
                if (markup < 0)
                {
                    _logger.LogWarning($"Наценка для комбинации дней отрицательна: {markup}");
                    continue;
                }

                if (markup < minMarkup)
                {
                    minMarkup = markup;
                }
            }

            // Если не найдено допустимых значений
            if (minMarkup == double.MaxValue)
            {
                _logger.LogWarning($"Невозможно рассчитать наценку для мероприятия {eventId}");
                return -1;
            }

            return Math.Round(minMarkup, 2) * 100;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Ошибка расчета наценки для мероприятия {eventId}");
            throw new EconomyServiceException("Не удалось рассчитать наценку", ex);
        }
    }

    /// <summary>
    /// Рассчитывает интервал фундаментальной цены для мероприятия
    /// </summary>
    /// <remarks>
    /// Формула: [C_min/N_max, C_max/N_min]
    /// где:
    /// - C_min - минимальная стоимость дня
    /// - C_max - максимальная стоимость дня
    /// - N_min - минимальное количество участников в комбинации
    /// - N_max - максимальное количество участников в комбинации
    /// </remarks>
    public async Task<(double Min, double Max)> CalculateFundamentalPriceIntervalAsync(Guid eventId)
    {
        try
        {
            // 1. Получаем все дни мероприятия
            var days = await _dayRepository.GetAllDaysByEventAsync(eventId);
            if (!days.Any())
            {
                _logger.LogWarning($"Мероприятие {eventId} не содержит дней");
                return (0, 0);
            }

            // 2. Рассчитываем стоимости всех дней
            var dayCosts = new List<double>();
            foreach (var day in days)
            {
                var cost = await GetDaysCostAsync(new[] { day.Id });
                dayCosts.Add(cost);
            }

            var C_min = dayCosts.Min();
            var C_max = dayCosts.Max();

            // 3. Получаем все комбинации дней
            var combinations = await GetCurrentDayCombinationsAsync(eventId);
            if (!combinations.Any())
            {
                _logger.LogWarning($"Мероприятие {eventId} не имеет комбинаций дней");
                return (0, 0);
            }

            // 4. Рассчитываем количество участников для каждой комбинации
            var participantsCounts = new List<int>();
            foreach (var combo in combinations)
            {
                var count = await GetPersonCountAsync(combo.Select(d => d.Id));
                participantsCounts.Add(count);
            }

            var N_min = participantsCounts.Min();
            var N_max = participantsCounts.Max();

            // 5. Проверка делителей
            if (N_max <= 0 || N_min <= 0)
            {
                _logger.LogError($"Некорректное количество участников: N_min={N_min}, N_max={N_max}");
                throw new EconomyServiceException("Количество участников должно быть положительным");
            }

            // 6. Рассчитываем границы интервала
            var lowerBound = C_min / N_max;
            var upperBound = C_max / N_min;

            _logger.LogInformation($"Интервал фундаментальной цены для {eventId}: [{lowerBound}, {upperBound}]");
            return (lowerBound, upperBound);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Ошибка расчета интервала фундаментальной цены для мероприятия {eventId}");
            throw new EconomyServiceException("Не удалось рассчитать интервал цены", ex);
        }
    }
}
