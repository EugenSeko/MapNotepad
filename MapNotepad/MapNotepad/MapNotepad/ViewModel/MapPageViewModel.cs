using MapNotepad.Helpers;
using MapNotepad.Model;
using MapNotepad.Services.Authentification;
using MapNotepad.Services.PinService;
using MapNotepad.Services.SearchService;
using Prism;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.GoogleMaps;

namespace MapNotepad.ViewModel
{
    class MapPageViewModel:BaseViewModel, IActiveAware
    {
        private readonly IPinService _pinService;
        private readonly ISearchServise _searchService;
        private readonly IAuthentificationService _authentifService;
        public MapPageViewModel(INavigationService navigationService, 
                                PinService pinService, 
                                ISearchServise searchServise, 
                                IAuthentificationService authentificationService):base(navigationService)
        {
            _authentifService = authentificationService;
            _pinService = pinService;
            _searchService = searchServise;
            InitAsync();
        }
        #region ---Public Properties---
        public ICommand PinClickCommand { get; set; }
        public ICommand ClearClickCommand { get; set; }
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

        private ObservableCollection<PinViewModel> _obserPinList;
        public ObservableCollection<PinViewModel> ObserPinList
        {
            get => _obserPinList;
            set => SetProperty(ref _obserPinList, value);
        }
        public bool ShowCurrentPinDescription => CurrentPin != null;

        private PinModel _currentPin;
        public PinModel CurrentPin
        {
            get => _currentPin;
            set 
            { 
                SetProperty(ref _currentPin, value);
                RaisePropertyChanged(nameof(ShowCurrentPinDescription));
            }
        }
        private PinModel _pin;
        public PinModel Pin
        {
            get => _pin;
            set => SetProperty(ref _pin, value);
        }

        private bool _isFocus;
        public bool IsFocus
        {
            get => _isFocus;
            set => SetProperty(ref _isFocus, value);
        }

        private List<PinModel> _pinList;
        public List<PinModel> PinList
        {
            get => _pinList;
            set => SetProperty(ref _pinList, value);
        }

        public bool ShowCurrentPinList => SearchEntry != null && SearchEntry !="" && ObserPinList !=null;

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
        #region ---Overrides ---
        protected override void OnPropertyChanged(PropertyChangedEventArgs args)
        {
            base.OnPropertyChanged(args);
            if(args.PropertyName == nameof(SearchEntry))
            {
                // TODO: передать значение поиска в поисковой сервис.. Поиск осуществляется через поля –
                // название, координаты, ключевые слова в описании)
                // Пользователь должен получить уведомление если по его запросу ничего не найдено.
               CurrentPin = null;
               ObserPinList = null;

               var serchRes = _searchService.Search(SearchEntry,_constPinList);
                if (serchRes.Count > 0)
                {
                    PinList = serchRes;
                    var oc = new ObservableCollection<PinViewModel>();

                    foreach (PinModel pm in PinList)
                    {
                        var pvm = Extensions.PinExtension.ToPinViewModel(pm);
                        pvm.MoveToPinLocationCommand = SingleExecutionCommand.FromFunc(GoToPinLocation);
                        oc.Add(pvm); 
                    }
                    ObserPinList = oc;
                }
                if (!ShowCurrentPinList) 
                {
                    PinList = _constPinList;
                } 
            }
            if(args.PropertyName == nameof(IsActive))
            {
                if (IsActive && NavigationParameter == null )
                {
                    InitAsync();
                }
            }
        }

        #endregion
        #region ---Private Helpers---
        private List<PinModel> _constPinList;
        private async void InitAsync()
        {
            PinClickCommand = new Command<PinModel>(OnPinClick);
            ClearClickCommand = new Command(OnClearClick);
            GoToSettingsPageCommand = SingleExecutionCommand.FromFunc(GoToAddPinPageAsync);
            LogoutCommand = SingleExecutionCommand.FromFunc(OnLogoutCommand);

            var pinList = new List<PinModel>();
            var allPinList = await _pinService.GetPinsAsync();
            var favor_list = allPinList.Where(x => x.IsFavorite == true);
            foreach(var p in favor_list)
            {
                pinList.Add(p);
            }
            PinList = pinList;
            _constPinList = PinList;

            if (NavigationParameter?.GetType() == typeof(PinModel))
            {
              await  GoToPinLocation(NavigationParameter);
            }
        }

        private Task GoToPinLocation(object obj)
        { 
            if(obj.GetType() == typeof(PinModel))
            {
                Pin = obj as PinModel;
            }
            else if(obj.GetType() == typeof(PinViewModel))
            {
                Pin = Extensions.PinExtension.ToPinModel(obj as PinViewModel);
            }
            IsFocus = IsFocus == false;
            NavigationParameter = null;
            IsFocus = IsFocus == false;

            return Task.CompletedTask;
        }

        private void OnClearClick(object obj)
        {
            CurrentPin = null;
        }
        private void OnPinClick(PinModel pinModel)
        {
            if (pinModel != null)
            {
                CurrentPin = pinModel;
            }

        }
        private Task OnLogoutCommand()
        {
            _authentifService.Logout();
            return Task.CompletedTask;
        }
        #endregion
    }
}
