using Eventor.GUI.Controllers;

namespace Eventor.GUI;

public partial class EventForm : Form
{
    private readonly EventFormController _eventFormController;

    public EventForm(EventFormController eventFormController)
    {
        _eventFormController = eventFormController;
        InitializeComponent();
        InitializeBindings();
    }

    private void InitializeBindings()
    {
        try
        {
            var eventBindingSource = new BindingSource { DataSource = _eventFormController };

            // Event info
            eventName_label.DataBindings.Add("Text", eventBindingSource, "CurrentEvent.Name");
            eventDescription_textBox.DataBindings.Add("Text", eventBindingSource, "EventDescription");
            eventDaysCount_label.DataBindings.Add("Text", eventBindingSource, "EventDaysCount");
            eventPersonCount_label.DataBindings.Add("Text", eventBindingSource, "EventPersonCount");
        }
        catch (Exception ex)
        {
            MessageBox.Show("Ошибка: не удалось произвести связывание данных между формой информации о мероприятии и контроллером.");
            return;
        }
    }

    private void InitializeEventDaysGrid()
    {
        try
        {
            eventDays_dataGridView.AutoGenerateColumns = false;
            eventDays_dataGridView.Rows.Clear();

            foreach (var evt in _eventFormController.EventDays)
            {
                eventDays_dataGridView.Rows.Add(
                    evt.Id,
                    evt.SequenceNumber,
                    evt.Name,
                    evt.Price,
                    _eventFormController.GetDayPersonsCount(evt.Id)
                );
            }
        }
        catch (Exception ex)
        {
            throw;
        }
    }

    private async void EventForm_Load(object sender, EventArgs e)
    {
        try
        {
            await _eventFormController.InitializeAsync();
            InitializeEventDaysGrid();
            InitializeTimer();
        }
        catch (Exception ex)
        {
            MessageBox.Show("Ошибка: не удалось произвести загрузку формы информации о мероприятии.");
            return;
        }
    }

    private void InitializeTimer()
    {
        try
        {
            _timer.Tick += async (sender, e) =>
            {
                await _eventFormController.InitializeAsync();
                InitializeEventDaysGrid();
            };
            _timer.Start();
        }
        catch (Exception ex)
        {
            MessageBox.Show("Ошибка: не удалось автоматически обновить данные формы информации о мероприятии по таймеру.");
            return;
        }
    }

    public void SetIds(Guid eventId, Guid userId)
    {
        try
        {
            _eventFormController.EventId = eventId;
            _eventFormController.UserId = userId;
        }
        catch (Exception ex)
        {
            MessageBox.Show("Ошибка: не удалось передать данные в форму информации о мероприятии.");
            return;
        }
    }

    private async void feedback_button_Click(object sender, EventArgs e)
    {
        try
        {
            await _eventFormController.OpenFeedbackCreating();
        }
        catch (Exception ex)
        {
            MessageBox.Show("Ошибка: не удалось открыть форму создания отзыва.");
            return;
        }
    }

    private async void participation_button_Click(object sender, EventArgs e)
    {
        try
        {
            await _eventFormController.OpenParticipationCreating();
        }
        catch (Exception ex)
        {
            MessageBox.Show("Ошибка: не удалось открыть форму выбора дней присутствия на мероприятии.");
            return;
        }
    }

    private async void eventDays_dataGridView_CellClick(object sender, DataGridViewCellEventArgs e)
    {
        try
        {
            if (e.RowIndex < 0) return;
            var row = eventDays_dataGridView.Rows[e.RowIndex];
            var dayId = (Guid)row.Cells[0].Value;
            await _eventFormController.OpenDayInfo(dayId);
        }
        catch (Exception ex)
        {
            MessageBox.Show("Ошибка: не удалось открыть форму информации о дне.");
            return;
        }
    }

    private async void eventFeedbacks_button_Click(object sender, EventArgs e)
    {
        try
        {
            await _eventFormController.OpenFeedbacks();
        }
        catch (Exception ex)
        {
            MessageBox.Show("Ошибка: не удалось открыть форму отзывов.");
            return;
        }
    }

    private async void eventPersonCount_label_Click(object sender, EventArgs e)
    {
        try
        {
            await _eventFormController.OpenParticipants();
        }
        catch (Exception ex)
        {
            MessageBox.Show("Ошибка: не удалось открыть форму участников мероприятия.");
            return;
        }
    }
}