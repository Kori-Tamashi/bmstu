using Eventor.Common.Core;
using Eventor.Services;
using Microsoft.Build.Framework;
using Microsoft.Extensions.DependencyInjection;
using PhoneNumbers;
using System.ComponentModel;
using System.Globalization;
using System.Runtime.CompilerServices;
using Day = Eventor.Common.Core.Day;

namespace Eventor.GUI.Controllers;

public class EventOrganizationFormController : INotifyPropertyChanged
{
    private const double _errorValue = -(double)ErrorType.NOT_A_NUMBER;

    private readonly IServiceProvider _serviceProvider;
    private readonly IEventService _eventService;
    private readonly IDayService _dayService;
    private readonly IEconomyService _economyService;
    private readonly IPersonService _personService;
    private readonly ILocationService _locationService;

    public Guid EventId { get; set; }
    public Guid UserId { get; set; }
    public double MaxPrice { get; set; } = 30000;

    private double _eventCost = 0;
    public double CurrentEventCost
    {
        get => _eventCost;
        set
        {
            _eventCost = value;
            OnPropertyChanged();
        }
    }

    private double _eventDaysAverageCost = 0;
    public double CurrentEventDaysAverageCost
    {
        get => _eventDaysAverageCost;
        set
        {
            _eventDaysAverageCost = value;
            OnPropertyChanged();
        }
    }

    private bool _eventSolutionExists = false;
    public bool CurrentEventSolutionExists
    {
        get => _eventSolutionExists;
        set
        {
            _eventSolutionExists = value;
            OnPropertyChanged();
        }
    }

    private bool _eventSolutionExistsWithPrivileges = false;
    public bool CurrentEventSolutionExistsWithPrivileges
    {
        get => _eventSolutionExistsWithPrivileges;
        set
        {
            _eventSolutionExistsWithPrivileges = value;
            OnPropertyChanged();
        }
    }

    private int _eventN = 0;
    public int CurrentEventN
    {
        get => _eventN;
        set
        {
            _eventN = value;
            OnPropertyChanged();
        }
    }

    private int _eventDaysBefore = 0;
    public int CurrentEventDaysBefore
    {
        get => _eventDaysBefore;
        set
        {
            _eventDaysBefore = value;
            OnPropertyChanged();
        }
    }


    private double _eventFundamentalPrice = 0;
    public double CurrentEventFundamentalPrice
    {
        get => _eventFundamentalPrice;
        set
        {
            _eventFundamentalPrice = value;
            OnPropertyChanged();
        }
    }

    private double _eventFundamentalPriceWithPrivileges = 0;
    public double CurrentEventFundamentalPriceWithPrivileges
    {
        get => _eventFundamentalPriceWithPrivileges;
        set
        {
            _eventFundamentalPriceWithPrivileges = value;
            OnPropertyChanged();
        }
    }

    private double _eventFundamentalPriceRelativeDifference = 0;
    public double CurrentEventFundamentalPriceRelativeDifference
    {
        get => _eventFundamentalPriceRelativeDifference;
        set
        {
            _eventFundamentalPriceRelativeDifference = value;
            OnPropertyChanged();
        }
    }

    private double _eventDaysAveragePrice = 0;
    public double CurrentEventDaysAveragePrice
    {
        get => _eventDaysAveragePrice;
        set
        {
            _eventDaysAveragePrice = value;
            OnPropertyChanged();
        }
    }

    private double _eventDaysAveragePriceWithPrivileges = 0;
    public double CurrentEventDaysAveragePriceWithPrivileges
    {
        get => _eventDaysAveragePriceWithPrivileges;
        set
        {
            _eventDaysAveragePriceWithPrivileges = value;
            OnPropertyChanged();
        }
    }

    private double _eventPrice = 0;
    public double CurrentEventPrice
    {
        get => _eventPrice;
        set
        {
            _eventPrice = value;
            OnPropertyChanged();
        }
    }

    private double _eventPriceWithPrivileges = 0;
    public double CurrentEventPriceWithPrivileges
    {
        get => _eventPriceWithPrivileges;
        set
        {
            _eventPriceWithPrivileges = value;
            OnPropertyChanged();
        }
    }

    private (double Min, double Max) _eventFundamentalPriceInterval = (0, 0);
    public (double Min, double Max) CurrentEventFundamentalPriceInterval
    {
        get => _eventFundamentalPriceInterval;
        set
        {
            _eventFundamentalPriceInterval = value;
            OnPropertyChanged();
        }
    }


    private double _eventExpenses = 0;
    public double CurrentEventExpenses
    {
        get => _eventExpenses;
        set
        {
            _eventExpenses = value;
            OnPropertyChanged();
        }
    }

    private double _eventIncome = 0;
    public double CurrentEventIncome
    {
        get => _eventIncome;
        set
        {
            _eventIncome = value;
            OnPropertyChanged();
        }
    }

    private double _eventRealProfit = 0;
    public double CurrentEventRealProfit
    {
        get => _eventRealProfit;
        set
        {
            _eventRealProfit = value;
            OnPropertyChanged();
        }
    }

    private double _eventTheoryProfit = 0;
    public double CurrentEventTheoryProfit
    {
        get => _eventTheoryProfit;
        set
        {
            _eventTheoryProfit = value;
            OnPropertyChanged();
        }
    }

    private double _eventMaxMarkup = 0;
    public double CurrentEventMaxMarkup
    {
        get => _eventMaxMarkup;
        set
        {
            _eventMaxMarkup = value;
            OnPropertyChanged();
        }
    }

    private double _eventMinParticipants = 0;
    public double CurrentEventMinParticipants
    {
        get => _eventMinParticipants;
        set
        {
            _eventMinParticipants = value;
            OnPropertyChanged();
        }
    }


    private Location _currentLocation;
    public Location CurrentLocation
    {
        get => _currentLocation;
        set
        {
            _currentLocation = value;
            OnPropertyChanged();
        }
    }

    private List<Location> _allLocations;
    public List<Location> AllLocations
    {
        get => _allLocations;
        set
        {
            _allLocations = value;
            OnPropertyChanged();
        }
    }

    private Event _currentEvent;
    public Event CurrentEvent
    {
        get => _currentEvent;
        set
        {
            _currentEvent = value;
            OnPropertyChanged();
        }
    }


    private BindingList<Day> _eventDays = new BindingList<Day>();
    public BindingList<Day> EventDays
    {
        get => _eventDays;
        set
        {
            _eventDays = value;
            OnPropertyChanged();
        }
    }

    private Dictionary<Guid, int> _daysPersons = new Dictionary<Guid, int>();
    public Dictionary<Guid, int> DaysPersons
    {
        get => _daysPersons;
        set
        {
            _daysPersons = value;
            OnPropertyChanged();
        }
    }

    private Dictionary<Guid, double> _daysCosts = new Dictionary<Guid, double>();
    public Dictionary<Guid, double> DaysCosts
    {
        get => _daysCosts;
        set
        {
            _daysCosts = value;
            OnPropertyChanged();
        }
    }

    private Dictionary<string, Guid> _locationsIds = new Dictionary<string, Guid>();
    public Dictionary<string, Guid> LocationsIds
    {
        get => _locationsIds;
        set
        {
            _locationsIds = value;
            OnPropertyChanged();
        }
    }

    private bool _userIsOrganizer;
    public bool UserIsOrganizer
    {
        get => _userIsOrganizer;
        set
        {
            _userIsOrganizer = value;
            OnPropertyChanged();
        }
    }

    public string EventName => CurrentEvent?.Name ?? string.Empty;
    public string EventLocationName => CurrentLocation?.Name ?? string.Empty;
    public string EventDescription => CurrentEvent?.Description ?? string.Empty;
    public string EventDate => CurrentEvent?.Date.ToString() ?? string.Empty;
    public string EventPersonCount => $"{CurrentEvent?.PersonCount}/{CurrentLocation?.Capacity}";
    public string EventDaysCount => CurrentEvent?.DaysCount.ToString() ?? string.Empty;
    public string EventPercent => CurrentEvent?.Percent.ToString() ?? string.Empty;
    public string EventRating => CurrentEvent?.Rating.ToString() ?? string.Empty;

    public string EventCost => _eventCost.ToString(CultureInfo.InvariantCulture) ?? string.Empty;
    public string EventDaysAverageCost => _eventDaysAverageCost.ToString(CultureInfo.InvariantCulture);
    public string EventSolutionExists => _eventSolutionExists ? "Расчет возможен" : "Расчет невозможен";
    public string EventN => _eventN.ToString();
    public string EventDaysBefore
    {
        get
        {
            if (_eventDaysBefore > 0)
            {
                return _eventDaysBefore.ToString();
            }
            else if (CurrentEvent == null)
            {
                return "0";
            }
            else
            {
                return !CurrentEvent.IsEventExpired() ? "Мероприятие проходит." : "Мероприятие завершилось.";
            }
        }
    }

    public string EventFundamentalPrice => ValueCheck(_eventFundamentalPrice);

    public string EventFundamentalPriceWithPrivileges => ValueCheck(_eventFundamentalPriceWithPrivileges);

    public string EventFundamentalPriceRelativeDifference => 
        _eventFundamentalPriceRelativeDifference != _errorValue ? $"{_eventFundamentalPriceRelativeDifference}%" : "Расчет невозможен";

    public string EventDaysAveragePrice => ValueCheck(_eventDaysAveragePrice);

    public string EventDaysAveragePriceWithPrivileges => ValueCheck(_eventDaysAveragePriceWithPrivileges);

    public string EventFundamentalPriceInterval =>
        (_eventFundamentalPriceInterval.Item1 != _errorValue && _eventFundamentalPriceInterval.Item2 != _errorValue) ?
        $"({ValueCheck(_eventFundamentalPriceInterval.Item1)}, " +
        $"{ValueCheck(_eventFundamentalPriceInterval.Item2)})" : "Расчет невозможен";

    public string EventPrice => ValueCheck(_eventPrice);

    public string EventPriceWithPrivileges => ValueCheck(_eventPriceWithPrivileges);


    public string EventExpenses => ValueCheck(_eventExpenses);

    public string EventIncome => ValueCheck(_eventIncome);

    public string EventRealProfit => ValueCheck(_eventRealProfit);

    public string EventTheoryProfit => ValueCheck(_eventTheoryProfit);

    public string EventMaxMarkup => 
        _eventMaxMarkup != _errorValue ? $"{Math.Round(_eventMaxMarkup, 2)}%" : "0%";

    public string EventMinParticipants => ValueCheck(_eventMinParticipants);


    public event PropertyChangedEventHandler PropertyChanged;

    public EventOrganizationFormController(
        IServiceProvider serviceProvider,
        IEventService eventService,
        IDayService dayService,
        IEconomyService economyService,
        IPersonService personService,
        ILocationService locationService)
    {
        _serviceProvider = serviceProvider;
        _eventService = eventService;
        _dayService = dayService;
        _economyService = economyService;
        _personService = personService;
        _locationService = locationService;
    }

    public async Task InitializeAsync()
    {
        try
        {
            CurrentEvent = await _eventService.GetEventByIdAsync(EventId);
            CurrentLocation = await _locationService.GetLocationByIdAsync(CurrentEvent.LocationId);
            AllLocations = await _locationService.GetAllLocationsAsync();

            await LoadEventDays();
            await LoadDaysPersons();
            await LoadDaysCosts();
            await LoadLocationsIds();
            await LoadEventInformation();
        }
        catch (Exception ex)
        {
            throw;
        }
    }

    private async Task LoadEventInformation()
    {
        try
        {
            UserIsOrganizer = await IsUserOrganizer();

            // Общая информация

            CurrentEventCost = await _economyService.GetEventCostAsync(EventId);

            CurrentEventDaysAverageCost = Math.Round(DaysCosts.Values.Average(), 2);

            CurrentEventSolutionExists = await _economyService.CheckSolutionExistenceAsync(EventId);

            CurrentEventSolutionExistsWithPrivileges = await _economyService.CheckSolutionExistenceWithPrivilegesAsync(EventId);

            CurrentEventN = await _economyService.GetEventNAsync(EventId);

            CurrentEventDaysBefore = await _economyService.GetDaysCountBeforeEvent(EventId);

            // Ценообразование

            CurrentEventFundamentalPrice = CurrentEventSolutionExists ? 
                await _economyService.CalculateFundamentalPriceNDAsync(EventId) : _errorValue;

            CurrentEventFundamentalPriceWithPrivileges = CurrentEventSolutionExistsWithPrivileges ?
                await _economyService.CalculateFundamentalPriceWithPrivilegesNDAsync(EventId) : _errorValue;

            CurrentEventFundamentalPriceRelativeDifference = CurrentEventSolutionExists && CurrentEventSolutionExistsWithPrivileges ?
                GetRelativeDiff(CurrentEventFundamentalPriceWithPrivileges, CurrentEventFundamentalPrice) : _errorValue;

            CurrentEventDaysAveragePrice = CurrentEventSolutionExists ?
                await GetDaysAveragePriceAsync() : _errorValue;

            CurrentEventDaysAveragePriceWithPrivileges = CurrentEventSolutionExistsWithPrivileges ?
                await GetDaysAveragePriceWithPrivilegesAsync() : _errorValue;

            CurrentEventFundamentalPriceInterval = CurrentEventSolutionExists ?
                await _economyService.CalculateFundamentalPriceIntervalAsync(EventId) : (_errorValue, _errorValue);

            CurrentEventPrice = CurrentEventSolutionExists ?
                await _economyService.GetEventPriceAsync(EventId) : _errorValue;

            CurrentEventPriceWithPrivileges = CurrentEventSolutionExistsWithPrivileges ? 
                await _economyService.GetEventPriceWithPrivilegesAsync(EventId) : _errorValue;

            // Прибыль

            CurrentEventExpenses = CurrentEventCost;

            CurrentEventIncome = CurrentEventSolutionExistsWithPrivileges ? 
                await _economyService.CalculateCurrentIncomeAsync(EventId) : _errorValue;

            CurrentEventRealProfit = CurrentEventSolutionExistsWithPrivileges ? 
                CurrentEventIncome - CurrentEventExpenses : -CurrentEventExpenses;

            CurrentEventTheoryProfit = (CurrentEvent.Percent / 100) * CurrentEventCost;

            CurrentEventMaxMarkup = CurrentEventSolutionExistsWithPrivileges ?
                await _economyService.CalculateMaxMarkupAsync(EventId, MaxPrice) : _errorValue;

            CurrentEventMinParticipants = await _economyService.CalculateCriticalParticipantsCountAsync(EventId, MaxPrice);
        }
        catch (Exception ex)
        {
            throw;
        }
    }

    private async Task LoadEventDays()
    {
        try
        {
            var days = await _dayService.GetAllDaysByEventAsync(CurrentEvent.Id);
            EventDays = new BindingList<Day>(days.ToList());
        }
        catch (Exception ex)
        {
            throw;
        }
    }

    private async Task LoadDaysPersons()
    {
        try
        {
            _daysPersons.Clear();
            foreach (var day in EventDays)
            {
                _daysPersons[day.Id] = await _economyService.GetPersonCountAsync(day.Id);
            }
        }
        catch (Exception ex)
        {
            throw;
        }
    }

    private async Task LoadDaysCosts()
    {
        try
        {
            _daysCosts.Clear();
            foreach (var day in EventDays)
            {
                _daysCosts[day.Id] = await _economyService.GetDaysCostAsync(new[] { day.Id });
            }
        }
        catch (Exception ex)
        {
            throw;
        }
    }

    private async Task LoadLocationsIds()
    {
        try
        {
            _locationsIds.Clear();
            foreach (var loc in AllLocations)
            {
                _locationsIds[loc.Name] = loc.Id;
            }
        }
        catch (Exception ex)
        {
            throw;
        }
    }

    private async void UpdateOrganizerDays()
    {
        var userPerson = await _personService.GetPersonByUserAndEventAsync(UserId, EventId);
        var personDays = await _dayService.GetAllDaysByPersonAsync(userPerson.Id);
        var eventDays = await _dayService.GetAllDaysByEventAsync(EventId);

        foreach (var day in eventDays)
        {
            if (!personDays.Contains(day))
                await _dayService.AddPersonToDayAsync(userPerson.Id, day.Id);
        }
    }

    private double GetRelativeDiff(double measured, double real)
    {
        try
        {
            return real != 0 ? Math.Round((measured - real) / real * 100, 2) : 0;
        }
        catch (Exception ex)
        {
            throw;
        }
    }

    private string ValueCheck(int value)
    {
        try
        {
            if (value == null || value.Equals(_errorValue))
                return "Расчет невозможен";

            return value.ToString();
        }
        catch (Exception ex)
        {
            throw;
        }
    }

    private string ValueCheck(double value)
    {
        try
        {
            if (value == null || value.Equals(_errorValue))
                return "Расчет невозможен";

            return Math.Round(value, 2).ToString();
        }
        catch (Exception ex)
        {
            throw;
        }
    }

    private async Task<double> GetDaysAveragePriceAsync()
    {
        try
        {
            // Создаем список для хранения цен
            var prices = new List<double>();

            foreach (var day in EventDays)
            {
                double price = await _economyService.GetDayPriceAsync(day.Id);
                prices.Add(price);
            }

            // Возвращаем среднее значение цен
            return Math.Round(prices.Average(), 2);
        }
        catch (Exception ex)
        {
            throw;
        }
    }

    private async Task<double> GetDaysAveragePriceWithPrivilegesAsync()
    {
        try
        {
            // Создаем список для хранения цен
            var prices = new List<double>();

            foreach (var day in EventDays)
            {
                double price = await _economyService.GetDayPriceWithPrivilegesAsync(day.Id);
                prices.Add(price);
            }

            // Возвращаем среднее значение цен
            return Math.Round(prices.Average(), 2);
        }
        catch (Exception ex)
        {
            throw;
        }
    }

    public async Task<double> GetDayParticipantsCountAsync(Guid dayId)
    {
        return await _economyService.GetPersonCountAsync(dayId);
    }

    public async Task<string> GetFirstDayPrice()
    {
        var price = await _economyService.GetDayPriceWithPrivilegesAsync(EventDays[0].Id);
        return price.ToString();
    }

    public async Task<string> GetFundamentalPrice()
    {
        var fPrice = await _economyService.CalculateFundamentalPriceWithPrivilegesNDAsync(CurrentEvent.Id);
        return fPrice.ToString();
    }

    private async Task<bool> IsUserOrganizer()
    {
        var userPerson = await _personService.GetPersonByUserAndEventAsync(UserId, EventId);
        return userPerson?.Type == Common.Enums.PersonType.Organizer;
    }

    public async Task SaveEvent(Guid locationId, string name, string description, string date, 
        int daysCount, double percent, double maxPrice)
    {
        if (string.IsNullOrEmpty(name))
            throw new ArgumentNullException("name");

        if (string.IsNullOrEmpty(description))
            throw new ArgumentNullException("description");

        if (string.IsNullOrEmpty(date))
            throw new ArgumentNullException("date");

        if (percent < 0)
            throw new ArgumentOutOfRangeException("percent");

        if (maxPrice <= 0)
            throw new ArgumentOutOfRangeException("maxPrice");

        if (!DateOnly.TryParse(date, out var updatedDate))
            throw new InvalidDataException("date");

        var updatedEvent = new Event(
            id: CurrentEvent.Id,
            locationId: locationId,
            name: name,
            description: description,
            date: updatedDate,
            personCount: CurrentEvent.PersonCount,
            daysCount: daysCount,
            percent: percent,
            rating: CurrentEvent.Rating
        );

        await _eventService.UpdateEventAsync(updatedEvent);
        UpdateOrganizerDays();
        MaxPrice = maxPrice;
    }

    public void OpenDayOrganization(Guid dayId)
    {
        var dayOrganizationForm = _serviceProvider.GetRequiredService<DayOrganizationForm>();
        dayOrganizationForm.SetIds(dayId, EventId, UserId);
        dayOrganizationForm.ShowDialog();
    }

    public void OpenLocationCreate()
    {
        var locationCreateForm = _serviceProvider.GetRequiredService<LocationCreateForm>();
        locationCreateForm.ShowDialog();
    }

    public void OpenFeedbackCreate()
    {
        var feedbackCreateForm = _serviceProvider.GetRequiredService<FeedbackCreateForm>();
        feedbackCreateForm.SetIds(EventId, UserId);
        feedbackCreateForm.ShowDialog();
    }

    public void OpenFeedbacks()
    {
        var feedbacksForm = _serviceProvider.GetRequiredService<FeedbacksForm>();
        feedbacksForm.SetIds(EventId, UserId);
        feedbacksForm.ShowDialog();
    }

    public void OpenParticipation()
    {
        var participationForm = _serviceProvider.GetRequiredService<ParticipationForm>();
        participationForm.SetIds(EventId, UserId);
        participationForm.ShowDialog();
    }

    public void OpenParticipants()
    {
        var participantsForm = _serviceProvider.GetRequiredService<ParticipantsForm>();
        participantsForm.SetIds(EventId, UserId);
        participantsForm.ShowDialog();
    }

    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
