using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace MapNotepad.ViewModel
{
    class PinsListViewModel : BaseViewModel
    {
        public PinsListViewModel(INavigationService navigationService):base(navigationService)
        {

        }
        #region ---Command---
        public ICommand OnAddButtonTap => new Command(GoToAddPinPage);
        #endregion
        #region ---Public Properties---
        #endregion
        #region ---Privat Helpers---
        #endregion
    }
}
