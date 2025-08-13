using Eventor.Common.Core;
using Eventor.Database.Core;
using Eventor.Services;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Eventor.GUI.Controllers;

public class ParticipationFormController : INotifyPropertyChanged
{
    private readonly IEventService _eventService;
    private readonly IDayService _dayService;
    private readonly IPersonService _personService;
    private readonly IUserService _userService;
    private List<int> _selectedDays = new();

    public Guid EventId { get; set; }
    public Guid UserId { get; set; }
    public int DaysCount { get; set; }

    public List<int> SelectedDays => _selectedDays;

    public List<string> DayLabels { get; private set; } = new();

    public event PropertyChangedEventHandler PropertyChanged;

    public ParticipationFormController(IEventService eventService, IDayService dayService, IPersonService personService, IUserService userService)
    {
        _eventService = eventService;
        _dayService = dayService;
        _personService = personService;
        _userService = userService;
    }

    public async Task InitializeAsync()
    {
        try
        {
            var days = await _dayService.GetAllDaysByEventAsync(EventId);
            DaysCount = days.Count;
            DayLabels = days.Select(d => $"День {d.SequenceNumber}").ToList();

            var userPerson = await _personService.GetPersonByUserAndEventAsync(UserId, EventId);
            if (userPerson != null)
            {
                var userDays = await _dayService.GetAllDaysByPersonAsync(userPerson.Id);
                _selectedDays = userDays.Select(d => d.SequenceNumber).ToList();
            }

            OnPropertyChanged(nameof(DayLabels));
        }
        catch
        {
            throw;
        }
    }

    public void UpdateSelectedDays(List<int> selectedIndices)
    {
        try
        {
            _selectedDays = selectedIndices.Select(i => i + 1).ToList();
        }
        catch (Exception e) 
        {
            throw;
        }
    }

    public async Task SaveParticipationAsync()
    {
        try
        {
            // 1. Получаем участника пользователя
            var userPerson = await _personService.GetPersonByUserAndEventAsync(UserId, EventId);

            // 2. Если дней не выбрано, удаляем участника
            if (_selectedDays.Count == 0)
            {
                if (userPerson != null)
                {
                    await _personService.DeletePersonForUserAsync(EventId, UserId);
                    await _eventService.DeleteUserFromEventAsync(UserId, EventId);
                }

                return;
            }

            // 3. Если дни выбраны, обновляем данные
            if (userPerson == null)
            {
                var user = await _userService.GetUserByIdAsync(UserId);
                userPerson = await _personService.AddPersonForUserAsync(user.Name, UserId);
                await _eventService.AddUserForEventAsync(UserId, EventId);
            }

            var userEvents = await _eventService.GetAllEventsByUserAsync(UserId);
            var userEventsIds = userEvents.Select(e => e.Id).ToList();
            if (userEventsIds != null)
            {
                if (!userEventsIds.Contains(EventId))
                    await _eventService.AddUserForEventAsync(UserId, EventId);
            }

            var currentDays = await _dayService.GetAllDaysByPersonAsync(userPerson.Id);
            var currentSequenceNumbers = currentDays.Select(d => d.SequenceNumber).ToList();

            foreach (var day in currentDays)
            {
                if (!_selectedDays.Contains(day.SequenceNumber))
                {
                    await _dayService.DeletePersonFromDayAsync(userPerson.Id, day.Id);
                }
            }

            var eventDays = await _dayService.GetAllDaysByEventAsync(EventId);
            foreach (var sequenceNumber in _selectedDays)
            {
                if (!currentSequenceNumbers.Contains(sequenceNumber))
                {
                    var day = eventDays.FirstOrDefault(d => d.SequenceNumber == sequenceNumber);
                    await _dayService.AddPersonToDayAsync(userPerson.Id, day.Id);
                }
            }
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