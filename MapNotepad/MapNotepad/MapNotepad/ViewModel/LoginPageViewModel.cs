using MapNotepad.Services.Authentification;
using Prism.Navigation;
using System.Threading.Tasks;
using System.Windows.Input;
using MapNotepad.Helpers;
using MapNotepad.Services.Settings;
using System;
using Xamarin.Auth.Presenters;
using Xamarin.Forms;
using Xamarin.Essentials;
using Xamarin.Auth;
using MapNotepad.AppConstants;
using System.Linq;
using MapNotepad.Helpers.AuthHelpers;
using Newtonsoft.Json;
using System.Diagnostics;

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
        public ICommand GoogleAuthCommand { get; set; }
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
            GoogleAuthCommand = new Command(OnGoogleAuthCommand);
            LeftButtonTap = SingleExecutionCommand.FromFunc(GoBackAsync);
            BlueButtonTap = SingleExecutionCommand.FromFunc(OnBlueButtonTap, () => true);
            if (_settingsManager.UserId != null)
            {
                Email = _settingsManager.UserId;
            }
        }
        void OnGoogleAuthCommand()
        {
            string clientId = null;
            string redirectUri = null;
            switch (Device.RuntimePlatform)
            {
                case Device.iOS:
                    clientId = Constants.iOSClientId;
                    redirectUri = Constants.iOSRedirectUrl;
                    break;

                case Device.Android:
                    clientId = Constants.AndroidClientId;
                    redirectUri = Constants.AndroidRedirectUrl;
                    break;
            }
            var authenticator = new OAuth2Authenticator(
                clientId,
                null,
                Constants.Scope,
                new Uri(Constants.AuthorizeUrl),
                new Uri(redirectUri),
                new Uri(Constants.AccessTokenUrl),
                null,
                true);

            authenticator.Completed += OnAuthCompleted;
            authenticator.Error += OnAuthError;

            AuthenticationState.Authenticator = authenticator;

            var presenter = new OAuthLoginPresenter();
            presenter.Login(authenticator);
        }
        async void OnAuthCompleted(object sender, AuthenticatorCompletedEventArgs e)
        {
            var authenticator = sender as OAuth2Authenticator;
            if (authenticator != null)
            {
                authenticator.Completed -= OnAuthCompleted;
                authenticator.Error -= OnAuthError;
            }
            User user = null;
            if (e.IsAuthenticated)
            {
                // If the user is authenticated, request their basic user data from Google
                // UserInfoUrl = https://www.googleapis.com/oauth2/v2/userinfo
                var request = new OAuth2Request("GET", new Uri(Constants.UserInfoUrl), null, e.Account);
                var response = await request.GetResponseAsync();
                if (response != null)
                {
                    // Deserialize the data and store it in the account store
                    // The users email address will be used to identify data in SimpleDB
                    string userJson = await response.GetResponseTextAsync();
                    user = JsonConvert.DeserializeObject<User>(userJson);
                }
                if(user != null)
                {
                    _authentificationService.RegisterWithGoogleAccount(user.Name, user.Email);
                    await GoToMainPageAsync();
                }
            }
        }
        void OnAuthError(object sender, AuthenticatorErrorEventArgs e)
        {
            var authenticator = sender as OAuth2Authenticator;
            if (authenticator != null)
            {
                authenticator.Completed -= OnAuthCompleted;
                authenticator.Error -= OnAuthError;
            }
            Debug.WriteLine("Authentication error: " + e.Message);
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
