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
            eventFundamentalPriceIntervalValue_label.DataBindings.Add("Text", eventOrganizationController, "EventFundamentalPriceInterval");

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

            foreach (var loc in _eventOrganizationFormController.AllLocations)
            {
                eventLocation_comboBox.Items.Add(loc.Name);
            }
            eventLocation_comboBox.Items.Add("Добавить");
            eventLocation_comboBox.SelectedItem = _eventOrganizationFormController.EventLocationName;
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
                    await _eventOrganizationFormController.InitializeAsync();
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
}
