using Eventor.GUI.Controllers;

namespace Eventor.GUI;

public partial class FeedbackForm : Form
{
    private readonly FeedbackFormController _feedbackController;

    public FeedbackForm(FeedbackFormController feedbackController)
    {
        _feedbackController = feedbackController;
        InitializeComponent();
        InitializeBindings();
    }

    private void InitializeBindings()
    {
        try
        {
            var feedbackBindingSource = new BindingSource { DataSource = _feedbackController };

            feedbackTitle_label.DataBindings.Add("Text", feedbackBindingSource, "Title");
            comment_textBox.DataBindings.Add("Text", feedbackBindingSource, "Comment");
            rating_label.DataBindings.Add("Text", feedbackBindingSource, "Rating");
            delete_button.DataBindings.Add("Visible", feedbackBindingSource, "IsOwnedBy");
        }
        catch (Exception ex)
        {
            MessageBox.Show("Ошибка: не удалось произвести связывание данных между формой информации об отзыве и контроллером.");
            return;
        }
    }

    private void InitializeTimer()
    {
        try
        {
            _timer.Tick += async (sender, e) =>
            {
                await _feedbackController.InitializeAsync();
            };
            _timer.Start();
        }
        catch (Exception ex)
        {
            MessageBox.Show("Ошибка: не удалось автоматически обновить данные формы информации об отзыве по таймеру.");
            return;
        }
    }

    private async void FeedbackForm_Load(object sender, EventArgs e)
    {
        try
        {
            await _feedbackController.InitializeAsync();
            InitializeTimer();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Ошибка: не удалось произвести загрузку формы информации об отзыве.");
            return;
        }
    }

    public void SetIds(Guid feedbackId, Guid eventId, Guid userId)
    {
        try
        {
            _feedbackController.FeedbackId = feedbackId;
            _feedbackController.EventId = eventId;
            _feedbackController.UserId = userId;
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Ошибка: не удалось передать данные в форму инфорации об отзыве.");
            return;
        }
    }

    private async void delete_button_Click(object sender, EventArgs e)
    {
        try
        {
            await _feedbackController.DeleteFeedbackAsync();
            MessageBox.Show("Отзыв успешно удален.");
            Close();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Ошибка: не удалось удалить отзыв.");
            return;
        }
    }
}