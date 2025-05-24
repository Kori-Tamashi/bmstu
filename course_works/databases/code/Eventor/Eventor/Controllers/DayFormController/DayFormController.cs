using Eventor.Common.Core;
using Eventor.Database.Core;
using Eventor.Services;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Day = Eventor.Common.Core.Day;

namespace Eventor.GUI.Controllers;

public class DayFormController : INotifyPropertyChanged
{
    private readonly IDayService _dayService;
    private readonly IMenuService _menuService;
    private readonly IItemService _itemService;
    private readonly IPersonService _personService;

    public Guid DayId { get; set; }
    public Guid EventId { get; set; }
    public Guid UserId { get; set; }

    

    private Day _currentDay;
    public Day CurrentDay
    {
        get => _currentDay;
        set
        {
            _currentDay = value;
            OnPropertyChanged();
        }
    }

    private List<Person> _dayPersons = new();
    public List<Person> DayPersons
    {
        get => _dayPersons;
        set
        {
            _dayPersons = value;
            OnPropertyChanged();
        }
    }

    private Menu _dayMenu;
    public Menu DayMenu
    {
        get => _dayMenu;
        set
        {
            _dayMenu = value;
            OnPropertyChanged();
        }
    }

    private List<Item> _menuItems = new();
    public List<Item> MenuItems
    {
        get => _menuItems;
        set
        {
            _menuItems = value;
            OnPropertyChanged();
        }
    }

    public event PropertyChangedEventHandler PropertyChanged;

    public DayFormController(
        IDayService dayService,
        IMenuService menuService,
        IItemService itemService,
        IPersonService personService)
    {
        _dayService = dayService;
        _menuService = menuService;
        _itemService = itemService;
        _personService = personService;
    }

    public void SetIds(Guid dayId, Guid eventId, Guid userId)
    {
        try
        {
            DayId = dayId;
            EventId = eventId;
            UserId = userId;
        }
        catch (Exception ex)
        {
            throw;
        }
    }

    public async Task InitializeDayAsync()
    {
        try
        {
            CurrentDay = await _dayService.GetDayByIdAsync(DayId);
            DayPersons = await _personService.GetAllPersonsByDayAsync(DayId);
            DayMenu = await _menuService.GetMenuByDayAsync(DayId);
            MenuItems = await _itemService.GetAllItemsByMenuAsync(DayMenu.Id);
        }
        catch (Exception ex)
        {
            throw;
        }
    }

    public async Task<int> GetItemAmountAsync(Guid itemId)
    {
        try
        {
            return await _menuService.GetAmountForItemAsync(DayMenu.Id, itemId);
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