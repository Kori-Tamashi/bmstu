using Eventor.Common.Core;
using Eventor.Common.Enums;
using Eventor.Database.Core;
using Eventor.GUI.Controllers;
using Eventor.Services;
using User = Eventor.Common.Core.User;

namespace Eventor.GUI;

public partial class LoginForm : Form
{
    private readonly LoginFormController _loginFormController;

    public User? AuthenticatedUser => _loginFormController.AuthenticatedUser;

    public LoginForm(LoginFormController loginFormController)
    {
        _loginFormController = loginFormController;
        InitializeComponent();
        InitializeBindings();
    }

    private void InitializeBindings()
    {
        var loginFormController = new BindingSource { DataSource = _loginFormController };

        userName_textBox.DataBindings.Add("Text", loginFormController, "Name");
        userPhone_maskedTextBox.DataBindings.Add("Text", loginFormController, "Phone");
        userGender_comboBox.DataBindings.Add("Text", loginFormController, "Gender");
        userPassword_textBox.DataBindings.Add("Text", loginFormController, "Password");
        userCheckPassword_textBox.DataBindings.Add("Text", loginFormController, "ConfirmPassword");
    }

    private async void registrate_button_Click(object sender, EventArgs e)
    {
        if (await _loginFormController.TryRegisterAsync())
        {
            MessageBox.Show("Вы успешно зарегистрировались в системе.");
            DialogResult = DialogResult.OK;
            Close();
            
        }
        else
        {
            MessageBox.Show("Ошибка: не удалось произвести регистрацию.");
        }
    }

    private async void authorization_button_Click(object sender, EventArgs e)
    {
        if (await _loginFormController.TryLoginAsync())
        {
            MessageBox.Show("Вы успешно авторизовались в системе.");
            DialogResult = DialogResult.OK;
            Close();
        }
        else
        {
            MessageBox.Show("Ошибка: не удалось произвести авторизацию.");
        }
    }
}
