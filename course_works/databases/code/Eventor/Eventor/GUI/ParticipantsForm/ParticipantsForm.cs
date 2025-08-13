using Eventor.GUI.Controllers;

namespace Eventor.GUI;

public partial class ParticipantsForm : Form
{
    private readonly ParticipantsFormController _participantsFormController;

    public ParticipantsForm(ParticipantsFormController participantsFormController)
    {
        _participantsFormController = participantsFormController;
        InitializeComponent();
        InitializeBindings();
    }

    public void SetIds(Guid eventId, Guid userId)
    {
        try
        {
            _participantsFormController.EventId = eventId;
            _participantsFormController.UserId = userId;
        }
        catch
        {
            MessageBox.Show("Ошибка: не удалось передать данные в форму участников мероприятия.");
        }
    }

    private void InitializeBindings()
    {
        try
        {
            var bindingSource = new BindingSource { DataSource = _participantsFormController };
            title_label.DataBindings.Add("Text", bindingSource, "MainLabel");
        }
        catch
        {
            MessageBox.Show("Ошибка: не удалось произвести связывание данных между формой участников мероприятия и контроллером.");
        }
    }

    private void InitializeTimer()
    {
        try
        {
            _timer.Tick += async (sender, e) =>
            {
                await _participantsFormController.InitializeAsync();
                InitializeParticipantsGrid();
            };
            _timer.Start();
        }
        catch
        {
            MessageBox.Show("Ошибка: не удалось автоматически обновить данные формы информации об отзывах по таймеру.");
            return;
        }
    }

    private async void Participants_Load(object sender, EventArgs e)
    {
        try
        {
            await _participantsFormController.InitializeAsync();
            InitializeParticipantsGrid();
            InitializeTimer();
        }
        catch
        {
            MessageBox.Show("Ошибка: не удалось произвести загрузку формы участников мероприятия.");
            Close();
        }
    }

    private void InitializeParticipantsGrid()
    {
        try
        {
            participants_dataGridView.AutoGenerateColumns = false;
            participants_dataGridView.Rows.Clear();

            foreach (var participant in _participantsFormController.Participants)
            {
                participants_dataGridView.Rows.Add(
                    participant.Id,
                    participant.Name,
                    participant.Paid ? "Оплачено" : "Не оплачено" 
                );
            }
        }
        catch
        {
            throw;
        }
    }
}