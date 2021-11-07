using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Text;

namespace MapNotepad.ViewModel
{
    class AddPinViewModel : BaseViewModel
    {
        public AddPinViewModel(INavigationService navigationService):base(navigationService){}

        #region ---Public Properties---
        private String _headerText="Add Pin";
        public string HeaderText
        {
            get => _headerText;
            set => SetProperty(ref _headerText, value);
        }
        #endregion


    }
}
