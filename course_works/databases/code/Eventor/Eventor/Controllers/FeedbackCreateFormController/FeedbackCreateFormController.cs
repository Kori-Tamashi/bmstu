using Eventor.Common.Core;
using Eventor.Services;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Eventor.GUI.Controllers
{
    public class FeedbackCreateFormController : INotifyPropertyChanged
    {
        private readonly IFeedbackService _feedbackService;
        private readonly IPersonService _personService;
        private string _comment;
        private double _rating;

        public Guid EventId { get; set; }
        public Guid UserId { get; set; }

        public string Comment
        {
            get => _comment;
            set
            {
                _comment = value;
                OnPropertyChanged();
            }
        }

        public double Rating
        {
            get => _rating;
            set
            {
                _rating = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public FeedbackCreateFormController(IFeedbackService feedbackService, IPersonService personService)
        {
            _feedbackService = feedbackService;
            _personService = personService;
        }

        public async Task CreateFeedbackAsync()
        {
            if (string.IsNullOrWhiteSpace(Comment))
                throw new ArgumentException("Комментарий не может быть пустым");

            if (Rating < 0 || Rating > 10)
                throw new ArgumentException("Рейтинг должен быть от 0 до 10");

            try
            {
                var userPerson = await _personService.GetPersonByUserAndEventAsync(UserId, EventId);

                var feedback = new Feedback(
                    id: Guid.NewGuid(),
                    eventId: EventId,
                    personId: userPerson.Id,
                    comment: Comment,
                    rating: Rating
                );

                await _feedbackService.AddFeedbackAsync(feedback);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public void ResetForm()
        {
            try
            {
                Comment = string.Empty;
                Rating = 0;
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
}