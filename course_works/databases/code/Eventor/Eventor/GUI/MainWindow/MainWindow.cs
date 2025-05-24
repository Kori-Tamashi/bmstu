using Eventor.GUI.Controllers;

namespace Eventor.GUI;

public partial class MainWindow : Form
{
    private MainWindowController _mainWindowController;

    public MainWindow() { }

    public MainWindow(MainWindowController mainWindowController)
    {
        _mainWindowController = mainWindowController;
        InitializeComponent();
        InitializeBindings();
    }

    #region Initialize

    private async void MainWindow_Load(object sender, EventArgs e)
    {
        try
        {
            await _mainWindowController.InitializeAsync();
            InitializeUserEventsGrid();
            InitializeAllEventsGrid();
            InitializeAllOrganizedEvents();
            InitializeLastUpdate();
            InitializeTimer();
        }
        catch (Exception ex)
        {
            MessageBox.Show("Ошибка: не удалось произвести загрузку главной формы.",
                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
        }
    }

    private void InitializeBindings()
    {
        try
        {
            var userBindingSource = new BindingSource { DataSource = _mainWindowController };

            userName_textBox.DataBindings.Add("Text", userBindingSource, "CurrentUser.Name");
            userPhone_maskedTextBox.DataBindings.Add("Text", userBindingSource, "CurrentUser.Phone");
            userGender_comboBox.DataBindings.Add("Text", userBindingSource, "CurrentGender");
            userRole_textBox.DataBindings.Add("Text", userBindingSource, "CurrentRole");
        }
        catch (Exception ex)
        {
            MessageBox.Show("Ошибка: не удалось произвести связывание данных между главной формой и контроллером.",
                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
        }
    }

    private void InitializeUserEventsGrid()
    {
        try
        {
            userEvents_dataGridView.AutoGenerateColumns = false;
            userEvents_dataGridView.Rows.Clear();

            foreach (var evt in _mainWindowController.UserEvents)
            {
                userEvents_dataGridView.Rows.Add(
                    evt.Name,
                    evt.Date.ToString(),
                    evt.Id
                );
            }
        }
        catch (Exception ex)
        {
            throw;
        }
    }

    private void InitializeAllEventsGrid()
    {
        try
        {
            events_dataGridView.AutoGenerateColumns = false;
            events_dataGridView.Rows.Clear();

            foreach (var evt in _mainWindowController.AllEvents)
            {
                var row = new DataGridViewRow();
                row.CreateCells(events_dataGridView);

                row.Cells[0].Value = evt.Id;
                row.Cells[1].Value = evt.Name;
                row.Cells[2].Value = evt.Description;
                row.Cells[3].Value = evt.Date.ToString();
                row.Cells[4].Value = evt.PersonCount;
                row.Cells[5].Value = evt.DaysCount;
                row.Cells[6].Value = evt.Rating;

                row.Tag = evt;
                events_dataGridView.Rows.Add(row);
            }
        }
        catch (Exception ex)
        {
            throw;
        }
    }

    private async void InitializeAllOrganizedEvents()
    {
        try
        {
            organizedEvents_dataGridView.AutoGenerateColumns = false;
            organizedEvents_dataGridView.Rows.Clear();

            foreach (var evt in _mainWindowController.AllOrganizedEvents)
            {
                var row = new DataGridViewRow();
                row.CreateCells(organizedEvents_dataGridView);

                var location = await _mainWindowController.GetEventLocation(evt.Id);

                row.Cells[0].Value = evt.Id;
                row.Cells[1].Value = evt.Name;
                row.Cells[2].Value = location.Name;
                row.Cells[3].Value = evt.Description;
                row.Cells[4].Value = evt.Date.ToString();
                row.Cells[5].Value = evt.PersonCount;
                row.Cells[6].Value = evt.DaysCount;
                row.Cells[7].Value = evt.Percent;
                row.Cells[8].Value = evt.Rating;

                row.Tag = evt;
                organizedEvents_dataGridView.Rows.Add(row);
            }
        }
        catch (Exception ex)
        {
            return;
        }
    }

    private void InitializeLastUpdate()
    {
        dataStatus_toolStripStatusLabel.Text = $"Последнее обновление данных: {DateTime.Now}";
    }

    private void InitializeTimer()
    {
        try
        {
            _timer.Tick += async (sender, e) =>
            {
                try
                {
                    await _mainWindowController.InitializeAsync();
                    InitializeUserEventsGrid();
                    InitializeAllEventsGrid();
                    InitializeAllOrganizedEvents();
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
            MessageBox.Show("Ошибка: не удалось установить автоматическое обновление данных в главной форме.",
                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
        }
    }

    #endregion

    public void SetController(MainWindowController controller)
    {
        try
        {
            _mainWindowController = controller;
            InitializeComponent();
            InitializeBindings();
        }
        catch (Exception ex)
        {
            MessageBox.Show("Ошибка: не удалось установить контроллер главной формы.",
                "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
        }
    }

    private bool IsAdmin()
    {
        try
        {
            return _mainWindowController.IsAdmin();
        }
        catch
        {
            return false;
        }
    }

    private async void userInfoSave_button_Click(object sender, EventArgs e)
    {
        try
        {
            var newName = userName_textBox.Text.Trim();
            var newGender = userGender_comboBox.Text.Trim();
            var newPhone = userPhone_maskedTextBox.Text.Trim();
            var newRole = userRole_textBox.Text.Trim();

            await _mainWindowController.SaveUser(newName, newGender, newPhone, newRole);
            MessageBox.Show("Информация успешно сохранена.");
        }
        catch (Exception ex)
        {
            MessageBox.Show("Ошибка: не удалось сохранить информацию о пользователе.",
                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
        }
    }

    private async void events_dataGridView_CellClick(object sender, DataGridViewCellEventArgs e)
    {
        try
        {
            if (e.RowIndex < 0) return;
            var row = events_dataGridView.Rows[e.RowIndex];
            var eventId = (Guid)row.Cells[0].Value;

            await _mainWindowController.OpenEventLogic(eventId);
        }
        catch (Exception ex)
        {
            MessageBox.Show("Ошибка: не удалось открыть форму информации о мероприятии.",
                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
        }
    }

    private async void userEvents_dataGridView_CellClick(object sender, DataGridViewCellEventArgs e)
    {
        try
        {
            if (e.RowIndex < 0) return;
            var row = userEvents_dataGridView.Rows[e.RowIndex];
            var eventId = (Guid)row.Cells[2].Value;

            await _mainWindowController.OpenEventLogic(eventId);
        }
        catch (Exception ex)
        {
            MessageBox.Show("Ошибка: не удалось открыть форму информации о мероприятии.",
                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
        }
    }

    private async void organisedEvents_dataGridView_CellClick(object sender, DataGridViewCellEventArgs e)
    {
        try
        {
            if (e.RowIndex < 0) return;
            var row = organizedEvents_dataGridView.Rows[e.RowIndex];
            var eventId = (Guid)row.Cells[0].Value;

            await _mainWindowController.OpenEventOrganization(eventId);
        }
        catch (Exception ex)
        {
            MessageBox.Show("Ошибка: не удалось открыть форму информации о мероприятии.",
                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error); ;
            return;
        }
    }

    private async void eventCreate_button_Click(object sender, EventArgs e)
    {
        try
        {
            await _mainWindowController.OpenEventCreate();
        }
        catch
        {
            MessageBox.Show("Ошибка: не удалось открыть форму создания мероприятия.",
                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
        }
    }

    private void events_dataGridView_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
    {
        try
        {
            if (e.Button == MouseButtons.Right && e.RowIndex >= 0 && e.ColumnIndex >= 0 && IsAdmin())
            {
                events_dataGridView.CurrentCell = events_dataGridView[e.ColumnIndex, e.RowIndex];
                allEvents_contextMenuStrip.Show(Cursor.Position);
            }
        }
        catch
        {
            return;
        }
    }

    private async void eventDelete_ToolStripMenuItem_Click(object sender, EventArgs e)
    {
        try
        {
            var currentCell = events_dataGridView.CurrentCell;
            var currentRow = events_dataGridView.Rows[currentCell.RowIndex];
            var eventId = (Guid)currentRow.Cells[0].Value;
            await _mainWindowController.DeleteEventAsync(eventId);
            MessageBox.Show("Мероприятие успешно удалено.");

            await _mainWindowController.InitializeAsync();
            InitializeAllEventsGrid();
            InitializeUserEventsGrid();
            InitializeAllOrganizedEvents();
            InitializeLastUpdate();
        }
        catch
        {
            MessageBox.Show("Ошибка: не удалось удалить мероприятие.",
                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
        }
    }

    private void organizedEvents_dataGridView_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
    {
        if (e.Button == MouseButtons.Right && e.RowIndex >= 0 && e.ColumnIndex >= 0)
        {
            organizedEvents_dataGridView.CurrentCell = organizedEvents_dataGridView[e.ColumnIndex, e.RowIndex];
            organisedEvents_contextMenuStrip.Show(Cursor.Position);
        }
    }

    private async void deleteOrganisedEvent_ToolStripMenuItem_Click(object sender, EventArgs e)
    {
        try
        {
            var currentCell = organizedEvents_dataGridView.CurrentCell;
            var currentRow = organizedEvents_dataGridView.Rows[currentCell.RowIndex];
            var eventId = (Guid)currentRow.Cells[0].Value;
            await _mainWindowController.DeleteEventAsync(eventId);
            MessageBox.Show("Мероприятие успешно удалено.");

            await _mainWindowController.InitializeAsync();
            InitializeAllEventsGrid();
            InitializeUserEventsGrid();
            InitializeAllOrganizedEvents();
            InitializeLastUpdate();
        }
        catch
        {
            MessageBox.Show("Ошибка: не удалось удалить мероприятие.",
                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
        }
    }
}
