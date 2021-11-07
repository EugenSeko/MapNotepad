using MapNotepad.Model;
using MapNotepad.View;
using MapNotepad.ViewModel;
using Prism.Ioc;
using Prism.Unity;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MapNotepad
{
    public partial class App : PrismApplication
    {
        public App()
        {
        }
        #region --- Overrides ---
        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            //navigation
            containerRegistry.RegisterForNavigation<NavigationPage>();
            containerRegistry.RegisterForNavigation<MainPage,MainPageViewModel>();
            containerRegistry.RegisterForNavigation<LoginAndRegisterPage, LoginAndRegisterPageViewModel>();
            containerRegistry.RegisterForNavigation<LoginPage, LoginPageViewModel>();
            containerRegistry.RegisterForNavigation<TestPage>();
            containerRegistry.RegisterForNavigation<CameraPage>();
            containerRegistry.RegisterForNavigation<BindingPinViewPage>();
            containerRegistry.RegisterForNavigation<CustomPinsPage>();
            containerRegistry.RegisterForNavigation<PinsPage>();






        }

        protected override void OnInitialized()
        {
            InitializeComponent();
            NavigationService.NavigateAsync($"{nameof(PinsPage)}");
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
        #endregion
    }
}
