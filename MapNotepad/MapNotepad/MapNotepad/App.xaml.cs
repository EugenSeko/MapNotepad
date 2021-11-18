using MapNotepad.Model;
using MapNotepad.Services.Authentification;
using MapNotepad.Services.PinService;
using MapNotepad.Services.Repository;
using MapNotepad.Services.Settings;
using MapNotepad.Views;
using MapNotepad.ViewModel;
using Prism.Ioc;
using Prism.Unity;
using Xamarin.Forms;
using MapNotepad.Services.SearchService;
using System.Collections.Generic;
using MapNotepad.Resources;

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
            containerRegistry.RegisterInstance<ISearchServise>(Container.Resolve<SearchService>());

            //navigation
            containerRegistry.RegisterForNavigation<NavigationPage>();
            containerRegistry.RegisterForNavigation<MainPage,MainPageViewModel>();
            containerRegistry.RegisterForNavigation<LoginAndRegisterPage, LoginAndRegisterPageViewModel>();
            containerRegistry.RegisterForNavigation<LoginPage, LoginPageViewModel>();
            containerRegistry.RegisterForNavigation<PinsListPage, PinsListViewModel>();
            containerRegistry.RegisterForNavigation<AddPinPage, AddPinViewModel>();
            containerRegistry.RegisterForNavigation<MapPage, MapPageViewModel>();
            containerRegistry.RegisterForNavigation<RegisterPage, RegisterPageViewModel>();
            containerRegistry.RegisterForNavigation<RegisterAndPassword, RegisterAndPasswordViewModel>();
        }

        protected override void OnInitialized()
        {
            InitializeComponent();

            Current.Resources.MergedDictionaries.Add(new BaseStyles());

            var settingsManager = Container.Resolve<ISettingsManager>();

           

            // NavigationService.NavigateAsync($"/{nameof(LoginAndRegisterPage)}");

            if (settingsManager.UserId == null)
            {
                NavigationService.NavigateAsync("/" + nameof(LoginAndRegisterPage));
            }
            else
            {
                NavigationService.NavigateAsync("/" + nameof(MainPage));
            }
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
