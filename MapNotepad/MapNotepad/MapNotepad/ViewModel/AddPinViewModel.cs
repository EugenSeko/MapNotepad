using MapNotepad.Services.PinService;
using Prism.Navigation;
using System;
using System.ComponentModel;
using MapNotepad.Helpers;
using System.Threading.Tasks;
using System.Windows.Input;
using MapNotepad.Model;
using Xamarin.Forms;
using Xamarin.Forms.GoogleMaps;
using System.Collections.Generic;

namespace MapNotepad.ViewModel
{
    class AddPinViewModel : BaseViewModel
    {
        private readonly IPinService _pinservice;
        public AddPinViewModel(INavigationService navigationService, IPinService pinService ):base(navigationService)
        {
            _pinservice = pinService;
            Init();
        }

        private bool isPositionChangeMuted = false;
        private bool isEdit;

        #region ---Public Properties---
        public ICommand OnButtonLeftCommand => SingleExecutionCommand.FromFunc(GoToMainPageListPageAsync);
        public ICommand OnButtonSaveCommand => SingleExecutionCommand.FromFunc(SavePin);
        public ICommand MapLongClickedCommand => new Command<PinModel>(OnMapLongClick);
        private List<PinModel> _pinList;
        public List<PinModel> PinList
        {
            get => _pinList;
            set => SetProperty(ref _pinList, value);
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
        protected override void OnPropertyChanged(PropertyChangedEventArgs args)
        {
            base.OnPropertyChanged(args);
            if((args.PropertyName == nameof(Latitude) && Longitude != 0 && !isPositionChangeMuted) || (args.PropertyName == nameof(Longitude) && Latitude != 0 && !isPositionChangeMuted))
            {
                if(Pin == null)
                {
                    Pin = new PinModel();
                }
                Pin.Latitude = Latitude;
                Pin.Longitude = Longitude;
                IsFocus = IsFocus == false;
            }
        }
        #endregion
        #region ---Private Helpers---
        private void Init()
        {
            if (NavigationParameter != null)
            {
                isEdit = true;
                HeaderText = "Edit Pin"; 
                PinViewModel pinViewModel = NavigationParameter as PinViewModel;
                Label = pinViewModel.Label;
                Description = pinViewModel.Description;

                isPositionChangeMuted = true;
                Longitude = pinViewModel.Longitude;
                Latitude = pinViewModel.Latitude;
                isPositionChangeMuted = false;

                PinList = new List<PinModel>() {Extensions.PinExtension.ToPinModel(pinViewModel)};
                NavigationParameter = null;
            }
        }
        private async Task SavePin()
        {
            if (isEdit)
            {
                var pinViewModel = Extensions.PinExtension.ToPinViewModel(PinList[0]);

                await _pinservice.UpdatePinAsync(new PinModel()
                {
                    Label = Label,
                    Description = Description,
                    Longitude = Longitude,
                    Latitude = Latitude,
                    Id = pinViewModel.Id,
                    IsFavorite = pinViewModel.IsFavorite,
                    UserId = pinViewModel.UserId,
                    Address = pinViewModel.Address
                });
            }
            else
            {
                Pin.Label = Label; 
                Pin.IsFavorite = true;
                Pin.Description = Description;
                Pin.Longitude = Longitude;
                Pin.Latitude = Latitude;

                await _pinservice.AddPinAsync(Pin);
                Pin = null;
            }
        }
        private void OnMapLongClick(PinModel pin)
        {
            isPositionChangeMuted = true;

            Longitude = pin.Longitude;
            Latitude = pin.Latitude;

            isPositionChangeMuted = false;
        }
        #endregion
    }
}
