using Eventor.Common.Core;
using Eventor.Common.Enums;
using Eventor.Services;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Xml.Linq;

namespace Eventor.GUI.Controllers;

public class LoginFormController
{
    IOauthService _oauthService;
    IUserService _userService;

    private string _name;
    public string Name
    {
        get => _name;
        set
        {
            _name = value;
            OnPropertyChanged();
        }
    }

    private string _phone;
    public string Phone
    {
        get => _phone;
        set
        {
            _phone = value;
            OnPropertyChanged();
        }
    }

    private string _gender;
    public string Gender
    {
        get => _gender;
        set
        {
            _gender = value;
            OnPropertyChanged();
        }
    }

    private string _password;
    public string Password
    {
        get => _password;
        set
        {
            _password = value;
            OnPropertyChanged();
        }
    }

    private string _confirmPassword;
    public string ConfirmPassword
    {
        get => _confirmPassword;
        set
        {
            _confirmPassword = value;
            OnPropertyChanged();
        }
    }

    private string _errorMessage;
    public string ErrorMessage
    {
        get => _errorMessage;
        set
        {
            _errorMessage = value;
            OnPropertyChanged();
        }
    }

    public User? AuthenticatedUser { get; private set; }
    public event PropertyChangedEventHandler PropertyChanged;

    public LoginFormController(
        IOauthService oauthService,
        IUserService userService)
    {
        _oauthService = oauthService;
        _userService = userService;
    }

    public async Task<bool> TryRegisterAsync()
    {
        try
        {
            if (Password != ConfirmPassword)
            {
                ErrorMessage = "Пароли не совпадают";
                return false;
            }

            var user = new User(
                id: Guid.NewGuid(),
                name: Name,
                phone: Phone,
                gender: Gender.ToGender(),
                passwordHash: string.Empty,
                role: UserRole.User
            );

            await _oauthService.Registrate(user, Password);
            AuthenticatedUser = await _userService.GetUserByIdAsync(user.Id);
            return true;
        }
        catch (Exception ex)
        {
            ErrorMessage = ex.Message;
            return false;
        }
    }

    public async Task<bool> TryLoginAsync()
    {
        try
        {
            await _oauthService.Login(Phone, Password);
            AuthenticatedUser = await _userService.GetUserByPhoneAsync(Phone);
            return true;
        }
        catch (Exception ex)
        {
            ErrorMessage = ex.Message;
            return false;
        }
    }

    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
