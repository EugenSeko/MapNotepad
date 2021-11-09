﻿using MapNotepad.Model;
using MapNotepad.Services.Authentification;
using MapNotepad.Services.PinService;
using MapNotepad.Services.Repository;
using MapNotepad.Services.Settings;
using MapNotepad.Views;
using MapNotepad.ViewModel;
using Prism.Ioc;
using Prism.Unity;
using Xamarin.Forms;

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
            //services
            containerRegistry.RegisterInstance<IRepository>(Container.Resolve<Repository>());
            containerRegistry.RegisterInstance<ISettingsManager>(Container.Resolve<SettingsManager>());
            containerRegistry.RegisterInstance<IAuthentificationService>(Container.Resolve<AuthentificationService>());
            containerRegistry.RegisterInstance<IPinService>(Container.Resolve<PinService>());

            //navigation
            containerRegistry.RegisterForNavigation<NavigationPage>();
            containerRegistry.RegisterForNavigation<MainPage,MainPageViewModel>();
            containerRegistry.RegisterForNavigation<LoginAndRegisterPage, LoginAndRegisterPageViewModel>();
            containerRegistry.RegisterForNavigation<LoginPage, LoginPageViewModel>();
            containerRegistry.RegisterForNavigation<PinsListPage, PinsListViewModel>();
            containerRegistry.RegisterForNavigation<AddPinPage, AddPinViewModel>();
            containerRegistry.RegisterForNavigation<MapPage, MapPageViewModel>();

            //test
            containerRegistry.RegisterForNavigation<TestPage>();
        }

        protected override void OnInitialized()
        {
            InitializeComponent();
            NavigationService.NavigateAsync($"{nameof(MainPage)}");
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
