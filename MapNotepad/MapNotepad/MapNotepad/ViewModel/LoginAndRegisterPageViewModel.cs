using Prism.Navigation;
using System.Windows.Input;
using MapNotepad.Helpers;

namespace MapNotepad.ViewModel
{
    class LoginAndRegisterPageViewModel : BaseViewModel
    {
        public LoginAndRegisterPageViewModel(INavigationService navigationService):base(navigationService)
        {
            LoginButtonTap = SingleExecutionCommand.FromFunc(GoToLoginPageAsync);
            RegisterButtonTap = SingleExecutionCommand.FromFunc(GoToRegisterPageAsync);  
        }

        #region --- Public Properties ---
        public ICommand LoginButtonTap { get; set; }
        public ICommand RegisterButtonTap { get; set; }
        #endregion
    }
}
