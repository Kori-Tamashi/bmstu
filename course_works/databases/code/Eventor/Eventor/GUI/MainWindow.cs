using Eventor.Common.Core;
using Eventor.Common.Enums;
using Eventor.Database.Core;
using Eventor.Eventor.GUI;
using Eventor.Services;
using Eventor.Services.Exceptions;
using Microsoft.Extensions.DependencyInjection;
using System.ComponentModel.DataAnnotations;

namespace Eventor;

public partial class MainWindow : Form
{
    IServiceProvider _serviceProvider;
    IUserService _userService;
    IOauthService _oauthService;
    IEventService _eventService;
    IDayService _dayService;
    IPersonService _personService;
    IMenuService _menuService;
    IItemService _itemService;
    IEconomyService _economyService;
    IFeedbackService _feedbackService;

    public User? CurrentUser { get; set; }

    public MainWindow(
        IServiceProvider serviceProvider,
        IUserService userService,
        IOauthService oauthService,
        IEventService eventService,
        IDayService dayService,
        IPersonService personService,
        IMenuService menuService,
        IItemService itemService,
        IEconomyService economyService,
        IFeedbackService feedbackService
        )
    {
        _serviceProvider = serviceProvider;
        _userService = userService;
        _oauthService = oauthService;
        _eventService = eventService;
        _dayService = dayService;
        _personService = personService;
        _menuService = menuService;
        _itemService = itemService;
        _economyService = economyService;
        _feedbackService = feedbackService;

        InitializeComponent();
    }

    private void MainWindow_Load(object sender, EventArgs e)
    {
        InitializeData();
        InitializeTimer();
    }

    private async void InitializeData()
    {
        await RefreshUserDataAsync();
        await RefreshUserEventsAsync();
        await RefreshEventsAsync();
    }

    private void InitializeTimer()
    {
        _timer.Tick += async (sender, e) =>
        {
            InitializeData();
        };
        _timer.Start();
    }

    private async void userInfoSave_button_Click(object sender, EventArgs e)
    {
        var phone = userPhone_maskedTextBox.Text.Trim();
        var newName = userName_textBox.Text.Trim();
        var newGender = userGender_comboBox.Text.Trim();

        try
        {
            var updatedUser = CurrentUser;
            updatedUser.Name = newName;
            updatedUser.Phone = phone;
            updatedUser.Gender = newGender.ToGender();

            await _userService.UpdateUserAsync(updatedUser);
            CurrentUser = updatedUser;
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message);
            return;
        }

        MessageBox.Show("ƒанные пользовател€ успешно обновлены.");
    }

    private async Task RefreshUserDataAsync()
    {
        try
        {
            CurrentUser = await _userService.GetUserByIdAsync(CurrentUser.Id);
            userName_textBox.Text = CurrentUser.Name;
            userPhone_maskedTextBox.Text = CurrentUser.Phone;
            userGender_comboBox.Text = EnumConverter.ToString(CurrentUser.Gender);
            userRole_textBox.Text = EnumConverter.ToString(CurrentUser.Role);
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message);
            return;
        }
    }

    private async Task RefreshUserEventsAsync()
    {
        try
        {
            List<Event> userEvents = await _eventService.GetAllEventsByUserAsync(CurrentUser.Id);
            userEvents_dataGridView.AutoGenerateColumns = false;
            userEvents_dataGridView.DataSource = userEvents;
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message);
            return;
        }
    }

    private async Task RefreshEventsAsync()
    {
        try
        {
            List<Event> events = await _eventService.GetAllEventsAsync();
            events_dataGridView.AutoGenerateColumns = false;
            events_dataGridView.DataSource = events;
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message);
            return;
        }
    }

    private void events_dataGridView_CellClick(object sender, DataGridViewCellEventArgs e)
    {
        if(e.RowIndex < 0) return;

        var row = events_dataGridView.Rows[e.RowIndex];
        var eventItem = row.DataBoundItem as Event;
        if (eventItem == null) return;

        var eventForm = _serviceProvider.GetRequiredService<EventForm>();
        eventForm.EventId = eventItem.Id;
        eventForm.Show();
    }
}
