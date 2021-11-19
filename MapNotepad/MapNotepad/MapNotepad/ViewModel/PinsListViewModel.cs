using MapNotepad.Extensions;
using MapNotepad.Model;
using MapNotepad.Services.PinService;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using MapNotepad.Helpers;
using Xamarin.Forms;
using MapNotepad.Services.Authentification;

namespace MapNotepad.ViewModel
{
    class PinsListViewModel : BaseViewModel
    {
        // TODO: Пользователь нажимает на вкладу Список пинов и видит список пинов, название,
        // описание и локацию.По тапу на пин пользователь переходит на карту с фокусировкой на выбранном пине.
        private readonly IAuthentificationService _authentifService;
        private readonly IPinService _pinservice;
        public PinsListViewModel(INavigationService navigationService, IPinService pinService, 
            IAuthentificationService authentificationService) :base(navigationService)
        {
            _pinservice = pinService;
            _authentifService = authentificationService;
            InitAsync();
        }
        
        #region ---Public Properties---
        public ICommand AddButtonTapCommand { get; set; }
        public ICommand LogoutCommand { get; set; }
        public ICommand GoToSettingsPageCommand { get; set; }

        private PinViewModel _selectedItem;
        public PinViewModel SelectedItem
        {
            get => _selectedItem;
            set => SetProperty(ref _selectedItem, value);
        }
       
        private ObservableCollection<PinViewModel> _pinList;
        public ObservableCollection<PinViewModel> PinList
        {
            get => _pinList;
            set => SetProperty(ref _pinList, value);
        }
        #endregion
        #region ---Privat Helpers---
        private async void InitAsync()
        {
            AddButtonTapCommand = SingleExecutionCommand.FromFunc(GoToAddPinPageAsync);
            GoToSettingsPageCommand = SingleExecutionCommand.FromFunc(GoToAddPinPageAsync);
            LogoutCommand = SingleExecutionCommand.FromFunc(OnLogoutCommand);

            var pl = new ObservableCollection<PinViewModel>();
            var l = await _pinservice.GetPinsAsync();
            foreach (var v in l)
            {
                var pvm = PinExtension.ToPinViewModel(v);
                pvm.DeleteCommand = SingleExecutionCommand.FromFunc(OnDeleteButtonCommand);
                pvm.EditCommand = SingleExecutionCommand.FromFunc(OnEditButtonCommand);

                pl.Add(pvm);
            }
            PinList = pl;
        }

        private Task OnLogoutCommand()
        {
            _authentifService.Logout();
            return Task.CompletedTask;
        }

        private async Task OnDeleteButtonCommand(object obj)
        {

            PinList.Remove(obj as PinViewModel);
            await _pinservice.DeletePinAsync(PinExtension.ToPinModel(obj as PinViewModel));
        }
        private async Task OnEditButtonCommand(object obj)
        {
            NavigationParameter = obj;
            await GoToAddPinPageAsync();
        }

        #endregion
    }
}
