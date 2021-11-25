using MapNotepad.Enums;
using MapNotepad.Model;
using MapNotepad.Services.Repository;
using MapNotepad.Services.Settings;
using Prism.Navigation;
using System.Linq;
using System.Threading.Tasks;
using System;
using Xamarin.Auth.Presenters;
using Xamarin.Forms;
using Xamarin.Auth;
using MapNotepad.AppConstants;
using MapNotepad.Helpers.AuthHelpers;
using Newtonsoft.Json;
using System.Diagnostics;
using MapNotepad.ViewModel;
using Acr.UserDialogs;

namespace MapNotepad.Services.Authentification
{
  public  class AuthentificationService : IAuthentificationService
    {
        private readonly INavigationService _navigationService;
        private readonly IRepository _repository;
        private readonly ISettingsManager _settingsManager;
        public AuthentificationService(IRepository repository,
                                       ISettingsManager settingsManager, 
                                       INavigationService navigationService)
        {
            _repository = repository;
            _settingsManager = settingsManager;
            _navigationService = navigationService;
        }
        #region --- Interface implementation ---
        public void Register(string username, string email)
        {
            Helpers.Global.UserId = email;
            _settingsManager.UserName = username;
        }
        public void RegisterWithGoogleAccount()
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
        public async Task RegisterAsync(string password)
        {
            await _repository.InsertAsync(new UserModel()
            {
                UserName = _settingsManager.UserName,
                Email = Helpers.Global.UserId,
                Password = password
            });
            _settingsManager.UserId = Helpers.Global.UserId;
            Helpers.Global.UserId = null;
        }
        public async Task< ValidationResults> ValidateAsync(string username, string email)
        {
            ValidationResults resultValue = new ValidationResults();
            var users = await _repository.GetAllAsync<UserModel>();
            
            if ((username == null || username == "" ) && (email == null || email == ""))
            {
                resultValue = ValidationResults.EmptyAll;
            }
            else if(username == null || username== "")
            {
                resultValue = ValidationResults.EmptyName;
            }
            else if(email == null || email == "")
            {
                resultValue = ValidationResults.EmptyEmail;
            }
            else
            {
                var subs = email.Split("@");
                if (subs.Length != 2)
                {
                    resultValue = ValidationResults.IncorrectEmail;
                }
                else if( subs[0].Length > 64 || subs[1].Length > 64 || subs[1].Length < 3 )
                {
                    resultValue = ValidationResults.IncorrectEmail;
                }
                else if (users?.Count(x => x.Email == email) > 0)
                {
                    resultValue = ValidationResults.BusyEmail;
                }
                else
                {
                    resultValue = ValidationResults.Correct;
                }
            }
            return resultValue;
        }
        public ValidationResults Validate(string password)
        {
            var validationResult = new ValidationResults();
            if (password == null || password == "")
            {
                validationResult = ValidationResults.EmptyPassword;
            }
            else if (password.Length < 6)
            {
                validationResult = ValidationResults.TooShortPassword;
            }
            else if (!password.Any(char.IsUpper))
            {
                validationResult = ValidationResults.NoUpperCasePassword;
            }
            else if (!password.Any(char.IsDigit))
            {
                validationResult = ValidationResults.NoNumberPassword;
            }
            else
            {
                validationResult = ValidationResults.Correct;
            }
            return validationResult;
        }
        public async Task<VerficationResult> VerificateAsync(string email, string password)
        {
          VerficationResult result = new VerficationResult();
          var users =  await _repository.GetAllAsync<UserModel>();
            if(users?.Count(x => x.Email == email) == 0)
            {
                result = VerficationResult.NoSuchEmail;
            }
            else
            {
                foreach (var user in users)
                {
                    if (user.Email == email && user.Password == password)
                    {
                        result = VerficationResult.Correct;
                        _settingsManager.UserId = email;
                        break;
                    }
                    else
                    {
                        result = VerficationResult.WrongPassword;
                    }
                }
            }
            return result;
        }
        public async Task Logout()
        {
            _settingsManager.UserId = null;
            _settingsManager.UserName = null;
           await _navigationService.NavigateAsync("/LoginAndRegisterPage");
        }
        #endregion
        #region --- Private Helpers ---
        private async void OnAuthCompleted(object sender, AuthenticatorCompletedEventArgs e)
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
                var request = new OAuth2Request("GET", new Uri(Constants.UserInfoUrl), null, e.Account);
                var response = await request.GetResponseAsync();
                if (response != null)
                {
                    string userJson = await response.GetResponseTextAsync();
                    user = JsonConvert.DeserializeObject<User>(userJson);
                }
                if (user != null && user.Email != null)
                {
                    _settingsManager.UserId = user.Email;
                    _settingsManager.UserName = user.Name != null ? user.Name : user.Email.Split("@")[0];
                    Helpers.Global.UserId = null;

                    UserDialogs.Instance.ShowLoading("Authorization ...");
                    await Task.Delay(300);
                    await _navigationService.NavigateAsync("/MainPage");
                }
                else
                {
                  await  UserDialogs.Instance.AlertAsync("Authorization via Google account failed, perhaps there is no Internet connection");
                }
            }
        }
        private void OnAuthError(object sender, AuthenticatorErrorEventArgs e)
        {
            var authenticator = sender as OAuth2Authenticator;
            if (authenticator != null)
            {
                authenticator.Completed -= OnAuthCompleted;
                authenticator.Error -= OnAuthError;
            }
            Debug.WriteLine("Authentication error: " + e.Message);
        }
        #endregion
    }
}
