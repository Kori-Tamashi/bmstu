using Eventor.GUI.Controllers;

namespace Eventor.Eventor.GUI;

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
        var eventBindingSource = new BindingSource { DataSource = _eventFormController };

        // Event info
        eventName_label.DataBindings.Add("Text", eventBindingSource, "CurrentEvent.Name");
        eventDescription_textBox.DataBindings.Add("Text", eventBindingSource, "EventDescription");
        eventDaysCount_label.DataBindings.Add("Text", eventBindingSource, "EventDaysCount");
    }

    private void InitializeEventDaysGrid()
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

    private async void EventForm_Load(object sender, EventArgs e)
    {
        await _eventFormController.InitializeAsync();
        InitializeEventDaysGrid();
        InitializeTimer();
    }

    private void InitializeTimer()
    {
        _timer.Tick += async (sender, e) =>
        {
            await _eventFormController.InitializeAsync();
            InitializeEventDaysGrid();
        };
        _timer.Start();
    }

    public void SetIds(Guid eventId, Guid userId)
    {
        _eventFormController.EventId = eventId;
        _eventFormController.UserId = userId;
    }

    private async void feedback_button_Click(object sender, EventArgs e)
    {
        await _eventFormController.OpenFeedbackCreating();
    }

    private void participation_button_Click(object sender, EventArgs e)
    {

    }
}