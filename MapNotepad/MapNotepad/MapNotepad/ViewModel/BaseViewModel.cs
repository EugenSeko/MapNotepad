using MapNotepad.View;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Text;

namespace MapNotepad.ViewModel
{

     class BaseViewModel : BindableBase
    {
        private readonly INavigationService _navigationService;
        public BaseViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
        }
        #region --- Navigation ---
        public async void NavigateGoBack()
        {
            await _navigationService.GoBackAsync();
        }

        public async void GoToMainPage()
        {
            await _navigationService.NavigateAsync("/" + nameof(MainPage));
        }
        public async void GoBackToRootAsync()
        {
            await _navigationService.GoBackToRootAsync();
        }
        public async void GoToLoginPage()
        {
            await _navigationService.NavigateAsync(nameof(LoginPage));
        }
        public async void GoToRegisterPage()
        {
            await _navigationService.NavigateAsync("/" + nameof(RegisterPage));
        }
        public async void GoToRegisterAndPasswordPage()
        {
            await _navigationService.NavigateAsync(nameof(RegisterAndPassword));
        }
        public async void GoToLoginAndRegisterPage()
        {
            await _navigationService.NavigateAsync(nameof(LoginAndRegisterPage));
        }
        public async void GoToAddPinPage()
        {
            await _navigationService.NavigateAsync(nameof(AddPinPage));
        }
        #endregion
    }
}
