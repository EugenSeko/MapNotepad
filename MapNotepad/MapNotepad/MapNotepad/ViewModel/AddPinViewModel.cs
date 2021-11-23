using MapNotepad.Services.PinService;
using Prism.Navigation;
using System;
using MapNotepad.Helpers;
using System.Threading.Tasks;
using System.Windows.Input;
using MapNotepad.Model;
using Xamarin.Forms;
using System.ComponentModel;
using MapNotepad.Services.Settings;

namespace MapNotepad.ViewModel
{
    class AddPinViewModel : BaseViewModel
    {
        private readonly IPinService _pinservice;
        private bool isEdit;
        public AddPinViewModel(INavigationService navigationService, IPinService pinService ):base(navigationService)
        {
            _pinservice = pinService;
            Init();
        }
        #region ---Public Properties---
        public ICommand OnButtonLeftCommand => SingleExecutionCommand.FromFunc(GoToMainPageListPageCommAsync);
        public ICommand OnButtonSaveCommand => SingleExecutionCommand.FromFunc(SavePin);
        public ICommand MapLongClickedCommand => new Command<PinModel>(OnMapLongClick);
        public ICommand UnFocusedCommand => new Command(OnGoToPinLocation);
        public ICommand FocusedCommand => new Command(OnGoToPinLocation);

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

        private string _headerText="Add Pin";
        public string HeaderText
        {
            get => _headerText;
            set => SetProperty(ref _headerText, value);
        }
        private string _longitude;
        public string Longitude
        {
            get => _longitude;
            set => SetProperty(ref _longitude, value);
        }
        private string _latitude;
        public string Latitude
        {
            get => _latitude;
            set => SetProperty(ref _latitude, value);
        }
        private string _description;
        public string Description
        {
            get => _description;
            set => SetProperty(ref _description, value);
        }
        private string _label;
        public string Label
        {
            get => _label;
            set => SetProperty(ref _label, value);
        }
        #endregion
        #region ---Overrides ---
        protected override void OnPropertyChanged(PropertyChangedEventArgs args)
        {
            base.OnPropertyChanged(args);
        }
            #endregion
            #region ---Private Helpers---
            private void Init()
        {
            if (NavigationParameter != null)
            {
                isEdit = true;
                HeaderText = "Edit Pin"; 
                Pin = Extensions.PinExtension.ToPinModel(NavigationParameter as PinViewModel);
                Latitude = Pin.Latitude.ToString();
                Longitude = Pin.Longitude.ToString();
                Label = Pin.Label;
                Description = Pin.Description;
                NavigationParameter = null;
            }
            else
            {
                Pin = new PinModel();
            }
        }
        private async Task SavePin()
        {
            if (isEdit)
            {
                await _pinservice.UpdatePinAsync(new PinModel()
                {
                    Label = Label??"", //TODO userdialog
                    Description = Description,
                    Longitude = StringToDouble(Longitude),
                    Latitude = StringToDouble(Latitude),
                    Id = Pin.Id,
                    IsFavorite = Pin.IsFavorite,
                    UserId = Pin.UserId,
                    Address = Pin.Address
                });
            }
            else
            {
                Pin.IsFavorite = true;
                await _pinservice.AddPinAsync(new PinModel()
                {
                    Label = Label??"", //TODO userdialog
                    Description = Description,
                    Longitude = StringToDouble(Longitude),
                    Latitude = StringToDouble(Latitude),
                    IsFavorite = true,
                    Address = Pin.Address
                });
            }
        }
        private void OnMapLongClick(PinModel pin)
        {
            Longitude = pin.Longitude.ToString();
            Latitude = pin.Latitude.ToString();
            Pin.Longitude = pin.Longitude;
            Pin.Latitude = pin.Latitude;
            Pin.Address = pin.Address;

            IsFocus = IsFocus == false;
        }
        private void OnGoToPinLocation()
        {
            if (StringToDouble(Latitude) != 0 && StringToDouble(Longitude) != 0)
            {
                Pin.Longitude = StringToDouble(Longitude);
                Pin.Latitude = StringToDouble( Latitude);
                IsFocus = IsFocus == false;
            }
        }
        private async Task GoToMainPageListPageCommAsync()
        {
            NavigationParameter = null;
            Pin = null;
            await GoToMainPageListPageAsync();
        }
        private double StringToDouble(string s)
        {
           if( double.TryParse(s, out double res))
            {
                return res;
            }
            else
            {
              return 0;
            }
            
        }
        #endregion
    }
}
