using Eventor.Common.Core;
using Eventor.Database.Core;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Eventor.GUI.Controllers
{
    public class FeedbackFormController : INotifyPropertyChanged
    {
        private readonly IFeedbackService _feedbackService;
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

        public FeedbackFormController(IFeedbackService feedbackService)
        {
            _feedbackService = feedbackService;
        }

        public async Task CreateFeedbackAsync()
        {
            if (string.IsNullOrWhiteSpace(Comment))
                throw new ArgumentException("Комментарий не может быть пустым");

            if (Rating < 0 || Rating > 10)
                throw new ArgumentException("Рейтинг должен быть от 0 до 10");

            var feedback = new Feedback(
                id: Guid.NewGuid(),
                eventId: EventId,
                personId: UserId,
                comment: Comment,
                rating: Rating
            );

            await _feedbackService.AddFeedbackAsync(feedback);
        }

        public void ResetForm()
        {
            Comment = string.Empty;
            Rating = 0;
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}