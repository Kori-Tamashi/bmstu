using Eventor.Common.Core;
using Eventor.Services;
using Microsoft.Extensions.DependencyInjection;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Day = Eventor.Common.Core.Day;

namespace Eventor.GUI.Controllers;

public class EventFormController : INotifyPropertyChanged
{
    private readonly IServiceProvider _serviceProvider;
    private readonly IEventService _eventService;
    private readonly IDayService _dayService;
    private readonly IEconomyService _economyService;
    private readonly ILocationService _locationService;

    public Guid EventId { get; set; }
    public Guid UserId { get; set; }

    private Event _currentEvent;
    private Location _currentLocation;
    private BindingList<Day> _eventDays = new BindingList<Day>();
    private Dictionary<Guid, int> _daysPersons = new Dictionary<Guid, int>();

    public Location CurrentLocation
    {
        get => _currentLocation;
        set
        {
            _currentLocation = value;
            OnPropertyChanged();
        }
    }

    public Event CurrentEvent
    {
        get => _currentEvent;
        set
        {
            _currentEvent = value;
            OnPropertyChanged();
            OnPropertyChanged(nameof(EventDescription));
            OnPropertyChanged(nameof(EventPersonCount));
            OnPropertyChanged(nameof(EventDaysCount));
        }
    }

    public BindingList<Day> EventDays
    {
        get => _eventDays;
        set
        {
            _eventDays = value;
            OnPropertyChanged();
        }
    }

    public string EventDescription => CurrentEvent?.Description ?? string.Empty;
    public string EventPersonCount => $"Участников: {CurrentEvent?.PersonCount}/{CurrentLocation?.Capacity}";
    public string EventDaysCount => $"Дней: {CurrentEvent?.DaysCount}";

    public event PropertyChangedEventHandler PropertyChanged;

    public EventFormController(
        IServiceProvider serviceProvider,
        IEventService eventService,
        IDayService dayService,
        IEconomyService economyService,
        ILocationService locationService)
    {
        _serviceProvider = serviceProvider;
        _eventService = eventService;
        _dayService = dayService;
        _economyService = economyService;
        _locationService = locationService;
    }

    public async Task InitializeAsync()
    {
        try
        {
            CurrentEvent = await _eventService.GetEventByIdAsync(EventId);
            CurrentLocation = await _locationService.GetLocationByIdAsync(CurrentEvent.LocationId);
            await LoadEventDays();
            await LoadDaysPersons();
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

    public int GetDayPersonsCount(Guid dayId)
    {
        try
        {
            return _daysPersons.TryGetValue(dayId, out int count) ? count : 0;
        }
        catch (Exception ex)
        {
            throw;
        }
    }

    public async Task OpenFeedbackCreating()
    {
        try
        {
            var feedbackForm = _serviceProvider.GetRequiredService<FeedbackCreateForm>();
            feedbackForm.SetIds(EventId, UserId);
            feedbackForm.ShowDialog();
        }
        catch (Exception ex)
        {
            throw;
        }
    }

    public async Task OpenParticipationCreating()
    {
        try
        {
            var participationForm = _serviceProvider.GetRequiredService<ParticipationForm>();
            participationForm.SetIds(EventId, UserId);
            participationForm.ShowDialog();
        }
        catch (Exception ex)
        {
            throw;
        }
    }

    public async Task OpenDayInfo(Guid dayId)
    {
        try
        {
            var dayForm = _serviceProvider.GetRequiredService<DayForm>();
            dayForm.SetIds(dayId, EventId, UserId);
            dayForm.ShowDialog();
        }
        catch (Exception ex)
        {
            throw;
        }
    }

    public async Task OpenFeedbacks()
    {
        try
        {
            var feedbacksForm = _serviceProvider.GetRequiredService<FeedbacksForm>();
            feedbacksForm.SetIds(EventId, UserId);
            feedbacksForm.ShowDialog();
        }
        catch (Exception ex)
        {
            throw;
        }
    }

    public async Task OpenParticipants()
    {
        try
        {
            var participantsForm = _serviceProvider.GetRequiredService<ParticipantsForm>();
            participantsForm.SetIds(EventId, UserId);
            participantsForm.ShowDialog();
        }
        catch (Exception ex)
        {
            throw;
        }
    }

    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}