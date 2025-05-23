using Eventor.Common.Core;
using Eventor.Services;
using Microsoft.Extensions.DependencyInjection;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Security.Policy;

namespace Eventor.GUI.Controllers;

public class EventCreateFormController : INotifyPropertyChanged
{
    private readonly IEventService _eventService;
    private readonly IDayService _dayService;
    private readonly IUserService _userService;
    private readonly IPersonService _personService;
    private readonly ILocationService _locationService;
    private readonly IServiceProvider _serviceProvider;

    public Guid UserId { get; set; }

    private List<Location> _allLocations = new();
    public List<Location> AllLocations
    {
        get => _allLocations;
        set
        {
            _allLocations = value;
            OnPropertyChanged();
        }
    }

    public event PropertyChangedEventHandler PropertyChanged;

    public EventCreateFormController(
        IEventService eventService, 
        ILocationService locationService,
        IDayService dayService,
        IPersonService personService,
        IUserService userService,
        IServiceProvider serviceProvider
    )
    {
        _eventService = eventService;
        _locationService = locationService;
        _dayService = dayService;
        _personService = personService;
        _userService = userService;
        _serviceProvider = serviceProvider;
    }

    public async Task InitializeAsync()
    {
        AllLocations = await _locationService.GetAllLocationsAsync();
    }

    public async Task CreateEventAsync(Guid locationId, string name, string description, string date,
        int daysCount, double percent)
    {
        if (string.IsNullOrEmpty(name))
            throw new ArgumentNullException("name");

        if (string.IsNullOrEmpty(description))
            throw new ArgumentNullException("description");

        if (string.IsNullOrEmpty(date))
            throw new ArgumentNullException("date");

        if (percent < 0)
            throw new ArgumentOutOfRangeException("percent");

        if (!DateOnly.TryParse(date, out var updatedDate))
            throw new InvalidDataException("date");

        var newEvent = new Event(
            id: Guid.NewGuid(),
            locationId: locationId,
            name: name,
            description: description,
            date: updatedDate,
            personCount: 0,
            daysCount: daysCount,
            percent: percent,
            rating: 10
        );

        await _eventService.AddEventAsync(newEvent);

        // Добавляем пользователя на мероприятия
        var user = await _userService.GetUserByIdAsync(UserId);
        var person = await _personService.AddPersonForUserAsync(user.Name, UserId);
        await _eventService.AddUserForEventAsync(UserId, newEvent.Id);

        // Устанавливаем права организатора
        person.Type = Common.Enums.PersonType.Organizer;
        await _personService.UpdatePersonAsync(person);

        // Добавляем на каждый из дней
        var eventDays = await _dayService.GetAllDaysByEventAsync(newEvent.Id);
        foreach (var day in eventDays)
        {
            await _dayService.AddPersonToDayAsync(person.Id, day.Id);
        }
    }

    public void OpenLocationCreate()
    {
        var locationCreateForm = _serviceProvider.GetRequiredService<LocationCreateForm>();
        locationCreateForm.Show();
    }

    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
