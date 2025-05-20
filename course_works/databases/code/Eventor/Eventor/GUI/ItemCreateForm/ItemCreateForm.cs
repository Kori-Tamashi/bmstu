using Eventor.GUI.Controllers;

namespace Eventor.GUI;

public partial class ItemCreateForm : Form
{
    ItemCreateFormController _itemCreateFormController;

    public ItemCreateForm(ItemCreateFormController itemCreateFormController)
    {
        _itemCreateFormController = itemCreateFormController;
        InitializeComponent();
    }

    private async void itemCreate_button_Click(object sender, EventArgs e)
    {
        try
        {
            var itemName = itemName_textBox.Text.Trim();
            var itemPrice = (double)itemPrice_numericUpDown.Value;
            await _itemCreateFormController.ItemCreate(itemName, itemPrice);
        }
        catch
        {
            MessageBox.Show("Ошибка: не удалось создать предмет.");
            return;
        }
    }
}
