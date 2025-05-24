using Eventor.Common.Core;
using Eventor.GUI.Controllers;

namespace Eventor.GUI;

public partial class EventCreateForm : Form
{
    private EventCreateFormController _eventCreateFormController;

    public EventCreateForm(EventCreateFormController eventCreateFormController)
    {
        _eventCreateFormController = eventCreateFormController;
        InitializeComponent();
    }

    private async void EventCreateForm_Load(object sender, EventArgs e)
    {
        try
        {
            await _eventCreateFormController.InitializeAsync();
            InitializeLocations();
            InitializeLastUpdate();
        }
        catch
        {
            Close();
            MessageBox.Show("Ошибка: не удалось загрузить форму создания мероприятия.");
        }
    }

    private void InitializeLocations()
    {
        try
        {
            eventLocation_comboBox.Items.Clear();
            foreach (var loc in _eventCreateFormController.AllLocations)
            {
                eventLocation_comboBox.Items.Add(loc);
            }

            eventLocation_comboBox.Items.Add("Добавить");
            eventLocation_comboBox.DisplayMember = "Name";
            eventLocation_comboBox.SelectedIndex = 0;
        }
        catch
        {
            throw;
        }
    }

    private void InitializeLastUpdate()
    {
        dataStatus_toolStripStatusLabel.Text = $"Последнее обновление данных: {DateTime.Now}";
    }

    private void InitializeTimer()
    {
        try
        {
            _timer.Tick += async (sender, e) =>
            {
                try
                {
                    await _eventCreateFormController.InitializeAsync();
                    InitializeLocations();
                    InitializeLastUpdate();
                }
                catch
                {
                    return;
                }
            };
            _timer.Start();
        }
        catch
        {
            return;
        }
    }

    public void SetIds(Guid userId)
    {
        _eventCreateFormController.UserId = userId;
    }

    private async void eventCreate_button_Click(object sender, EventArgs e)
    {
        try
        {
            var newLocation = eventLocation_comboBox.SelectedItem as Location;
            var newName = eventName_textBox.Text.Trim();
            var newDescription = eventDescription_textBox.Text.Trim();
            var newDate = eventDate_maskedTextBox.Text.Trim();
            var newDaysCount = (int)eventDaysCount_numericUpDown.Value;
            var newPercent = (double)eventPercent_numericUpDown.Value;

            await _eventCreateFormController.CreateEventAsync(
                newLocation.Id,
                newName,
                newDescription,
                newDate,
                newDaysCount,
                newPercent
            );

            MessageBox.Show("Мероприятие успешно создано.");
        }
        catch (Exception ex)
        {
            MessageBox.Show("Ошибка: не удалось создать мероприятие.");
        }
    }

    private async void eventLocation_comboBox_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (eventLocation_comboBox.SelectedItem != "Добавить")
            {
                var location = eventLocation_comboBox.SelectedItem as Location;

                string tooltipText = $"{location.Name}\n\n" +
                        $"{location.Description}\n\n" +
                        $"Цена аренды на 1 день: {location.Price}\n" +
                        $"Вместимость: {location.Capacity} человек";

                _toolTip.SetToolTip(eventLocation_comboBox, tooltipText);
            }
            else
            {
                _eventCreateFormController.OpenLocationCreate();
            }
        }
        catch
        {
            return;
        }
    }
}
