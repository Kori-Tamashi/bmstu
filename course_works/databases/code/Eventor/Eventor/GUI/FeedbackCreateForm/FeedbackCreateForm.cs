using Eventor.GUI.Controllers;

namespace Eventor.GUI;

public partial class FeedbackCreateForm : Form
{
    private readonly FeedbackCreateFormController _feedbackFormController;

    public FeedbackCreateForm(FeedbackCreateFormController feedbackFormcontroller)
    {
        _feedbackFormController = feedbackFormcontroller;
        InitializeComponent();
        InitializeBindings();
    }

    private void InitializeBindings()
    {
        try
        {
            feedbackComment_textBox.DataBindings.Add("Text", _feedbackFormController, "Comment");
            feedbackRating_numericUpDown.DataBindings.Add("Value", _feedbackFormController, "Rating");
        }
        catch (Exception ex)
        {
            MessageBox.Show("Ошибка: не удалось произвести связывание данных между формой создания отзыва и контроллером.");
            return;
        }

    }

    private async void createFeedback_button_Click(object sender, EventArgs e)
    {
        try
        {
            await _feedbackFormController.CreateFeedbackAsync();
            _feedbackFormController.ResetForm();
            MessageBox.Show("Отзыв успешно сохранен!");
            Close();
        }
        catch (Exception ex)
        {
            MessageBox.Show("Ошибка: не удалось создать отзыв.");
            return;
        }
    }

    public void SetIds(Guid eventId, Guid userId)
    {
        try
        {
            _feedbackFormController.EventId = eventId;
            _feedbackFormController.UserId = userId;
        }
        catch (Exception ex)
        {
            MessageBox.Show("Ошибка: не удалось передать данные в форму создания отзыва.");
            return;
        }
    }
}