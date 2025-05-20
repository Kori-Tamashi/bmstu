using Eventor.GUI.Controllers;

namespace Eventor.GUI;

public partial class LocationCreateForm : Form
{
    LocationCreateFormController _locationCreateFormController;

    public LocationCreateForm(LocationCreateFormController locationCreateFormController)
    {
        _locationCreateFormController = locationCreateFormController;
        InitializeComponent();
    }

    private async void locationSave_button_Click(object sender, EventArgs e)
    {
        try
        {
            var locationName = locationName_textBox.Text.Trim();
            var locationDescription = locationDescription_textBox.Text.Trim();
            var locationPrice = (double)locationPrice_numericUpDown.Value;
            var locationCapacity = (int)locationCapacity_numericUpDown.Value;

            await _locationCreateFormController.CreateLocationAsync(
                locationName,
                locationDescription,
                locationPrice,
                locationCapacity
            );

            MessageBox.Show("Локация успешно добавлена.");
        }
        catch
        {
            MessageBox.Show("Ошибка: не удалось создать локацию.");
            return;
        }
    }
}
