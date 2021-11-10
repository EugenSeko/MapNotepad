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
        private readonly IPinService _pinservice;
        private readonly ISearchServise _searchservise;
        public MapPageViewModel(INavigationService navigationService, 
                                PinService pinService, 
                                ISearchServise searchServise):base(navigationService)
        {
            _pinservice = pinService;
            _searchservise = searchServise;

            PinClickCommand = new Command(OnPinClick);
            InitAsync();
        }

        public void OnPinClick(object obj)
        {
            var b = Pin;
            if (b != null)
            {
                Console.WriteLine(b.Pin.Label);
                Console.WriteLine(b.Pin.Position.Latitude);
                Console.WriteLine(b.Pin.Position.Longitude);
            } // TODO: сделать плашку с инфой о пине
            
        }


        #region ---Command---
       // public ICommand OnButtonLeftCommand => new Command(GoToMainPageListPage);
        public ICommand PinClickCommand { get; set; }
        #endregion

        #region ---Public Properties---
        //private ObservableCollection<PinModel> _pinList;
        //public ObservableCollection<PinModel> PinList
        //{
        //    get => _pinList;
        //    set => SetProperty(ref _pinList, value);
        //}
        private List<PinModel> _pinList;
        public List<PinModel> PinList
        {
            get => _pinList;
            set => SetProperty(ref _pinList, value);
        }

        private PinClickedEventArgs _pin;
        public PinClickedEventArgs Pin
        {
            get => _pin;
            set => SetProperty(ref _pin, value);
        }
        private string _searchentry;
        public string SearchEntry
        {
            get => _searchentry;
            set => SetProperty(ref _searchentry, value);
        }
        //private double _longitude;
        //public double Longitude
        //{
        //    get => _longitude;
        //    set => SetProperty(ref _longitude, value);
        //}
        //private double _latitude;
        //public double Latitude
        //{
        //    get => _latitude;
        //    set => SetProperty(ref _latitude, value);
        //}
        #endregion

        #region ---Overrides ---
        protected override void OnPropertyChanged(PropertyChangedEventArgs args)
        {
            base.OnPropertyChanged(args);
            if(args.PropertyName == nameof(SearchEntry))
            {
                // TODO: передать значение поиска в поисковой сервис.. Поиск осуществляется через поля – название, координаты, ключевые слова в описании)
                // Пользователь должен получить уведомление если по его запросу ничего не найдено.
               PinList = _searchservise.Search(SearchEntry,_list);
            }
        }

        #endregion

        #region ---Private Helpers---
        private List<PinModel> _list;
        private async void InitAsync()
        {
            //var oc = new ObservableCollection<PinModel>();
            //var pl = await _pinservice.GetPins();
            //foreach(PinModel pm in pl)
            //{
            //    oc.Add(pm);
            //}
            //PinList = oc;

            PinList = await _pinservice.GetPins();
            _list = PinList;
        }

        #endregion
    }
}
