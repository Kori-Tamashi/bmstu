using Eventor.Common.Core;
using Eventor.GUI.Controllers;
using Microsoft.VisualBasic.Logging;

namespace Eventor.GUI;

public partial class EventOrganizationForm : Form
{
    EventOrganizationFormController _eventOrganizationFormController;

    public EventOrganizationForm(EventOrganizationFormController eventOrganizationFormController)
    {
        _eventOrganizationFormController = eventOrganizationFormController;
        InitializeComponent();
        InitializeBindings();
    }

    private async void EventOrganizationForm_Load(object sender, EventArgs e)
    {
        try
        {
            await _eventOrganizationFormController.InitializeAsync();
            InitializeComponents();
            InitializeLocations();
            InitializeEventInfo();
            InitializeDaysGrid();
            InitializeLastUpdate();
            InitializeTimer();
        }
        catch (Exception ex)
        {
            MessageBox.Show("Ошибка: не удалось произвести загрузку формы организации мероприятия.");
            Close();
        }
    }

    private void InitializeBindings()
    {
        try
        {
            var eventOrganizationController = new BindingSource { DataSource = _eventOrganizationFormController };

            eventParticipantsValue_label.DataBindings.Add("Text", eventOrganizationController, "EventPersonCount");
            eventRatingValue_label.DataBindings.Add("Text", eventOrganizationController, "EventRating");
            eventCostValue_label.DataBindings.Add("Text", eventOrganizationController, "EventCost");
            eventAverageDayCostValue_label.DataBindings.Add("Text", eventOrganizationController, "EventDaysAverageCost");
            eventSolutionExistsValue_label.DataBindings.Add("Text", eventOrganizationController, "EventSolutionExists");
            eventNValue_label.DataBindings.Add("Text", eventOrganizationController, "EventN");
            eventDaysBeforeValue_label.DataBindings.Add("Text", eventOrganizationController, "EventDaysBefore");

            eventFundamentalPriceValue_label.DataBindings.Add("Text", eventOrganizationController, "EventFundamentalPrice");
            eventFundamentalPriceWithPrivilegesValue_label.DataBindings.Add("Text", eventOrganizationController, "EventFundamentalPriceWithPrivileges");
            eventFundamentalPriceRelativeDifferenceValue_label.DataBindings.Add("Text", eventOrganizationController, "EventFundamentalPriceRelativeDifference");
            eventAveragePriceValue_label.DataBindings.Add("Text", eventOrganizationController, "EventDaysAveragePrice");
            eventAveragePriceWithPrivilegesValue_label.DataBindings.Add("Text", eventOrganizationController, "EventDaysAveragePriceWithPrivileges");
            eventPriceValue_label.DataBindings.Add("Text", eventOrganizationController, "EventPrice");
            eventPriceWithPrivilegesValue_label.DataBindings.Add("Text", eventOrganizationController, "EventPriceWithPrivileges");
            // eventFundamentalPriceIntervalValue_label.DataBindings.Add("Text", eventOrganizationController, "EventFundamentalPriceInterval");

            eventExpensesValue_label.DataBindings.Add("Text", eventOrganizationController, "EventExpenses");
            eventIncomeValue_label.DataBindings.Add("Text", eventOrganizationController, "EventIncome");
            eventRealProfitValue_label.DataBindings.Add("Text", eventOrganizationController, "EventRealProfit");
            eventTheoryProfitValue_label.DataBindings.Add("Text", eventOrganizationController, "EventTheoryProfit");
            eventMaxPercent_label.DataBindings.Add("Text", eventOrganizationController, "EventMaxMarkup");
            eventMinParticipantsCountValue_label.DataBindings.Add("Text", eventOrganizationController, "EventMinParticipants");
        }
        catch
        {
            throw;
        }
    }

    private void InitializeEventInfo()
    {
        try
        {
            eventName_textBox.Text = _eventOrganizationFormController.EventName;
            eventDescription_textBox.Text = _eventOrganizationFormController.EventDescription;

            eventLocationId_label.Text = _eventOrganizationFormController.CurrentEvent.LocationId.ToString();
            eventDate_maskedTextBox.Text = _eventOrganizationFormController.EventDate;
            eventDaysCount_numericUpDown.Value = _eventOrganizationFormController.CurrentEvent.DaysCount;
            eventPercent_numericUpDown.Value = (decimal)_eventOrganizationFormController.CurrentEvent.Percent;
        }
        catch
        {
            throw;
        }
    }

    private void InitializeLocations()
    {
        try
        {
            eventLocation_comboBox.Items.Clear();
            foreach (var loc in _eventOrganizationFormController.AllLocations)
            {
                eventLocation_comboBox.Items.Add(loc);
            }
            eventLocation_comboBox.Items.Add("Добавить");
            eventLocation_comboBox.DisplayMember = "Name";
            eventLocation_comboBox.SelectedItem = _eventOrganizationFormController.CurrentLocation;
        }
        catch
        {
            throw;
        }
    }

    private async void InitializeDaysGrid()
    {
        try
        {
            eventDays_dataGridView.AutoGenerateColumns = false;
            eventDays_dataGridView.Rows.Clear();

            foreach (var day in _eventOrganizationFormController.EventDays)
            {
                eventDays_dataGridView.Rows.Add(
                    day.Id,
                    day.SequenceNumber,
                    day.Name,
                    day.Price,
                    await _eventOrganizationFormController.GetDayParticipantsCountAsync(day.Id)
                );
            }
        }
        catch
        {
            return;
        }
    }

    private void InitializeComponents()
    {
        participation_button.Visible = _eventOrganizationFormController.UserIsOrganizer != true;
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
                    await _eventOrganizationFormController.InitializeAsync();
                    InitializeLocations();
                    InitializeDaysGrid();
                    InitializeLastUpdate();
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
            MessageBox.Show("Ошибка: не удалось установить автоматическое обновление данных в форме организации мероприятия.");
            return;
        }
    }

    public void SetIds(Guid eventId, Guid userId)
    {
        try
        {
            _eventOrganizationFormController.EventId = eventId;
            _eventOrganizationFormController.UserId = userId;
        }
        catch (Exception ex)
        {
            MessageBox.Show("Ошибка: не удалось передать данные в форму информации о мероприятии.");
            return;
        }
    }



    private async void eventSettingSave_button_Click(object sender, EventArgs e)
    {
        try
        {
            var newLocationId = eventLocationId_label.Text.Trim();
            var newName = eventName_textBox.Text.Trim();
            var newDescription = eventDescription_textBox.Text.Trim();
            var newDate = eventDate_maskedTextBox.Text.Trim();
            var newDaysCount = (int)eventDaysCount_numericUpDown.Value;
            var newPercent = (double)eventPercent_numericUpDown.Value;
            var newMaxPrice = (double)eventParticipantBudget_numericUpDown.Value;

            await _eventOrganizationFormController.SaveEvent(
                newLocationId,
                newName,
                newDescription,
                newDate,
                newDaysCount,
                newPercent,
                newMaxPrice
            );

            MessageBox.Show("Информация успешно обновлена.");
        }
        catch (Exception ex)
        {
            MessageBox.Show("Ошибка: не удалось обновить информацию о мероприятии.");
            return;
        }
    }

    private void eventDays_dataGridView_CellClick(object sender, DataGridViewCellEventArgs e)
    {
        try
        {
            if (e.RowIndex < 0) return;
            var row = eventDays_dataGridView.Rows[e.RowIndex];
            var dayId = (Guid)row.Cells[0].Value;
            _eventOrganizationFormController.OpenDayOrganization(dayId);
        }
        catch (Exception ex)
        {
            MessageBox.Show("Ошибка: не удалось открыть форму организации дня мероприятия.");
            return;
        }
    }

    private void eventLocation_comboBox_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (eventLocation_comboBox.SelectedItem != "Добавить")
            {
                var location = (Location)eventLocation_comboBox.SelectedItem;
                eventLocationId_label.Text = location.Id.ToString();

                string tooltipText = $"{location.Name}\n\n" +
                        $"{location.Description}\n\n" +
                        $"Цена аренды на 1 день: {location.Price}\n" +
                        $"Вместимость: {location.Capacity} человек";

                _toolTip.SetToolTip(eventLocation_comboBox, tooltipText);
            }
            else
            {
                _eventOrganizationFormController.OpenLocationCreate();
            }
        }
        catch
        {
            return;
        }
    }

    private void feedback_button_Click(object sender, EventArgs e)
    {
        try
        {
            _eventOrganizationFormController.OpenFeedbackCreate();
        }
        catch (Exception ex)
        {
            MessageBox.Show("Ошибка: не удалось открыть форму создания отзыва.");
            return;
        }
    }

    private void eventFeedbacks_button_Click(object sender, EventArgs e)
    {
        try
        {
            _eventOrganizationFormController.OpenFeedbacks();
        }
        catch (Exception ex)
        {
            MessageBox.Show("Ошибка: не удалось открыть форму информации об отзывах.");
            return;
        }
    }

    private void participation_button_Click(object sender, EventArgs e)
    {
        try
        {
            _eventOrganizationFormController.OpenParticipation();
        }
        catch (Exception ex)
        {
            MessageBox.Show("Ошибка: не удалось открыть форму участия на мероприятии.");
            return;
        }
    }
}
