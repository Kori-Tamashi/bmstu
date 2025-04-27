using Eventor.GUI.Controllers;

namespace Eventor.Eventor.GUI;

public partial class FeedbackForm : Form
{
    private readonly FeedbackFormController _feedbackFormController;

    public FeedbackForm(FeedbackFormController feedbackFormcontroller)
    {
        _feedbackFormController = feedbackFormcontroller;
        InitializeComponent();
        InitializeBindings();
    }

    private void InitializeBindings()
    {
        feedbackComment_textBox.DataBindings.Add("Text", _feedbackFormController, "Comment");
        feedbackRating_numericUpDown.DataBindings.Add("Value", _feedbackFormController, "Rating");
    }

    private async void createFeedback_button_Click(object sender, EventArgs e)
    {
        try
        {
            await _feedbackFormController.CreateFeedbackAsync();
            MessageBox.Show("Отзыв успешно сохранен!");

            _feedbackFormController.ResetForm();
            Close();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Ошибка: {ex.Message}");
            return;
        }
    }

    public void SetIds(Guid eventId, Guid userId)
    {
        _feedbackFormController.EventId = eventId;
        _feedbackFormController.UserId = userId;
    }
}