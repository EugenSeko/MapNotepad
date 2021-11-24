using MapNotepad.Services.Authentification;
using Prism.Navigation;
using System.Threading.Tasks;
using System.Windows.Input;
using MapNotepad.Helpers;
using MapNotepad.Services.Settings;

namespace MapNotepad.ViewModel
{
    class LoginPageViewModel : BaseViewModel
    {
        private readonly IAuthentificationService _authentificationService;
        private readonly ISettingsManager _settingsManager;
        public LoginPageViewModel(INavigationService navigationService, 
            IAuthentificationService authentificationService, 
            ISettingsManager settingsManager) : base(navigationService)
        {
            _settingsManager = settingsManager;
            _authentificationService = authentificationService;
            Init();
        }
        #region --- Public Properties ---
        public ICommand LeftButtonTap { get; set; }
        public ICommand BlueButtonTap { get; set; }
        public bool IsEmailErrorMessageVisible => EmailErrorMessage != null && EmailErrorMessage != "";
        private string _emailErrorMessage;
        public string EmailErrorMessage
        {
            get => _emailErrorMessage;
            set
            {
                SetProperty(ref _emailErrorMessage, value);
                RaisePropertyChanged(nameof(IsEmailErrorMessageVisible));
            }
        }
        private string _email;
        public string Email
        {
            get => _email;
            set => SetProperty(ref _email, value);
        }
        public bool IsPasswordErrorMessageVisible => PasswordErrorMessage != null && PasswordErrorMessage != "";
        private string _passwordErrorMessage;
        public string PasswordErrorMessage
        {
            get => _passwordErrorMessage;
            set
            {
                SetProperty(ref _passwordErrorMessage, value);
                RaisePropertyChanged(nameof(IsPasswordErrorMessageVisible));
            }
        }
        private string _password;
        public string Password
        {
            get => _password;
            set => SetProperty(ref _password, value);
        }
        #endregion
        #region --- Private Helpers ---
        private void Init()
        {
            LeftButtonTap = SingleExecutionCommand.FromFunc(GoBackAsync);
            BlueButtonTap = SingleExecutionCommand.FromFunc(OnBlueButtonTap, () => true);
            if (_settingsManager.UserId != null)
            {
                Email = _settingsManager.UserId;
            }
        }
        private async Task OnBlueButtonTap()
        {
            PasswordErrorMessage = null;
            EmailErrorMessage = null;
            Enums.ValidationResults authRes = await _authentificationService.ValidateAsync(Password, Email);
            Enums.VerficationResult verifRes = await _authentificationService.VerificateAsync(Email, Password);
            switch (authRes)
            {
                case Enums.ValidationResults.EmptyEmail:
                    {
                        EmailErrorMessage = "Empty field";
                        break;
                    }
                case Enums.ValidationResults.EmptyName:
                    {
                        PasswordErrorMessage = "Empty field";
                        break;
                    }
                case Enums.ValidationResults.EmptyAll:
                    {
                        PasswordErrorMessage = "Empty field";
                        EmailErrorMessage = "Empty field";
                        break;
                    }
                case Enums.ValidationResults.IncorrectEmail:
                    {
                        EmailErrorMessage = "Incorrect email format";
                        break;
                    }
                default:
                    {
                        switch (verifRes)
                        {
                            case Enums.VerficationResult.NoSuchEmail:
                                {
                                    EmailErrorMessage = "No such email exist";
                                    break;
                                }
                            case Enums.VerficationResult.WrongPassword:
                                {
                                    PasswordErrorMessage = "Wrong password";
                                    break;
                                }
                            case Enums.VerficationResult.Correct:
                                {
                                    await GoToMainPageAsync();
                                    break;
                                }
                        }
                        break;
                    }
            }
        }
        #endregion
    }
}
