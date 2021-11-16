using MapNotepad.Services.Authentification;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using MapNotepad.Helpers;

namespace MapNotepad.ViewModel
{
    class RegisterAndPasswordViewModel:BaseViewModel
    {
        private readonly IAuthentificationService _authentificationService;
        public RegisterAndPasswordViewModel(INavigationService navigationService, IAuthentificationService authentificationService):base(navigationService)
        {
            _authentificationService = authentificationService;
            LeftButtonTap = SingleExecutionCommand.FromFunc(GoBackAsync);
            BlueButtonTap = SingleExecutionCommand.FromFunc(OnBlueButtonTap, ()=>true);
        }


        #region --- Public Properties ---
        public ICommand LeftButtonTap { get; set; }
        public ICommand BlueButtonTap { get; set; }

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

        public bool IsConfirmPasswordErrorMessageVisible => ConfirmPasswordErrorMessage != null && ConfirmPasswordErrorMessage != "";
        private string _confirmpasswordErrorMessage;
        public string ConfirmPasswordErrorMessage
        {
            get => _confirmpasswordErrorMessage;
            set
            {
                SetProperty(ref _confirmpasswordErrorMessage, value);
                RaisePropertyChanged(nameof(IsConfirmPasswordErrorMessageVisible));
            }
        }
        private string _confirmPassword;
        public string ConfirmPassword
        {
            get => _confirmPassword;
            set => SetProperty(ref _confirmPassword, value);
        }
        #endregion
        #region --- Overrides ---
        #endregion
        #region --- Private Helpers ---
        private Task OnBlueButtonTap()
        {
            PasswordErrorMessage = null;
            ConfirmPasswordErrorMessage = null;
            
                switch (_authentificationService.Validate(Password))
                {
                    case Enums.ValidationResults.EmptyPassword:
                        {
                            PasswordErrorMessage = "Empty field";
                            break;
                        }
                    case Enums.ValidationResults.TooShortPassword:
                        {
                            PasswordErrorMessage = "To short password";
                            break;
                        }
                    case Enums.ValidationResults.NoNumberPassword:
                        {
                            PasswordErrorMessage = "Password must contain at least one number";
                            break;
                        }
                    case Enums.ValidationResults.NoUpperCasePassword:
                        {
                            PasswordErrorMessage = "Password must contain at least one uppercase character";
                            break;
                        }
                    case Enums.ValidationResults.IncorrectPassword:
                        {
                            PasswordErrorMessage = "Incorrect password";
                            break;
                        }
                    case Enums.ValidationResults.Correct:
                        {
                            if (Password != ConfirmPassword && Password != null && Password != "")
                            {
                                ConfirmPasswordErrorMessage = "Incorrect confirm";
                                break;
                            }
                            else
                            {
                            _authentificationService.RegisterAsync(Password);
                                GoToLoginPageAsync();
                                break;
                            }
                       
                        }
                }
            
            return Task.CompletedTask;
        }
        #endregion
    }
}
