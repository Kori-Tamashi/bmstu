using Eventor.Services;
using System.ComponentModel;
using Eventor.Common.Core;
using System.Runtime.CompilerServices;

namespace Eventor.GUI.Controllers;

public class LocationCreateFormController : INotifyPropertyChanged
{
    private readonly ILocationService _locationService;

    public event PropertyChangedEventHandler PropertyChanged;

    public LocationCreateFormController(ILocationService locationService)
    {
        _locationService = locationService;
    }

    public async Task CreateLocationAsync(
        string locationName, 
        string locationDescription, 
        double locationPrice,
        int locationCapacity
    )
    {
        try
        {
            if (string.IsNullOrEmpty(locationName))
                throw new ArgumentNullException(nameof(locationName));

            if (string.IsNullOrEmpty(locationDescription))
                throw new ArgumentNullException(nameof(locationDescription));

            if (locationPrice < 0)
                throw new ArgumentOutOfRangeException(nameof(locationPrice));

            if (locationCapacity <= 0)
                throw new ArgumentOutOfRangeException(nameof(locationCapacity));

            var newLocation = new Location(
                id: Guid.NewGuid(),
                name: locationName,
                description: locationDescription,
                price: locationPrice,
                capacity: locationCapacity
            );

            await _locationService.AddLocationAsync(newLocation);
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
