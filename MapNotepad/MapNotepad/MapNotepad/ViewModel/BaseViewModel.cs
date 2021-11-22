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
        protected static object NavigationParameter { get; set; }
        public Task GoBackAsync()
        {
            return Task.FromResult(_navigationService.GoBackAsync());
        }
        public async Task GoToMainPageAsync()
        {
            await _navigationService.NavigateAsync("/" + nameof(MainPage));
        }
        public async Task GoToMainPageListPageAsync()
        {
            await _navigationService.NavigateAsync("/MainPage?selectedTab=PinsListPage");
        }
        public async Task GoToMainPagePageAsync()
        {
            await _navigationService.NavigateAsync("/MainPage?selectedTab=MapPage");
        }
        public async Task GoBackToRootAsync()
        {
            await _navigationService.GoBackToRootAsync();
        }
        public async Task GoToLoginPageAsync()
        {
            await _navigationService.NavigateAsync(nameof(LoginPage));
        }
        public async Task GoToRegisterPageAsync()
        {
            await _navigationService.NavigateAsync(nameof(RegisterPage));
        }
        public async Task GoToRegisterAndPasswordPageAsync()
        {
           await _navigationService.NavigateAsync(nameof(RegisterAndPassword));
        }
        public async Task GoToLoginAndRegisterPageAsync()
        {
            await _navigationService.NavigateAsync(nameof(LoginAndRegisterPage));
        }
        public async Task GoToAddPinPageAsync()
        {
            await _navigationService.NavigateAsync(nameof(AddPinPage));
        }
        public async Task GoToPinsListPageAsync()
        {
            await _navigationService.NavigateAsync(nameof(PinsListPage));
        }
    }
}
