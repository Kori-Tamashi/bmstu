using Eventor.Common.Core;
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
        await _mainWindowController.InitializeAsync();
        InitializeUserEventsGrid();
        InitializeAllEventsGrid();
        InitializeTimer();
    }

    private void InitializeBindings()
    {
        var userBindingSource = new BindingSource { DataSource = _mainWindowController };

        // User info
        userName_textBox.DataBindings.Add("Text", userBindingSource, "CurrentUser.Name");
        userPhone_maskedTextBox.DataBindings.Add("Text", userBindingSource, "CurrentUser.Phone");
        userGender_comboBox.DataBindings.Add("Text", userBindingSource, "CurrentGender");
        userRole_textBox.DataBindings.Add("Text", userBindingSource, "CurrentRole");
    }

    private void InitializeUserEventsGrid()
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

    private void InitializeAllEventsGrid()
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
            row.Cells[3].Value = evt.Date.ToString("dd.MM.yyyy");
            row.Cells[4].Value = evt.PersonCount;
            row.Cells[5].Value = evt.DaysCount;
            row.Cells[6].Value = evt.Rating;

            row.Tag = evt; 
            events_dataGridView.Rows.Add(row);
        }
    }

    private void InitializeTimer()
    {
        _timer.Tick += async (sender, e) =>
        {
            await _mainWindowController.InitializeAsync();
            InitializeUserEventsGrid();
            InitializeAllEventsGrid();
        };
        _timer.Start();
    }

    #endregion

    public void SetController(MainWindowController controller)
    {
        _mainWindowController = controller;
        InitializeComponent();
        InitializeBindings();
    }

    private async void userInfoSave_button_Click(object sender, EventArgs e)
    {
        var newName = userName_textBox.Text.Trim();
        var newGender = userGender_comboBox.Text.Trim();
        var newPhone = userPhone_maskedTextBox.Text.Trim();
        var newRole = userRole_textBox.Text.Trim();

        await _mainWindowController.SaveUser(newName, newGender, newPhone, newRole);
    }

    private async void events_dataGridView_CellClick(object sender, DataGridViewCellEventArgs e)
    {
        if (e.RowIndex < 0) return;
        var row = events_dataGridView.Rows[e.RowIndex];
        var selectedEvent = row.Tag as Event;

        await _mainWindowController.OpenEventDetails(selectedEvent);
    }
}
