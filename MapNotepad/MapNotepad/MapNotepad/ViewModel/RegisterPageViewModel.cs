using MapNotepad.Services.Authentification;
using Prism.Navigation;
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
            NextButtonTap = SingleExecutionCommand.FromFunc(OnNextButtonTapAsync); // ()=>false
        }

        #region --- Public Properties ---
        public ICommand LeftButtonTap { get; set; }
        public ICommand NextButtonTap { get; set; }

        private bool _isNameErrorMessageVisible;
        public bool IsNameErrorMessageVisible 
        {
            get => _isNameErrorMessageVisible;
            set => SetProperty(ref _isNameErrorMessageVisible, value);
        }

        private bool _isEmailErrorMessageVisible;
        public bool IsEmailErrorMessageVisible
        {
            get => _isEmailErrorMessageVisible;
            set => SetProperty(ref _isEmailErrorMessageVisible, value);
        }

        private string _emailErrorMessage;
        public string EmailErrorMessage
        {
            get => _emailErrorMessage;
            set => SetProperty(ref _emailErrorMessage, value);
        }

        private string _nameErrorMessage;
        public string NameErrorMessage
        {
            get => _nameErrorMessage;
            set => SetProperty(ref _nameErrorMessage, value);
        }

        private string _email;
        public string Email
        {
            get => _email;
            set => SetProperty(ref _email, value);
        }

        private string _name;
        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }
        #endregion
        #region --- Overrides ---
        #endregion
        #region --- Private Helpers ---
        private Task OnNextButtonTapAsync()
        {
            switch (_authentificationService.Validate(Name, Email))
            {
                case Enums.ValidationResults.EmptyName: 
                    {
                        IsNameErrorMessageVisible = true;
                        NameErrorMessage = "Empty field";
                        break;
                    }       
                case Enums.ValidationResults.EmptyEmail: 
                    {
                        IsEmailErrorMessageVisible = true;
                        EmailErrorMessage = "Empty field";
                        break;
                    }
                case Enums.ValidationResults.EmptyAll: 
                    {
                        IsEmailErrorMessageVisible = true;
                        IsNameErrorMessageVisible = true;
                        NameErrorMessage = "Empty field";
                        EmailErrorMessage = "Empty field";
                        break;

                    }
                case Enums.ValidationResults.IncorrectEmail: 
                    {
                        IsEmailErrorMessageVisible = true;
                        EmailErrorMessage = "Incorrect email";
                        break;

                    }
                case Enums.ValidationResults.Correct:
                    {
                        GoToRegisterAndPasswordPage();
                        break;
                    }
            }
            return Task.CompletedTask;
        }
        #endregion
    }
}
