using Eventor.Common.Enums;
using Eventor.GUI.Controllers;
using Microsoft.Extensions.Logging;
using System.Windows.Forms;

namespace Eventor.GUI;

public partial class DayOrganizationForm : Form
{
    private bool _isLoaded = false;
    private DayOrganizationFormController _dayOrganizationFormController;

    public DayOrganizationForm(DayOrganizationFormController dayOrganizationFormController)
    {
        _dayOrganizationFormController = dayOrganizationFormController;
        InitializeComponent();
        InitializeBindings();
    }

    public void InitializeBindings()
    {
        try
        {
            var dayOrganizationController = new BindingSource { DataSource = _dayOrganizationFormController };

            dayCostValue_label.DataBindings.Add("Text", dayOrganizationController, "DayCost");
            dayPriceValue_label.DataBindings.Add("Text", dayOrganizationController, "DayPrice");
            dayPriceWithPrivilegesValue_label.DataBindings.Add("Text", dayOrganizationController, "DayPriceWithPrivileges");
            dayParticipantsCountValue_label.DataBindings.Add("Text", dayOrganizationController, "DayParticipantsCount");
            dayCoefficientValue_label.DataBindings.Add("Text", dayOrganizationController, "DayCoefficient");
        }
        catch
        {
            throw;
        }
    }

    private async void DayOrganizationForm_Load(object sender, EventArgs e)
    {
        try
        {
            await _dayOrganizationFormController.InitializeAsync();
            await InitializeDayInfo();
            await InitializeParticipantsGrid();
            await InitializeMenuGrid();
            await InitializeTimer();
            InitializeLastUpdate();

            Text = $"Организация дня мероприятия №{_dayOrganizationFormController.CurrentDay.SequenceNumber}";
            _isLoaded = true;
        }
        catch (Exception ex)
        {
            Close();
            MessageBox.Show("Ошибка: не удалось загрузить форму информации о дне мероприятия.");
        }
    }

    private async Task InitializeDayInfo()
    {
        try
        {
            dayName_textBox.Text = _dayOrganizationFormController.DayName;
            dayDescription_textBox.Text = _dayOrganizationFormController.DayDescription;
        }
        catch
        {
            throw;
        }
    }

    private async Task InitializeParticipantsGrid()
    {
        try
        {
            dayParticipants_dataGridView.AutoGenerateColumns = false;
            dayParticipants_dataGridView.Rows.Clear();

            foreach (var person in _dayOrganizationFormController.DayPersons)
            {
                dayParticipants_dataGridView.Rows.Add(
                    person.Id,
                    person.Name,
                    EnumConverter.ToString(person.Type),
                    person.Paid ? "Оплачено" : "Не оплачено"
                );
            }
        }
        catch (Exception ex)
        {
            throw;
        }
    }

    private async Task InitializeMenuGrid()
    {
        try
        {
            dayMenu_dataGridView.AutoGenerateColumns = false;
            dayMenu_dataGridView.Rows.Clear();

            foreach (var item in _dayOrganizationFormController.MenuItems)
            {
                dayMenu_dataGridView.Rows.Add(
                    item.Id,
                    item.Name,
                    await _dayOrganizationFormController.GetItemAmountAsync(item.Id)
                );
            }

            items_comboBox.Items.Clear();
            foreach (var itemName in _dayOrganizationFormController.ItemsIds.Keys)
            {
                items_comboBox.Items.Add(itemName);
            }
            items_comboBox.Items.Add("Добавить");
            items_comboBox.SelectedItem = items_comboBox.Items[0];
        }
        catch (Exception ex)
        {
            throw;
        }
    }

    private void InitializeLastUpdate()
    {
        dataStatus_toolStripStatusLabel.Text = $"Последнее обновление данных: {DateTime.Now}";
    }

    private async Task InitializeTimer()
    {
        try
        {
            _timer.Tick += async (sender, e) =>
            {
                try
                {
                    await _dayOrganizationFormController.InitializeAsync();
                    await InitializeParticipantsGrid();
                    await InitializeMenuGrid();
                    InitializeLastUpdate();
                }
                catch (Exception ex)
                {
                    return;
                }
            };
            _timer.Start();
        }
        catch (Exception ex)
        {
            MessageBox.Show("Ошибка: не удалось установить автоматическое обновление данных в форме организации мероприятия.");
            return;
        }
    }

    public void SetIds(Guid dayId, Guid eventId, Guid userId)
    {
        try
        {
            _dayOrganizationFormController.DayId = dayId;
            _dayOrganizationFormController.EventId = eventId;
            _dayOrganizationFormController.UserId = userId;
        }
        catch (Exception ex)
        {
            throw;
        }
    }

    private async void dayParticipants_dataGridView_CellValueChanged(object sender, DataGridViewCellEventArgs e)
    {
        try
        {
            if (!_isLoaded) return;

            if (dayParticipants_dataGridView.Columns[e.ColumnIndex].HeaderText == "Тип")
            {
                var row = dayParticipants_dataGridView.Rows[e.RowIndex];
                var personId = (Guid)row.Cells[0].Value;
                var personType = (string)row.Cells[2].Value;

                await _dayOrganizationFormController.UpdateParticipantTypeAsync(personId, personType);
                MessageBox.Show("Тип участника успешно изменен.");
            }

            if (dayParticipants_dataGridView.Columns[e.ColumnIndex].HeaderText == "Оплата")
            {
                var row = dayParticipants_dataGridView.Rows[e.RowIndex];
                var personId = (Guid)row.Cells[0].Value;
                var personPaid = (string)row.Cells[3].Value;

                await _dayOrganizationFormController.UpdateParticipantPaidAsync(personId, personPaid);
                MessageBox.Show("Оплата участника успешно изменена.");
            }
        }
        catch
        {
            MessageBox.Show("Ошибка: не удалось изменить тип участника.");
            return;
        }
    }

    private async void saveSettings_button_Click(object sender, EventArgs e)
    {
        try
        {
            var newName = dayName_textBox.Text.Trim();
            var newDescription = dayDescription_textBox.Text.Trim();
            await _dayOrganizationFormController.UpdateDayInformationAsync(newName, newDescription);
            MessageBox.Show("Информация о дне успешно изменена.");
        }
        catch
        {
            MessageBox.Show("Ошибка: не удалось изменить информацию о дне.");
            return;
        }
    }

    private async void addItem_button_Click(object sender, EventArgs e)
    {
        try
        {
            if ((string)items_comboBox.SelectedItem != "Добавить")
            {
                var itemName = (string)items_comboBox.SelectedItem;
                var itemsAmount = (int)itemCount_numericUpDown.Value;
                await _dayOrganizationFormController.AddItemToMenu(itemName, itemsAmount);
                MessageBox.Show("Предмет был успешно добавлен.");
            }
            else
            {
                await _dayOrganizationFormController.OpenItemCreate();
            }
        }
        catch
        {
            MessageBox.Show("Ошибка: не удалось добавить предмет в меню.");
            return;
        }
    }

    private void dayMenu_dataGridView_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
    {
        try
        {
            if (e.Button == MouseButtons.Right && e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                dayMenu_dataGridView.CurrentCell = dayMenu_dataGridView[e.ColumnIndex, e.RowIndex];
                _contextMenuStrip.Show(Cursor.Position);
            }
        }
        catch
        {
            return;
        }
    }

    private async void deleteItem_toolStripMenuItem_Click(object sender, EventArgs e)
    {
        try
        {
            var currentCell = dayMenu_dataGridView.CurrentCell;
            var currentRow = dayMenu_dataGridView.Rows[currentCell.RowIndex];
            var itemId = (Guid)currentRow.Cells[0].Value;
            await _dayOrganizationFormController.DeleteItemFromMenu(itemId);
            MessageBox.Show("Предмет был успешно удален из меню.");
        }
        catch
        {
            MessageBox.Show("Ошибка: не удалось удалить предмет из меню.");
            return;
        }
    }
}
