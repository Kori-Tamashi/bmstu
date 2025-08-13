using Eventor.Common.Core;
using Eventor.Services;
namespace Eventor.GUI.Controllers;

public class ItemCreateFormController
{
    private readonly IItemService _itemService;

    public ItemCreateFormController(IItemService itemService)
    {
        _itemService = itemService;
    }

    public async Task ItemCreate(string itemName, double itemPrice)
    {
        try
        {
            if (string.IsNullOrEmpty(itemName))
                throw new ArgumentNullException(nameof(itemName));

            if (itemPrice <= 0)
                throw new ArgumentOutOfRangeException(nameof(itemPrice));

            var newItem = new Item(
                id: Guid.NewGuid(),
                name: itemName,
                type: Common.Enums.ItemType.OneDay,
                price: itemPrice
            );

            await _itemService.AddItemAsync(newItem);
        }
        catch
        {
            throw;
        }
    }
}
