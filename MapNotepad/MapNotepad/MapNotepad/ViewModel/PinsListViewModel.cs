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
using Prism;
using System.ComponentModel;

namespace MapNotepad.ViewModel
{
    class PinsListViewModel : BaseViewModel, IActiveAware
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

        public event EventHandler IsActiveChanged;

        private bool _isActive;
        public bool IsActive
        {
            get { return _isActive; }
            set { SetProperty(ref _isActive, value, RaiseIsActiveChanged); }
        }
        protected virtual void RaiseIsActiveChanged()
        {
            IsActiveChanged?.Invoke(this, EventArgs.Empty);
        }

        private PinViewModel _isFavorit;
        public PinViewModel IsFavorit
        {
            get => _isFavorit;
            set => SetProperty(ref _isFavorit, value);
        }
       
        private ObservableCollection<PinViewModel> _pinList;
        public ObservableCollection<PinViewModel> PinList
        {
            get => _pinList;
            set => SetProperty(ref _pinList, value);
        }
        #endregion
        #region --- Overrides ---
        protected override void OnPropertyChanged(PropertyChangedEventArgs args)
        {
            base.OnPropertyChanged(args);
            if (args.PropertyName == nameof(IsActive))
            {
                if (IsActive )
                {
                    InitAsync();
                }
            }
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
                pvm.FavoritChangeCommand = SingleExecutionCommand.FromFunc(OnFavoritChangeCommand);
                pvm.MoveToPinLocationCommand = SingleExecutionCommand.FromFunc(GoToPinLocation);
                pl.Add(pvm);
            }
            PinList = pl;
        }

        private async Task OnFavoritChangeCommand(object obj)
        {
            var pinModel = PinExtension.ToPinModel(obj as PinViewModel);
            pinModel.IsFavorite = pinModel.IsFavorite == false;
            await _pinservice.UpdatePinAsync(pinModel);
            InitAsync(); // how faster??
            //var pinVM = obj as PinViewModel;
            //PinList.Remove(pinVM);
            //pinVM.IsFavorite = pinModel.IsFavorite;
            //PinList.Add(pinVM);
        }

        private Task OnLogoutCommand()
        {
            _authentifService.Logout();
            return Task.CompletedTask;
        }
        private async Task GoToPinLocation(object obj)
        {
            NavigationParameter ??= PinExtension.ToPinModel(obj as PinViewModel);
            await GoToMainPagePageAsync();
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
