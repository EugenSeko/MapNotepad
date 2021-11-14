using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using TikBid.Helpers;
using Xamarin.Forms;

namespace MapNotepad.ViewModel
{
    class RegisterAndPasswordViewModel:BaseViewModel
    {
        public RegisterAndPasswordViewModel(INavigationService navigationService):base(navigationService)
        {
            LeftButtonTap = SingleExecutionCommand.FromFunc(GoBackAsync);
        }


        #region --- Public Properties ---
        public ICommand LeftButtonTap { get; set; }
        #endregion
        #region --- Overrides ---
        #endregion
        #region --- Private Helpers ---
        #endregion
    }
}
