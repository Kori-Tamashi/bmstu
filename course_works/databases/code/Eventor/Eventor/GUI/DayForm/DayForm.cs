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
                    await LoadParticipants();
                    await LoadMenu();
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
            await LoadParticipants();
            await LoadMenu();
            InitializeTimer();
        }
        catch (Exception ex)
        {
            MessageBox.Show("Ошибка: не удалось загрузить форму информации о дне мероприятия.");
            Close();
        }
    }

    private async Task LoadParticipants()
    {
        try
        {
            await _dayFormController.LoadParticipantsAsync();
            dayPersons_dataGridView.AutoGenerateColumns = false;
            dayPersons_dataGridView.DataSource = _dayFormController.DayPersons.Select(p => new
            {
                p.Id,
                p.Name,
                Paid = p.Paid ? "Оплачено" : "Не оплачено"
            }).ToList();
        }
        catch (Exception ex)
        {
            throw;
        }
    }

    private async Task LoadMenu()
    {
        try
        {
            await _dayFormController.LoadMenuAsync();
            dayMenu_dataGridView.AutoGenerateColumns = false;

            var menuItemsTasks = _dayFormController.MenuItems
                .Select(async i => new
                {
                    i.Id,
                    i.Name,
                    Amount = await _dayFormController.GetItemAmountAsync(i.Id)
                })
                .ToList();

            var menuItems = await Task.WhenAll(menuItemsTasks);
            dayMenu_dataGridView.DataSource = menuItems.ToList();
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