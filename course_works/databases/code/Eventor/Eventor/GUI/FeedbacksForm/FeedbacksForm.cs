using Eventor.GUI.Controllers;

namespace Eventor.GUI
{
    public partial class FeedbacksForm : Form
    {
        private readonly FeedbacksFormController _feedbacksFormController;

        public FeedbacksForm(FeedbacksFormController feedbacksFormController)
        {
            _feedbacksFormController = feedbacksFormController;
            InitializeComponent();
            InitializeBindings();
        }

        public void SetIds(Guid eventId, Guid userId)
        {
            try
            {
                _feedbacksFormController.EventId = eventId;
                _feedbacksFormController.UserId = userId;
            }
            catch
            {
                MessageBox.Show("Ошибка: не удалось передать данные в форму отзывов мероприятия.");
                return;
            }
        }

        private void InitializeBindings()
        {
            try
            {
                var feedbacksBindingSource = new BindingSource { DataSource = _feedbacksFormController };
                eventFeedbacks_label.DataBindings.Add("Text", feedbacksBindingSource, "MainLabel");
            }
            catch
            {
                MessageBox.Show("Ошибка: не удалось произвести связывание данных между формой информации об отзывах и контроллером.");
                return;
            }
        }

        private async void InitializeFeedbacksGrid()
        {
            try
            {
                feedbacks_dataGridView.AutoGenerateColumns = false;
                feedbacks_dataGridView.Rows.Clear();

                foreach (var feedback in _feedbacksFormController.Feedbacks)
                {
                    feedbacks_dataGridView.Rows.Add(
                        feedback.Id,
                        feedback.EventId,
                        feedback.PersonId,
                        await _feedbacksFormController.GetFeedbackAuthorName(feedback.PersonId),
                        feedback.Comment,
                        feedback.Rating
                    );
                }
            }
            catch
            {
                throw;
            }
        }

        private void InitializeTimer()
        {
            try
            {
                _timer.Tick += async (sender, e) =>
                {
                    await _feedbacksFormController.InitializeAsync();
                    InitializeFeedbacksGrid();
                };
                _timer.Start();
            }
            catch
            {
                MessageBox.Show("Ошибка: не удалось автоматически обновить данные формы информации об отзывах по таймеру.");
                return;
            }
        }

        private async void FeedbacksForm_Load(object sender, EventArgs e)
        {
            try
            {
                await _feedbacksFormController.InitializeAsync();
                InitializeFeedbacksGrid();
                InitializeTimer();
            }
            catch
            {
                MessageBox.Show("Ошибка: не удалось загрузить форму отзывов мероприятия.");
                Close();
            }
        }

        private void feedbacks_dataGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex < 0) return;
                var row = feedbacks_dataGridView.Rows[e.RowIndex];
                var feedbackId = (Guid)row.Cells[0].Value;

                _feedbacksFormController.OpenFeedbackInfo(feedbackId);
            }
            catch
            {
                MessageBox.Show("Ошибка: не удалось открыть форму информации об отзыве.");
                Close();
            }
        }
    }
}