using MapNotepad.Services.PinService;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.GoogleMaps;

namespace MapNotepad.ViewModel
{
    class AddPinViewModel : BaseViewModel
    {
        private readonly IPinService _pinservice;
        public AddPinViewModel(INavigationService navigationService, IPinService pinService ):base(navigationService)
        {
            _pinservice = pinService;
            Map = new Map();
            Map.UiSettings.MyLocationButtonEnabled = true;
            Map.MyLocationEnabled = true;
            Map.MapLongClicked += AddMapPin;
            // TODO: create init method
        }

        #region ---Command---
        public ICommand OnButtonLeftCommand => new Command(GoToMainPageListPage);
        public ICommand OnButtonSaveCommand => new Command(SavePin);
        #endregion

        #region ---Public Properties---
        public Map Map { get; private set; }
        public Pin Pin { get; private set; }

        private String _headerText="Add Pin";
        public string HeaderText
        {
            get => _headerText;
            set => SetProperty(ref _headerText, value);
        }
        private String _label;
        public string Label
        {
            get => _label;
            set => SetProperty(ref _label, value);
        }
        private String _description;
        public string Description
        {
            get => _description;
            set => SetProperty(ref _description, value);
        }
        private double _longitude;
        public double Longitude
        {
            get => _longitude;
            set => SetProperty(ref _longitude, value);
        }
        private double _latitude;
        public double Latitude
        {
            get => _latitude;
            set => SetProperty(ref _latitude, value);
        }
        #endregion

        #region ---Overrides ---
        private bool isMapLongClicked = false;
        protected override void OnPropertyChanged(PropertyChangedEventArgs args)
        {
            base.OnPropertyChanged(args);
            if (args.PropertyName == nameof(Latitude) && Longitude != 0 && !isMapLongClicked|| args.PropertyName == nameof(Longitude) && Latitude != 0&&!isMapLongClicked)
            {
                if (Pin == null)
                {
                    Pin = new Pin()
                    {
                        Type = PinType.Place,
                        Label = Label ?? "no label",
                        Position = new Position(Latitude, Longitude)
                    };
                    Map.Pins.Add(Pin);
                    Map.MoveToRegion(MapSpan.FromCenterAndRadius(Pin.Position, Distance.FromMeters(5000)));
                }
                else
                {
                    Map.Pins.Remove(Pin);
                    Pin.Type = PinType.Place;
                    Pin.Label = Label ?? "no label";
                    Pin.Position = new Position(Latitude, Longitude);
                    Map.Pins.Add(Pin);
                    Map.MoveToRegion(MapSpan.FromCenterAndRadius(Pin.Position, Distance.FromMeters(5000)));
                }
            }
        }
        #endregion

        #region ---Private Helpers---
        private async void SavePin()
        {
          await _pinservice.AddPin(Label, Description, Longitude, Latitude, true);
        }
        private void AddMapPin(object obj, MapLongClickedEventArgs e)
        {
            isMapLongClicked = true;
            //var point = (obj as MapLongClickedEventArgs).Point;
            //Latitude = point.Latitude;
            //Longitude = point.Longitude;
            Latitude = e.Point.Latitude;
            Longitude = e.Point.Longitude;

            if (Pin == null)
            {
                Pin = new Pin()
                {
                    Type = PinType.Place,
                    Label = Label ?? "no label",
                    Position = e.Point
                };
                Map.Pins.Add(Pin);
            }
            else
            {
                Map.Pins.Remove(Pin);
                Pin.Type = PinType.Place;
                Pin.Label = Label ?? "no label";
                Pin.Position = e.Point;
                Map.Pins.Add(Pin);
            }
            isMapLongClicked = false;
        }
        #endregion
    }
}
