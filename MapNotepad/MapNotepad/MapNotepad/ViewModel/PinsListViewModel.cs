using MapNotepad.Extensions;
using MapNotepad.Model;
using MapNotepad.Services.PinService;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using MapNotepad.Helpers;
using MapNotepad.Services.Authentification;
using Prism;
using System.ComponentModel;
using MapNotepad.Services.SearchService;
using Acr.UserDialogs;

namespace MapNotepad.ViewModel
{
    class PinsListViewModel : BaseViewModel, IActiveAware
    {
        private readonly IAuthentificationService _authentifService;
        private readonly IPinService _pinservice;
        private readonly ISearchServise _searchService;
        private int _initCounter;

        public PinsListViewModel(INavigationService navigationService, 
            IPinService pinService, 
            IAuthentificationService authentificationService, 
            ISearchServise searchServise) :base(navigationService)
        {
            _pinservice = pinService;
            _searchService = searchServise;
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
        public bool ShowCurrentPinList => SearchEntry != null && SearchEntry != "" && PinList != null;
        private string _searchEntry;
        public string SearchEntry
        {
            get => _searchEntry;
            set
            {
                SetProperty(ref _searchEntry, value);
                RaisePropertyChanged(nameof(ShowCurrentPinList));
            }
        }
        #endregion
        #region --- Overrides ---
        protected override void OnPropertyChanged(PropertyChangedEventArgs args)
        {
            base.OnPropertyChanged(args);
            if (args.PropertyName == nameof(SearchEntry))
            {
                // TODO: 
                // Пользователь должен получить уведомление если по его запросу ничего не найдено.
                var serchRes = _searchService.Search(SearchEntry, PinList);
                if (serchRes.Count > 0)
                {
                    PinList = serchRes;
                }
                if (!ShowCurrentPinList)
                {
                    _initCounter = 0;
                    InitAsync();
                }
            }
            if (args.PropertyName == nameof(IsActive))
            {
                if (IsActive && NavigationParameter == null)
                {
                    InitAsync();
                }
                NavigationParameter = null;
            }
        }
        #endregion
        #region ---Privat Helpers---
        private List<PinModel> _constPinList;
        private async void InitAsync()
        {
            if(_initCounter != 1)
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
            _initCounter++;
        }
        private async Task OnFavoritChangeCommand(object obj)
        {
            var pinModel = PinExtension.ToPinModel(obj as PinViewModel);
            pinModel.IsFavorite = pinModel.IsFavorite == false;
            await _pinservice.UpdatePinAsync(pinModel);
            var pinvm = obj as PinViewModel;
            var index = PinList.IndexOf(pinvm);
            pinvm.IsFavorite = pinModel.IsFavorite;
            if (index == 0)
            {
                PinList.Insert(index +1, pinvm);
            }
            else
            {
                PinList.Insert(index, pinvm);
            }
            PinList.RemoveAt(index);
        }
        private async Task OnLogoutCommand()
        {
            var confirmConfig = new ConfirmConfig()
            {
                Message = "Do you want to logout?",
                OkText = "Ok",
                CancelText = "Cancel"
            };
            var confirm = await UserDialogs.Instance.ConfirmAsync(confirmConfig);
            if (confirm)
            {
              await _authentifService.Logout();
            }
        }
        private async Task GoToPinLocation(object obj)
        {
           // NavigationParameter ??= PinExtension.ToPinModel(obj as PinViewModel);
            Global.NavigationParameter ??= PinExtension.ToPinModel(obj as PinViewModel);
            await GoToMainPagePageAsync();
        }
        private async Task OnDeleteButtonCommand(object obj)
        {
            var confirmConfig = new ConfirmConfig()
            {
                Message = "Do you want to delete this pin?",
                OkText = "Ok",
                CancelText = "Cancel"
            };
            var confirm = await UserDialogs.Instance.ConfirmAsync(confirmConfig);
            if (confirm)
            {
                PinList.Remove(obj as PinViewModel);
                await _pinservice.DeletePinAsync(PinExtension.ToPinModel(obj as PinViewModel));
            }
        }
        private async Task OnEditButtonCommand(object obj)
        {
            NavigationParameter = obj;
            await GoToAddPinPageAsync();
        }
        #endregion
    }
}
