using Eventor.Common.Core;
using Eventor.Services;
using Microsoft.Extensions.DependencyInjection;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Eventor.GUI.Controllers;

public class FeedbacksFormController : INotifyPropertyChanged
{
    private readonly IServiceProvider _serviceProvider;
    private readonly IFeedbackService _feedbackService;
    private readonly IPersonService _personService;
    private readonly IEventService _eventService;

    public Guid EventId { get; set; }
    public Guid UserId { get; set; }

    private List<Feedback> _feedbacks = new();

    public List<Feedback> Feedbacks
    {
        get => _feedbacks;
        set
        {
            _feedbacks = value;
            OnPropertyChanged();
        }
    }

    private Event _currentEvent { get; set; }

    public Event CurrentEvent
    {
        get => _currentEvent;
        set
        {
            _currentEvent = value;
            OnPropertyChanged();
        }
    }

    public string MainLabel => $"Отзывы о мероприятии\n\"{CurrentEvent?.Name}\"";

    public event PropertyChangedEventHandler PropertyChanged;

    public FeedbacksFormController(
        IServiceProvider serviceProvider,
        IFeedbackService feedbackService,
        IPersonService personService,
        IEventService eventService)
    {
        _serviceProvider = serviceProvider;
        _feedbackService = feedbackService;
        _personService = personService;
        _eventService = eventService;
    }

    public async Task InitializeAsync()
    {
        try
        {
            Feedbacks = await _feedbackService.GetAllFeedbacksByEventAsync(EventId);
            CurrentEvent = await _eventService.GetEventByIdAsync(EventId);
        }
        catch (Exception ex)
        {
            throw;
        }
    }

    public async Task<string> GetFeedbackAuthorName(Guid personId)
    {
        try
        {
            var person = await _personService.GetPersonByIdAsync(personId);
            return person.Name;
        }
        catch (Exception ex)
        {
            throw;
        }
    }

    public void OpenFeedbackInfo(Guid feedbackId)
    {
        try
        {
            var feedbackForm = _serviceProvider.GetRequiredService<FeedbackForm>();
            feedbackForm.SetIds(feedbackId, EventId, UserId);
            feedbackForm.ShowDialog();
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