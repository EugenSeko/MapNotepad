using MapNotepad.Model;
using MapNotepad.Services.PinService;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.GoogleMaps;

namespace MapNotepad.ViewModel
{
    class MapPageViewModel:BaseViewModel
    {
        private readonly IPinService _pinservice;
        public MapPageViewModel(INavigationService navigationService, PinService pinService):base(navigationService)
        {
            _pinservice = pinService;
            PinClickCommand = new Command(OnPinClick);
            InitAsync();
        }

        public void OnPinClick(object obj)
        {
            Console.WriteLine("ggggggggggggggggggggg");
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

        //private String _label;
        //public string Label
        //{
        //    get => _label;
        //    set => SetProperty(ref _label, value);
        //}
        //private String _description;
        //public string Description
        //{
        //    get => _description;
        //    set => SetProperty(ref _description, value);
        //}
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

        #endregion

        #region ---Private Helpers---
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
        }

        #endregion
    }
}
