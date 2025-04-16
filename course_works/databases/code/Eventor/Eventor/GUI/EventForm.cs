using Eventor.Database.Core;
using Eventor.Services;
using Day = Eventor.Common.Core.Day;

namespace Eventor.Eventor.GUI
{
    public partial class EventForm : Form
    {
        IUserService _userService;
        IEventService _eventService;
        IDayService _dayService;
        IEconomyService _economyService;

        public Guid EventId { get; set; }

        public EventForm(
            IUserService userService, 
            IEventService eventService,
            IDayService dayService,
            IEconomyService economyService
            )
        {
            _dayService = dayService;
            _userService = userService;
            _eventService = eventService;
            _economyService = economyService;
            
            InitializeComponent();
        }

        private async void EventForm_Load(object sender, EventArgs e)
        {
            try
            {
                var eventData = await _eventService.GetEventByIdAsync(EventId);
                eventName_label.Text = $"{eventData.Name} ({eventData.Date.ToString()})";
                eventDescription_textBox.Text = eventData.Description;
                eventPersonCount_label.Text = $"Количество человек: {eventData.PersonCount.ToString()}";
                eventDaysCount_label.Text = $"Количество дней: {eventData.DaysCount.ToString()}";

                var eventDays = await _dayService.GetAllDaysByEventAsync(EventId);
                foreach (Day day in eventDays)
                {
                    var dayPersonCount = await _economyService.GetPersonCountAsync(day.Id);
                    eventDays_dataGridView.Rows.Add(day.Id, day.SequenceNumber, day.Name, day.Price, dayPersonCount);
                }

                MessageBox.Show($"Дни загружены");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки мероприятия: {ex.Message}");
                Close();
            }
        }

    }
}
