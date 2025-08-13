using Eventor.Common.Core;
using Eventor.Services;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Eventor.GUI.Controllers;

public class ParticipantsFormController : INotifyPropertyChanged
{
    private readonly IPersonService _personService;
    private readonly IEventService _eventService;

    public Guid EventId { get; set; }
    public Guid UserId { get; set; }

    private List<Person> _participants = new();
    public List<Person> Participants
    {
        get => _participants;
        set
        {
            _participants = value;
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

    public string MainLabel => $"Участники мероприятия \"{CurrentEvent?.Name}\"";

    public event PropertyChangedEventHandler PropertyChanged;

    public ParticipantsFormController(
        IPersonService personService,
        IEventService eventService)
    {
        _personService = personService;
        _eventService = eventService;
    }

    public async Task InitializeAsync()
    {
        try
        {
            Participants = await _personService.GetAllPersonsByEventAsync(EventId);
            CurrentEvent = await _eventService.GetEventByIdAsync(EventId);
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