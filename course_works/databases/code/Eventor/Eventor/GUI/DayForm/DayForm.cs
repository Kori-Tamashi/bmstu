using Eventor.GUI.Controllers;

namespace Eventor.GUI;

public partial class DayForm : Form
{
    private readonly DayFormController _dayFormController;

    public DayForm(DayFormController dayFormController)
    {
        _dayFormController = dayFormController;
        InitializeComponent();
        InitializeBindings();
    }

    private void InitializeBindings()
    {
        try
        {
            var dayBindingSource = new BindingSource { DataSource = _dayFormController };

            // Day info
            dayName_label.DataBindings.Add("Text", dayBindingSource, "CurrentDay.Name");
            daySequenceNumber_label.DataBindings.Add("Text", dayBindingSource, "CurrentDay.SequenceNumber");
            dayDescription_textBox.DataBindings.Add("Text", dayBindingSource, "CurrentDay.Description");
        }
        catch (Exception ex)
        {
            MessageBox.Show("Ошибка: не удалось произвести связывание данных между формой информации о дне мероприятия и контроллером.");
            return;
        }
    }

    private void InitializeTimer()
    {
        try
        {
            _timer.Tick += async (sender, e) =>
            {
                try
                {
                    await _dayFormController.InitializeDayAsync();
                    LoadParticipants();
                    LoadMenu();
                }
                catch (Exception ex)
                {
                    return;
                }
            };
            _timer.Start();
        }
        catch (Exception ex)
        {
            return;
        }
    }

    private async void DayForm_Load(object sender, EventArgs e)
    {
        try
        {
            await _dayFormController.InitializeDayAsync();
            LoadParticipants();
            LoadMenu();
            InitializeTimer();
        }
        catch (Exception ex)
        {
            MessageBox.Show("Ошибка: не удалось загрузить форму информации о дне мероприятия.");
            Close();
        }
    }

    private void LoadParticipants()
    {
        try
        {
            dayPersons_dataGridView.AutoGenerateColumns = false;
            dayPersons_dataGridView.Rows.Clear();

            foreach (var person in _dayFormController.DayPersons)
            {
                dayPersons_dataGridView.Rows.Add(
                    person.Id,
                    person.Name,
                    person.Paid ? "Оплачено" : "Не оплачено"
                );
            }
        }
        catch (Exception ex)
        {
            throw;
        }
    }

    private async void LoadMenu()
    {
        try
        {
            dayMenu_dataGridView.AutoGenerateColumns = false;
            dayMenu_dataGridView.Rows.Clear();

            foreach (var item in _dayFormController.MenuItems)
            {
                dayMenu_dataGridView.Rows.Add(
                    item.Id,
                    item.Name,
                    await _dayFormController.GetItemAmountAsync(item.Id)
                );
            }
        }
        catch (Exception ex)
        {
            throw;
        }
    }

    public void SetIds(Guid dayId, Guid eventId, Guid userId)
    {
        try
        {
            _dayFormController.SetIds(dayId, eventId, userId);
        }
        catch (Exception ex)
        {
            MessageBox.Show("Ошибка: не удалось передать данные в форму информации о дне мероприятия.");
            return;
        }

    }
}