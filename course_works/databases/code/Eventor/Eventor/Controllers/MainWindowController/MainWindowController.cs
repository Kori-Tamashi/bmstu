using Eventor.Common.Core;
using Eventor.Services;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using EnumConverter = Eventor.Common.Enums.EnumConverter;

namespace Eventor.GUI.Controllers;

public class MainWindowController : INotifyPropertyChanged
{
    private readonly IServiceProvider _serviceProvider;
    private readonly IEventService _eventService;
    private readonly ILocationService _locationService;
    private readonly IUserService _userService;
    private string _errorMessage;
    private User _currentUser;
    
    public User CurrentUser
    {
        get => _currentUser;
        set
        {
            _currentUser = value;
            OnPropertyChanged();
        }
    }

    public string CurrentGender => EnumConverter.ToString(CurrentUser.Gender);
    public string CurrentRole => EnumConverter.ToString(CurrentUser.Role);
    public string ErrorMessage => _errorMessage;

    public ObservableCollection<Event> UserEvents { get; } = new();
    public ObservableCollection<Event> AllEvents { get; } = new();

    public ObservableCollection<Event> AllOrganizedEvents { get; } = new();

    public event PropertyChangedEventHandler PropertyChanged;

    public MainWindowController(
        IServiceProvider serviceProvider,
        IEventService eventService,
        IUserService userService,
        ILocationService locationService)
    {
        _serviceProvider = serviceProvider;
        _eventService = eventService;
        _userService = userService;
        _locationService = locationService;
    }

    public async Task InitializeAsync()
    {
        try
        {
            await LoadUserData();
            await LoadUserEvents();
            await LoadAllEvents();
            await LoadAllOrganizedEvents();
        }
        catch
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
        catch
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
        catch
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
        catch
        {
            throw;
        }
    }

    private async Task LoadAllOrganizedEvents()
    {
        try
        {
            var organizedEvents = await _eventService.GetAllOrganizedEventsByUserAsync(CurrentUser.Id);
            AllOrganizedEvents.Clear();
            foreach (var e in organizedEvents) AllOrganizedEvents.Add(e);
        }
        catch
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
        catch
        {
            throw;
        }
    }

    public async Task<Location> GetEventLocation(Guid eventId)
    {
        try
        {
            var _event = await _eventService.GetEventByIdAsync(eventId);
            return await _locationService.GetLocationByIdAsync(_event.LocationId);
        }
        catch
        {
            throw;
        }
    }

    public async Task DeleteEventAsync(Guid eventId)
    {
        await _eventService.DeleteEventAsync(eventId);
    }

    public bool IsAdmin()
    {
        return CurrentUser.Role == Common.Enums.UserRole.Admin;
    }

    public async Task OpenEventLogic(Guid eventId)
    {
        try
        {
            if (CurrentUser.Role == Common.Enums.UserRole.Admin)
                OpenEventOrganization(eventId);
            else
                OpenEventDetails(eventId);
        }
        catch
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
        catch
        {
            throw;
        }
    }

    public async Task OpenEventOrganization(Guid eventId)
    {
        try
        {
            var eventForm = _serviceProvider.GetRequiredService<EventOrganizationForm>();
            eventForm.SetIds(eventId, CurrentUser.Id);
            eventForm.Show();
        }
        catch
        {
            throw;
        }
    }

    public async Task OpenEventCreate()
    {
        try
        {
            var eventCreateForm = _serviceProvider.GetRequiredService<EventCreateForm>();
            eventCreateForm.SetIds(CurrentUser.Id);
            eventCreateForm.Show();
        }
        catch
        {
            throw;
        }
    }

    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}