using MapNotepad.Services.Authentification;
using Prism.Navigation;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Input;
using TikBid.Helpers;
using Xamarin.Forms;

namespace MapNotepad.ViewModel
{
    class RegisterPageViewModel : BaseViewModel
    {
        private readonly IAuthentificationService _authentificationService;
        public RegisterPageViewModel(INavigationService navigationService, IAuthentificationService authentificationService):base(navigationService)
        {
            _authentificationService = authentificationService;
            LeftButtonTap = SingleExecutionCommand.FromFunc(GoBackAsync);
            NextButtonTap = SingleExecutionCommand.FromFunc(OnNextButtonTapAsync, () => true); 
        }

        #region --- Public Properties ---
        public ICommand LeftButtonTap { get; set; }
        public ICommand NextButtonTap { get; set; }


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


        public bool IsNameErrorMessageVisible => NameErrorMessage != null && NameErrorMessage != "";
        private string _nameErrorMessage;
        public string NameErrorMessage
        {
            get => _nameErrorMessage;
            set
            { 
                SetProperty(ref _nameErrorMessage, value);
                RaisePropertyChanged(nameof(IsNameErrorMessageVisible));
            }
        }
        private string _name;
        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }
        #endregion
        #region --- Overrides ---
        protected override void OnPropertyChanged(PropertyChangedEventArgs args)
        {
            base.OnPropertyChanged(args);
            switch (args.PropertyName)
            {
                case nameof(Name):
                    {
                        if(Name != null && Name != "")
                        {
                            NameErrorMessage = null;
                        }
                        break;
                    }
                case nameof(Email):
                    {
                        if (Email != null || Email != "")
                        {
                            EmailErrorMessage = null;
                        }
                        break;
                    }

            }
        }
        #endregion
        #region --- Private Helpers ---
        private async Task OnNextButtonTapAsync()
        {
            var authRes = await _authentificationService.ValidateAsync(Name, Email);
            switch (authRes)
            {
                case Enums.ValidationResults.BusyEmail:
                    {
                        EmailErrorMessage = "Email already exists";
                        break;
                    }
                case Enums.ValidationResults.EmptyName: 
                    {
                        NameErrorMessage = "Empty field";
                        break;
                    }       
                case Enums.ValidationResults.EmptyEmail: 
                    {
                        EmailErrorMessage = "Empty field";
                        break;
                    }
                case Enums.ValidationResults.EmptyAll: 
                    {
                        NameErrorMessage = "Empty field";
                        EmailErrorMessage = "Empty field";
                        break;

                    }
                case Enums.ValidationResults.IncorrectEmail: 
                    {
                        EmailErrorMessage = "Incorrect email";
                        break;

                    }
                case Enums.ValidationResults.Correct:
                    {
                        _authentificationService.Register(Name, Email);
                       await GoToRegisterAndPasswordPage();
                        break;

                    }
            }
        }
        #endregion
    }
}
