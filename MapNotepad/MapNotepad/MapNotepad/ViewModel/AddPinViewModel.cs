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

namespace MapNotepad.ViewModel
{
    class AddPinViewModel : BaseViewModel
    {
        private readonly IPinService _pinservice;
        public AddPinViewModel(INavigationService navigationService, IPinService pinService ):base(navigationService)
        {
            _pinservice = pinService;
            Init();
            MapLongClickedCommand = new Command<PinModel>(OnMapLongClick);
        }

       

        #region ---Public Properties---
        public ICommand OnButtonLeftCommand => SingleExecutionCommand.FromFunc(GoToMainPageListPageAsync);
        public ICommand OnButtonSaveCommand => SingleExecutionCommand.FromFunc(SavePin);
        public ICommand MapLongClickedCommand { get; set; }
        private PinModel _pin;
        public PinModel Pin 
        {
            get => _pin;
            set => SetProperty(ref _pin, value);
        }
        private Pin _focus;
        public Pin Focus
        {
            get => _focus;
            set => SetProperty(ref _focus, value);
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
            if((args.PropertyName == nameof(Latitude) && Longitude != 0 && !isMapLongClicked) || (args.PropertyName == nameof(Longitude) && Latitude != 0 && !isMapLongClicked))
            {
                this.Pin.Latitude = Latitude;
                this.Pin.Longitude = Longitude;
                Focus = null;
                Focus = new Pin() 
                { 
                    Label = "" 
                };
                
            }
        }
        #endregion

        #region ---Private Helpers---
        private bool isMapLongClicked = false;
        private void Init()
        {
            if (NavigationParameter != null)
            {
                HeaderText = "Edit Pin";
                var pinViewModel = NavigationParameter as PinViewModel;
                Label = pinViewModel.Label;
                Description = pinViewModel.Description;
                Longitude = pinViewModel.Longitude;
                Latitude = pinViewModel.Latitude;
            }
        }
        private async Task SavePin()
        {
            if (NavigationParameter != null)
            {
                var pinViewModel = NavigationParameter as PinViewModel;
                NavigationParameter = null;

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
                Pin.Description = Description;

                await _pinservice.AddPinAsync(Pin);
            }
        }
        private void OnMapLongClick(PinModel pin)
        {
            isMapLongClicked = true;

            Longitude = pin.Longitude;
            Latitude = pin.Latitude;

            isMapLongClicked = false;
        }
        #endregion
    }
}
