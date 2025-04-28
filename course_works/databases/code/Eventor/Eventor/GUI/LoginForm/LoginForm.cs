using Eventor.Common.Core;
using Eventor.Common.Enums;
using Eventor.Database.Core;
using Eventor.Services;
using User = Eventor.Common.Core.User;

namespace Eventor.GUI;

public partial class LoginForm : Form
{
    IOauthService _oauthService;
    IUserService _userService;

    public User? AuthenticatedUser { get; private set; }

    public LoginForm(IOauthService oauthService,
        IUserService userService)
    {
        _oauthService = oauthService;
        _userService = userService;

        InitializeComponent();
    }

    private async void registrate_button_Click(object sender, EventArgs e)
    {
        var name = userName_textBox.Text.Trim();
        var gender = userGender_comboBox.Text.Trim();
        var phone = userPhone_maskedTextBox.Text.Trim();
        var password = userPassword_textBox.Text.Trim();
        var checkPassword = userCheckPassword_textBox.Text.Trim();

        if (password != checkPassword)
        {
            MessageBox.Show("Пароли для регистрации не совпадают.");
            return;
        }

        try
        {
            var user = new User(
                id: Guid.NewGuid(),
                name: name,
                phone: phone,
                gender: gender.ToGender(),
                passwordHash: string.Empty,
                role: UserRole.User
            );

            await _oauthService.Registrate(user, password);

            AuthenticatedUser = await _userService.GetUserByIdAsync(user.Id);
            DialogResult = DialogResult.OK;
            Close();
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message);
            return;
        }

        MessageBox.Show("Вы успешно зарегистрировались.");
    }

    private async void authorization_button_Click(object sender, EventArgs e)
    {
        var name = userName_textBox.Text.Trim();
        var gender = userGender_comboBox.Text.Trim();
        var phone = userPhone_maskedTextBox.Text.Trim();
        var password = userPassword_textBox.Text.Trim();

        try
        {
            await _oauthService.Login(phone, password);

            AuthenticatedUser = await _userService.GetUserByPhoneAsync(phone);
            DialogResult = DialogResult.OK;
            Close();
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message);
            return;
        }

        MessageBox.Show("Вы успешно авторизовались.");
    }

    private void LoginForm_Load(object sender, EventArgs e)
    {

    }
}
