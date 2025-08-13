using Eventor.Common.Core;
using Eventor.Services;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Eventor.GUI.Controllers;

public class FeedbackFormController : INotifyPropertyChanged
{
    private readonly IEventService _eventService;
    private readonly IFeedbackService _feedbackService;
    private readonly IPersonService _personService;

    public Guid EventId { get; set; }
    public Guid UserId { get; set; }
    public Guid FeedbackId { get; set; }

    public string Comment => $"{Feedback?.Comment}";
    public string Rating => $"Рейтинг: {Feedback?.Rating}/10";

    private string _title;
    public string Title 
    {
        get => _title;
        set
        {
            _title = value;
            OnPropertyChanged();
        }
    }

    private Feedback _feedback;
    public Feedback Feedback
    {
        get => _feedback;
        set
        {
            _feedback = value;
            OnPropertyChanged();
        }
    }

    private bool _isOwnedBy;
    public bool IsOwnedBy
    {
        get => _isOwnedBy;
        set
        {
            _isOwnedBy = value;
            OnPropertyChanged();
        }
    }

    public event PropertyChangedEventHandler PropertyChanged;

    public FeedbackFormController(
        IEventService eventService,
        IFeedbackService feedbackService,
        IPersonService personService)
    {
        _eventService = eventService;
        _feedbackService = feedbackService;
        _personService = personService;
    }

    public async Task InitializeAsync()
    {
        try
        {
            LoadFeedbackData();
        }
        catch (Exception ex)
        {
            throw;
        }
    }

    public async void LoadFeedbackData()
    {
        try
        {
            // Feedback info
            Feedback = await _feedbackService.GetFeedbackByIdAsync(FeedbackId);
            var feedbackEvent = await _eventService.GetEventByIdAsync(Feedback.EventId);
            var feedbackPerson = await _personService.GetPersonByIdAsync(Feedback.PersonId);

            Title = $"Отзыв участника {feedbackPerson.Name} о мероприятии \"{feedbackEvent.Name}\"";

            // Delete button settings
            var userPerson = await _personService.GetPersonByUserAndEventAsync(UserId, Feedback.EventId);
            IsOwnedBy = (feedbackPerson.Id == userPerson?.Id);
        }
        catch (Exception ex)
        {
            throw;
        }
    }

    public async Task DeleteFeedbackAsync()
    {
        try
        {
            await _feedbackService.DeleteFeedbackAsync(FeedbackId);
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