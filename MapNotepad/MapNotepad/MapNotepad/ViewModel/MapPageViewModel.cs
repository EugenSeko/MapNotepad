using MapNotepad.Model;
using MapNotepad.Services.PinService;
using MapNotepad.Services.SearchService;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.GoogleMaps;

namespace MapNotepad.ViewModel
{
    class MapPageViewModel:BaseViewModel
    {
        private readonly IPinService _pinService;
        private readonly ISearchServise _searchService;
        public MapPageViewModel(INavigationService navigationService, 
                                PinService pinService, 
                                ISearchServise searchServise):base(navigationService)
        {
            _pinService = pinService;
            _searchService = searchServise;

            PinClickCommand = new Command<PinModel>(OnPinClick);
            ClearClickCommand = new Command(OnClearClick);

            InitAsync();
        }

        #region ---Public Properties---
        public ICommand PinClickCommand { get; set; }
        public ICommand ClearClickCommand { get; set; }

        private ObservableCollection<PinModel> _obserPinList;
        public ObservableCollection<PinModel> ObserPinList
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

               PinList = _searchService.Search(SearchEntry,_constPinList);

               var oc = new ObservableCollection<PinModel>();

                foreach (PinModel pm in PinList)
                {
                    oc.Add(pm);
                }

                ObserPinList = oc;

                if (!ShowCurrentPinList) PinList = _constPinList;
            }
        }

        #endregion

        #region ---Private Helpers---
        private List<PinModel> _constPinList;
        private async void InitAsync()
        {
            PinList = await _pinService.GetPinsAsync();
            _constPinList = PinList;
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

        #endregion
    }
}
