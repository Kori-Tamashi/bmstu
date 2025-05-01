using Eventor.Common.Core;
using Eventor.Database.Core;
using Eventor.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using EnumConverter = Eventor.Common.Enums.EnumConverter;

namespace Eventor.GUI.Controllers;

public class MainWindowController : INotifyPropertyChanged
{
    private readonly IServiceProvider _serviceProvider;
    private readonly IEventService _eventService;
    private readonly IUserService _userService;
    private string _errorMessage;
    private User _currentUser;
    
    public User CurrentUser
    {
        get => _currentUser;
        set
        {
            _currentUser = value;
            OnPropertyChanged(nameof(CurrentUser));
            OnPropertyChanged(nameof(CurrentGender));
            OnPropertyChanged(nameof(CurrentRole));
        }
    }
    public string CurrentGender => EnumConverter.ToString(CurrentUser.Gender);
    public string CurrentRole => EnumConverter.ToString(CurrentUser.Role);
    public string ErrorMessage => _errorMessage;

    public ObservableCollection<Event> UserEvents { get; } = new();
    public ObservableCollection<Event> AllEvents { get; } = new();

    public event PropertyChangedEventHandler PropertyChanged;

    public MainWindowController(
        IServiceProvider serviceProvider,
        IEventService eventService,
        IUserService userService)
    {
        _serviceProvider = serviceProvider;
        _eventService = eventService;
        _userService = userService;
    }

    public async Task InitializeAsync()
    {
        try
        {
            await LoadUserData();
            await LoadUserEvents();
            await LoadAllEvents();
        }
        catch (Exception ex)
        {
            throw;
        }
    }

    private async Task LoadUserData()
    {
        try
        {
            CurrentUser = await _userService.GetUserByIdAsync(CurrentUser.Id);
            OnPropertyChanged(nameof(CurrentUser));
            OnPropertyChanged(nameof(CurrentGender));
            OnPropertyChanged(nameof(CurrentRole));
        }
        catch (Exception ex)
        {
            throw;
        }
    }

    private async Task LoadUserEvents()
    {
        try
        {
            var events = await _eventService.GetAllEventsByUserAsync(CurrentUser.Id);
            UserEvents.Clear();
            foreach (var e in events) UserEvents.Add(e);
        }
        catch (Exception ex)
        {
            throw;
        }
    }

    private async Task LoadAllEvents()
    {
        try
        {
            var events = await _eventService.GetAllEventsAsync();
            AllEvents.Clear();
            foreach (var e in events) AllEvents.Add(e);
        }
        catch (Exception ex)
        {
            throw;
        }
    }

    public async Task SaveUser(string newName, string newGender, string newPhone, string newRole)
    {
        try
        {
            var updatedUser = new User(
                id: CurrentUser.Id,
                name: newName,
                phone: newPhone,
                gender: EnumConverter.ToGender(newGender),
                passwordHash: CurrentUser.PasswordHash,
                role: EnumConverter.ToUserRole(newRole)
            );

            await _userService.UpdateUserAsync(updatedUser);
            await LoadUserData();
        }
        catch (Exception ex)
        {
            throw;
        }
    }

    public async Task OpenEventDetails(Guid eventId)
    {
        try
        {
            var eventForm = _serviceProvider.GetRequiredService<EventForm>();
            eventForm.SetIds(eventId, CurrentUser.Id);
            eventForm.Show();
        }
        catch (Exception ex)
        {
            throw;
        }
    }

    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}