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

    public Guid EventId { get; set; }
    public Guid UserId { get; set; }

    private Event _currentEvent;
    private BindingList<Day> _eventDays = new BindingList<Day>();
    private Dictionary<Guid, int> _daysPersons = new Dictionary<Guid, int>();

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
    public string EventPersonCount => $"Участников: {CurrentEvent?.PersonCount}";
    public string EventDaysCount => $"Дней: {CurrentEvent?.DaysCount}";

    public event PropertyChangedEventHandler PropertyChanged;

    public EventFormController(
        IServiceProvider serviceProvider,
        IEventService eventService,
        IDayService dayService,
        IEconomyService economyService)
    {
        _serviceProvider = serviceProvider;
        _eventService = eventService;
        _dayService = dayService;
        _economyService = economyService;
    }

    public async Task InitializeAsync()
    {
        CurrentEvent = await _eventService.GetEventByIdAsync(EventId);
        await LoadEventDays();
        await LoadDaysPersons();
    }

    private async Task LoadEventDays()
    {
        var days = await _dayService.GetAllDaysByEventAsync(CurrentEvent.Id);
        EventDays = new BindingList<Day>(days.ToList());
    }

    private async Task LoadDaysPersons()
    {
        _daysPersons.Clear();
        foreach (var day in EventDays)
        {
            _daysPersons[day.Id] = await _economyService.GetPersonCountAsync(day.Id);
        }
    }

    public int GetDayPersonsCount(Guid dayId)
    {
        return _daysPersons.TryGetValue(dayId, out int count) ? count : 0;
    }

    public async Task OpenFeedbackCreating()
    {
        try
        {
            var feedbackForm = _serviceProvider.GetRequiredService<FeedbackForm>();
            feedbackForm.SetIds(EventId, UserId);
            feedbackForm.Show();
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message);
            return;
        }
    }

    public async Task OpenParticipationCreating()
    {
        try
        {
            var participationForm = _serviceProvider.GetRequiredService<ParticipationForm>();
            participationForm.SetIds(EventId, UserId);
            participationForm.Show();
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message);
            return;
        }
    }

    public async Task OpenDayInfo(Guid dayId)
    {
        try
        {
            var dayForm = _serviceProvider.GetRequiredService<DayForm>();
            dayForm.SetIds(dayId, EventId, UserId);
            dayForm.Show();
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message);
            return;
        }
    }

    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}