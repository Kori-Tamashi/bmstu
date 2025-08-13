using Eventor.GUI.Controllers;

namespace Eventor.GUI;

public partial class ParticipationForm : Form
{
    private readonly ParticipationFormController _participationFormController;

    public ParticipationForm(ParticipationFormController controller)
    {
        _participationFormController = controller;
        InitializeComponent();
    }

    private async void ParticipationForm_Load(object sender, EventArgs e)
    {
        try
        {
            await _participationFormController.InitializeAsync();
            InitializeDaysCheckBox();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Ошибка: не удалось загрузить форму выбора дней присутствия на мероприятии.");
            return;
        }
        
    }

    private void InitializeDaysCheckBox()
    {
        try
        {
            days_checkedListBox.Items.Clear();
            foreach (var label in _participationFormController.DayLabels)
            {
                days_checkedListBox.Items.Add(label);
            }

            for (int i = 0; i < days_checkedListBox.Items.Count; i++)
            {
                days_checkedListBox.SetItemChecked(i, _participationFormController.SelectedDays.Contains(i + 1));
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Ошибка: не удалось загрузить дни мероприятия.");
            return;
        }
    }

    public void SetIds(Guid eventId, Guid userId)
    {
        try
        {
            _participationFormController.EventId = eventId;
            _participationFormController.UserId = userId;
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Ошибка: не удалось передать данные форме выбора дней присутствия.");
            return;
        }
    }

    private async void saveDays_button_Click(object sender, EventArgs e)
    {
        try
        {
            var selectedIndices = days_checkedListBox.CheckedIndices
            .Cast<int>()
            .ToList();

            _participationFormController.UpdateSelectedDays(selectedIndices);
            await _participationFormController.SaveParticipationAsync();

            if (_participationFormController.SelectedDays.Count != 0)
            {
                MessageBox.Show("Вы успешно зарегистрировались на мероприятие.");
            }
            else
            {
                MessageBox.Show("Вы успешно покинули мероприятие.");
            }

            Close();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Ошибка: не удалось изменить дни присутствия на мероприятии.");
            return;
        }
    }
}