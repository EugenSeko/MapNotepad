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

        private void OnClearClick(object obj)
        {
            CurrentPin = null;
        }

        public void OnPinClick(PinModel pinModel)
        {
            if (pinModel != null)
            {
                CurrentPin = pinModel;
            } 
            
        }


        #region ---Command---
        public ICommand PinClickCommand { get; set; }
        public ICommand ClearClickCommand { get; set; }
        #endregion

        #region ---Public Properties---
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

        public bool ShowCurrentPinList => SearchEntry != null && SearchEntry !="";

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
                // TODO: передать значение поиска в поисковой сервис.. Поиск осуществляется через поля – название, координаты, ключевые слова в описании)
                // Пользователь должен получить уведомление если по его запросу ничего не найдено.
               PinList = _searchService.Search(SearchEntry,_list);
            }
        }

        #endregion

        #region ---Private Helpers---
        private List<PinModel> _list;
        private async void InitAsync()
        {
            var oc = new ObservableCollection<PinModel>();
            PinList = await _pinService.GetPins();
            foreach(var pm in PinList)
            {
                oc.Add(pm);
            }
            ObserPinList = oc;
            _list = PinList;
        }

        #endregion
    }
}
