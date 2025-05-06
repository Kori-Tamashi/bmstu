using Day = Eventor.Common.Core.Day;
namespace Eventor.Services;

/// <summary>
/// Интерфейс для финансовых расчетов мероприятия
/// </summary>
public interface IEconomyService
{
    // --- Функции стоимости ---

    /// <summary>
    /// Стоимость предмета (C_O)
    /// </summary>
    Task<double> GetItemCostAsync(Guid itemId);

    /// <summary>
    /// Стоимость меню (C_M)
    /// </summary>
    Task<double> GetMenuCostAsync(Guid menuId);

    /// <summary>
    /// Стоимость набора дней n-го порядка (C_D)
    /// </summary>
    Task<double> GetDaysCostAsync(IEnumerable<Guid> daysId);

    /// <summary>
    /// Стоимость мероприятия (C_E)
    /// </summary>
    Task<double> GetEventCostAsync(Guid eventId);


    // --- Функции цены ---

    /// <summary>
    /// Цена дня первого порядка (P_D)
    /// </summary>
    Task<double> GetDayPriceAsync(Guid dayId);

    /// <summary>
    /// Цена дня первого порядка с учетом привилегий (P_D)
    /// </summary>
    Task<double> GetDayPriceWithPrivilegesAsync(Guid dayId);

    /// <summary>
    /// Цена набора дней n-го порядка (P_D)
    /// </summary>
    Task<double> GetDaysPriceAsync(IEnumerable<Guid> daysId);

    /// <summary>
    /// Цена набора дней n-го порядка с учетом привилегий (P_D)
    /// </summary>
    Task<double> GetDaysPriceWithPrivilegesAsync(IEnumerable<Guid> daysId);

    /// <summary>
    /// Цена мероприятия (P_E)
    /// </summary>
    Task<double> GetEventPriceAsync(Guid eventId);


    // --- Коэффициенты и фундаментальные цены ---

    /// <summary>
    /// Коэффициент дня/набора дней (A)
    /// </summary>
    Task<double> GetDayCoefficientAsync(IEnumerable<Guid> daysId);

    /// <summary>
    /// Фундаментальная цена для общего одномерного случая (P0)
    /// </summary>
    Task<double> CalculateFundamentalPrice1DAsync(Guid eventId);

    /// <summary>
    /// Фундаментальная цена для общего n-мерного случая (P0)
    /// </summary>
    Task<double> CalculateFundamentalPriceNDAsync(Guid eventId);

    /// <summary>
    /// Фундаментальная цена для общего одномерного случая с учётом привилегий (P0)
    /// </summary>
    Task<double> CalculateFundamentalPriceWithPrivileges1DAsync(Guid eventId);

    /// <summary>
    /// Фундаментальная цена для общего n-мерного случая с учётом привилегий (P0)
    /// </summary>
    Task<double> CalculateFundamentalPriceWithPrivilegesNDAsync(Guid eventId);


    // --- Уравнения баланса ---

    /// <summary>
    /// Проверка баланса для общего одномерного случая
    /// </summary>
    Task<bool> CheckBalance1DAsync(Guid eventId);

    /// <summary>
    /// Проверка баланса для общего n-мерного случая
    /// </summary>
    Task<bool> CheckBalanceNDAsync(Guid eventId);


    // --- Вспомогательные методы ---

    /// <summary>
    /// Текущие комбинации дней (H)
    /// </summary>
    Task<IEnumerable<IEnumerable<Day>>> GetCurrentDayCombinationsAsync(Guid eventId);

    /// <summary>
    /// Получает текущие комбинации дней с учетом исключения привилегированных участников
    /// </summary>
    /// <remarks>
    /// Исключаются участники с типами: Organizer, VIP
    /// </remarks>
    Task<IEnumerable<IEnumerable<Day>>> GetCurrentDayCombinationsWithPrivilegesAsync(Guid eventId);

    /// <summary>
    /// Количество участников для дня/набора дней (N_D)
    /// </summary>
    Task<int> GetPersonCountAsync(Guid dayId);

    /// <summary>
    /// Общее количество участников (N_E)
    /// </summary>
    Task<int> GetAllPersonsCountAsync(Guid eventId);

    /// <summary>
    /// N-мерность мероприятия
    /// </summary>
    Task<int> GetEventNAsync(Guid eventId);

    /// <summary>
    /// Количество дней до мероприятия
    /// </summary>
    Task<int> GetDaysCountBeforeEvent(Guid eventId);

    /// <summary>
    /// Рассчитывает текущие доходы мероприятия на основе выбранных комбинаций дней
    /// </summary>
    /// <remarks>
    /// Формула: Σ [P_D(c_i) * N_D(c_i)] для всех c_i ∈ H(E)
    /// </remarks>
    Task<double> CalculateCurrentIncomeAsync(Guid eventId);

    // --- Теоремы ---

    /// <summary>
    /// Проверка условия существования решения
    /// </summary>
    Task<bool> CheckSolutionExistenceAsync(Guid eventId);

    /// <summary>
    /// Проверка условия существования решения, исключая привилегии
    /// </summary>
    Task<bool> CheckSolutionExistenceWithPrivilegesAsync(Guid eventId);

    /// <summary>
    /// Расчет минимального количества участников для покрытия расходов
    /// </summary>
    Task<int> CalculateCriticalParticipantsCountAsync(Guid eventId, double maxPrice);

    /// <summary>
    /// Расчет допустимой наценки
    /// </summary>
    Task<double> CalculateMaxMarkupAsync(Guid eventId, double maxPrice);

    /// <summary>
    /// Расчет интервала фундаментальной цены
    /// </summary>
    Task<(double Min, double Max)> CalculateFundamentalPriceIntervalAsync(Guid eventId);
}