using Eventor.Database.Core;
using Eventor.Services;
using Eventor.Services.Exceptions;

namespace Eventor
{
    public partial class MainWindow : Form
    {
        IUserService _userService;
        IOauthService _oauthService;
        IEventService _eventService;
        IDayService _dayService;
        IPersonService _personService;
        IMenuService _menuService;
        IItemService _itemService;
        IEconomyService _economyService;
        IFeedbackService _feedbackService;

        public MainWindow(
            IUserService userService,
            IOauthService oauthService,
            IEventService eventService,
            IDayService dayService,
            IPersonService personService,
            IMenuService menuService,
            IItemService itemService,
            IEconomyService economyService,
            IFeedbackService feedbackService
            )
        {
            _userService = userService;
            _oauthService = oauthService;
            _eventService = eventService;
            _dayService = dayService;
            _personService = personService;
            _menuService = menuService;
            _itemService = itemService;
            _economyService = economyService;
            _feedbackService = feedbackService;

            InitializeComponent();
        }

        public MainWindow()
        {
            InitializeComponent();
        }

        private void MainWindow_Load(object sender, EventArgs e)
        {

        }

        private async void userName_textBox_TextChanged(object sender, EventArgs e)
        {
            var phone = userPhone_maskedTextBox.Text.Trim();
            var newName = userName_textBox.Text.Trim();

            if (string.IsNullOrEmpty(phone))
            {
                MessageBox.Show("Номер телефона отсутствует.");
                return;
            }

            if (string.IsNullOrEmpty(newName))
            {
                MessageBox.Show("Новое имя отсутствует.");
                return;
            }

            try
            {
                await _userService.UpdateUserNameAsync(phone, newName);
            }
            catch (UserNotFoundException ex)
            {
                MessageBox.Show(ex.Message);
            }
            catch (UserUpdateException ex)
            {
                MessageBox.Show(ex.Message);
            }
            catch (UserServiceException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
