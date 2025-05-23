using Eventor.Common.Core;
using Eventor.Services;
using Microsoft.Extensions.DependencyInjection;
using PhoneNumbers;
using System.ComponentModel;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Runtime.CompilerServices;
using Day = Eventor.Common.Core.Day;
using EnumConverter = Eventor.Common.Enums.EnumConverter;

namespace Eventor.GUI.Controllers;

public class DayOrganizationFormController : INotifyPropertyChanged
{
    private const double _errorValue = -(double)ErrorType.NOT_A_NUMBER;

    private readonly IDayService _dayService;
    private readonly IMenuService _menuService;
    private readonly IItemService _itemService;
    private readonly IPersonService _personService;
    private readonly IEconomyService _economyService;
    private readonly IServiceProvider _serviceProvider;

    public Guid EventId { get; set; }
    public Guid DayId { get; set; }
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

    private List<Item> _remainingItems = new();
    public List<Item> RemainingItems
    {
        get => _remainingItems;
        set
        {
            _remainingItems = value;
            OnPropertyChanged();
        }
    }

    private List<Item> _allItems = new();
    public List<Item> AllItems
    {
        get => _allItems;
        set
        {
            _allItems = value;
            OnPropertyChanged();
        }
    }

    private bool _eventSolutionExists = false;
    public bool CurrentEventSolutionExists
    {
        get => _eventSolutionExists;
        set
        {
            _eventSolutionExists = value;
            OnPropertyChanged();
        }
    }

    private bool _eventSolutionExistsWithPrivileges = false;
    public bool CurrentEventSolutionExistsWithPrivileges
    {
        get => _eventSolutionExistsWithPrivileges;
        set
        {
            _eventSolutionExistsWithPrivileges = value;
            OnPropertyChanged();
        }
    }

    private double _currentDayCost = 0;
    public double CurrentDayCost
    {
        get => _currentDayCost;
        set
        {
            _currentDayCost = value;
            OnPropertyChanged();
        }
    }

    private double _currentDayPrice = 0;
    public double CurrentDayPrice
    {
        get => _currentDayPrice;
        set
        {
            _currentDayPrice = value;
            OnPropertyChanged();
        }
    }

    private double _currentDayPriceWithPrivileges = 0;
    public double CurrentDayPriceWithPrivileges
    {
        get => _currentDayPriceWithPrivileges;
        set
        {
            _currentDayPriceWithPrivileges = value;
            OnPropertyChanged();
        }
    }

    private double _currentDayParticipantsCount = 0;
    public double CurrentDayParticipantsCount
    {
        get => _currentDayParticipantsCount;
        set
        {
            _currentDayParticipantsCount = value;
            OnPropertyChanged();
        }
    }

    private double _currentDayCoefficient = 0;
    public double CurrentDayCoefficient
    {
        get => _currentDayCoefficient;
        set
        {
            _currentDayCoefficient = value;
            OnPropertyChanged();
        }
    }

    public string DayName => CurrentDay.Name.ToString();
    public string DayDescription => CurrentDay.Description.ToString();
    public string DayCost => Math.Round(CurrentDayCost, 2).ToString();
    public string DayPrice => ValueCheck(CurrentDayPrice);
    public string DayPriceWithPrivileges => ValueCheck(CurrentDayPriceWithPrivileges);
    public string DayParticipantsCount => CurrentDayParticipantsCount.ToString();
    public string DayCoefficient => ValueCheck(CurrentDayCoefficient);

    public event PropertyChangedEventHandler PropertyChanged;

    public DayOrganizationFormController(
        IDayService dayService,
        IMenuService menuService,
        IItemService itemService,
        IPersonService personService,
        IEconomyService economyService,
        IServiceProvider serviceProvider)
    {
        _dayService = dayService;
        _menuService = menuService;
        _itemService = itemService;
        _personService = personService;
        _economyService = economyService;
        _serviceProvider = serviceProvider;
    }

    public async Task InitializeAsync()
    {
        try
        {
            CurrentDay = await _dayService.GetDayByIdAsync(DayId);
            DayPersons = await _personService.GetAllPersonsByDayAsync(DayId);
            await LoadDayInformation();
            await LoadMenuAsync();
        }
        catch (Exception ex)
        {
            throw;
        }
    }

    private async Task LoadDayInformation()
    {
        try
        {
            CurrentEventSolutionExists = await _economyService.CheckSolutionExistenceAsync(EventId);
            CurrentEventSolutionExistsWithPrivileges = await _economyService.CheckSolutionExistenceWithPrivilegesAsync(EventId);
            CurrentDayCost = await _economyService.GetDaysCostAsync(new[] { DayId });
            CurrentDayPrice = CurrentEventSolutionExists ? await _economyService.GetDayPriceAsync(DayId) : _errorValue;
            CurrentDayPriceWithPrivileges = CurrentEventSolutionExistsWithPrivileges ? 
                await _economyService.GetDayPriceWithPrivilegesAsync(DayId) : _errorValue;
            CurrentDayParticipantsCount = await _economyService.GetPersonCountAsync(DayId);
            CurrentDayCoefficient = CurrentEventSolutionExists ?
                await _economyService.GetDayCoefficientAsync(new[] { DayId }) : _errorValue;
        }
        catch
        {
            throw;
        }
    }

    private async Task LoadMenuAsync()
    {
        try
        {
            var menu = await _menuService.GetMenuByDayAsync(DayId);
            MenuItems = await _itemService.GetAllItemsByMenuAsync(menu.Id);
            AllItems = await _itemService.GetAllItemsAsync();

            var menuItemIds = new HashSet<Guid>(MenuItems.Select(item => item.Id));
            RemainingItems = AllItems.Where(item => !menuItemIds.Contains(item.Id)).ToList();
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
            var menu = await _menuService.GetMenuByDayAsync(DayId);
            return await _menuService.GetAmountForItemAsync(menu.Id, itemId);
        }
        catch (Exception ex)
        {
            throw;
        }
    }

    private string ValueCheck(int value)
    {
        try
        {
            if (value == null || value.Equals(_errorValue))
                return "Расчет невозможен";

            return value.ToString();
        }
        catch (Exception ex)
        {
            throw;
        }
    }

    private string ValueCheck(double value)
    {
        try
        {
            if (value == null || value.Equals(_errorValue))
                return "Расчет невозможен";

            return Math.Round(value, 2).ToString();
        }
        catch (Exception ex)
        {
            throw;
        }
    }

    public async Task UpdateParticipantTypeAsync(Guid personId, string newType)
    {
        try
        {
            var person = await _personService.GetPersonByIdAsync(personId);

            var updatedPerson = new Person(
                id: personId,
                name: person.Name,
                type: EnumConverter.ToPersonType(newType),
                paid: person.Paid
            );

            await _personService.UpdatePersonAsync(updatedPerson);
        }
        catch (Exception ex)
        {
            throw;
        }
    }

    public async Task UpdateParticipantPaidAsync(Guid personId, string newPaid)
    {
        try
        {
            var person = await _personService.GetPersonByIdAsync(personId);

            var updatedPerson = new Person(
                id: personId,
                name: person.Name,
                type: person.Type,
                paid: (newPaid == "Оплачено")
            );

            await _personService.UpdatePersonAsync(updatedPerson);
        }
        catch
        {
            throw;
        }
    }

    public async Task UpdateDayInformationAsync(string newName, string newDescription)
    {
        try
        {
            if (string.IsNullOrEmpty(newName))
                throw new ArgumentNullException(nameof(newName));

            if (string.IsNullOrEmpty(newDescription))
                throw new ArgumentNullException(nameof(newDescription));

            var day = await _dayService.GetDayByIdAsync(DayId);

            var updatedDay = new Day(
                id: day.Id,
                menuId: day.MenuId,
                name: newName,
                description: newDescription,
                sequenceNumber: day.SequenceNumber,
                price: day.Price
            );

            await _dayService.UpdateDayAsync(updatedDay);
        }
        catch (Exception ex)
        {
            throw;
        }
    }

    public async Task AddItemToMenu(object? item, int amount)
    {
        try
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item));

            if (item is not Item)
                throw new ArgumentException(nameof(item));

            if (amount <= 0)
                throw new ArgumentOutOfRangeException(nameof(amount));

            
            await _menuService.AddItemAsync(CurrentDay.MenuId, ((Item)item).Id, amount);
        }
        catch
        {
            throw;
        }
    }

    public async Task AddItemToMenu(string itemName, int amount, double price)
    {
        try
        {
            if (!string.IsNullOrEmpty(itemName))
                throw new ArgumentNullException(nameof(itemName));

            if (amount <= 0)
                throw new ArgumentOutOfRangeException(nameof(amount));

            var newItem = new Item(
                id: Guid.NewGuid(),
                name: itemName,
                type: Common.Enums.ItemType.OneDay,
                price: price
            );

            await _itemService.AddItemAsync(newItem);
            await _menuService.AddItemAsync(CurrentDay.MenuId, newItem.Id, amount);
        }
        catch
        {
            throw;
        }
    }

    public async Task DeleteItemFromMenu(Guid itemId)
    {
        try
        {
            await _menuService.DeleteItemAsync(CurrentDay.MenuId, itemId);
        }
        catch
        {
            throw;
        }
    }

    public async Task OpenItemCreate()
    {
        try
        {
            var itemCreateForm = _serviceProvider.GetRequiredService<ItemCreateForm>();
            itemCreateForm.Show();
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
