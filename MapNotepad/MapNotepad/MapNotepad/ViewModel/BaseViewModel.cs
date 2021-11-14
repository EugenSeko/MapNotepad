using MapNotepad.Views;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

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
        public Task GoBackAsync()
        {
            return Task.FromResult(_navigationService.GoBackAsync());
        }

        public async void GoToMainPage()
        {
            await _navigationService.NavigateAsync("/" + nameof(MainPage));
        }
        public async void GoToMainPageListPage()
        {
            await _navigationService.NavigateAsync("/MainPage?selectedTab=PinsListPage");
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
            await _navigationService.NavigateAsync(nameof(RegisterPage));
        }
        public Task GoToRegisterAndPasswordPage()
        {
            return Task.FromResult(_navigationService.NavigateAsync(nameof(RegisterAndPassword)));
        }
        public async void GoToLoginAndRegisterPage()
        {
            await _navigationService.NavigateAsync(nameof(LoginAndRegisterPage));
        }
        public async void GoToAddPinPage()
        {
            await _navigationService.NavigateAsync(nameof(AddPinPage));
        }
        public async void GoToPinsListPage()
        {
            await _navigationService.NavigateAsync(nameof(PinsListPage));
        }
        #endregion
    }
}
