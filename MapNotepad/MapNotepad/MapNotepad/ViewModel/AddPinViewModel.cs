using MapNotepad.Services.PinService;
using Prism.Navigation;
using MapNotepad.Helpers;
using System.Threading.Tasks;
using System.Windows.Input;
using MapNotepad.Model;
using Xamarin.Forms;
using Acr.UserDialogs;

namespace MapNotepad.ViewModel
{
    class AddPinViewModel : BaseViewModel
    {
        private readonly IPinService _pinservice;
        private bool isEdit;
        public AddPinViewModel(INavigationService navigationService, 
                               IPinService pinService 
                               ):base(navigationService)
        {
            _pinservice = pinService;
            Init();
        }
        #region ---Public Properties---
        public ICommand OnButtonLeftCommand => SingleExecutionCommand.FromFunc(GoToMainPageListPageCommAsync);
        public ICommand OnButtonSaveCommand => SingleExecutionCommand.FromFunc(SavePin);
        public ICommand MapLongClickedCommand => new Command<PinModel>(OnMapLongClick);
        public ICommand UnFocusedCommand => SingleExecutionCommand.FromFunc(GoToPinLocation);
       // public ICommand FocusedCommand => SingleExecutionCommand.FromFunc(OnGoToPinLocation);

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
            if (                      Longitude == null || Latitude == null
                    || StringToDouble(Longitude) >= 180 || StringToDouble(Longitude) <= -180
                    || StringToDouble(Latitude)  >= 90  || StringToDouble(Latitude)  <= -90)
            {
                double res = StringToDouble(Longitude);
                double res2 = StringToDouble(Latitude);

                var confirmConfig = new ConfirmConfig()
                {
                    Message = "Coordinates are not specified or out of range" + "\n" + "Pin not saved",
                    OkText = "Ok",
                    CancelText = ""
                };
                var confirm = await UserDialogs.Instance.ConfirmAsync(confirmConfig);
                if (confirm)
                {
                    return;
                }
            }
            else
            {
                if (isEdit)
                {
                    await _pinservice.UpdatePinAsync(new PinModel()
                    {
                        Label = Label ?? "", //TODO userdialog
                        Description = Description,
                        Longitude = StringToDouble(Longitude),
                        Latitude = StringToDouble(Latitude),
                        Id = Pin.Id,
                        IsFavorite = Pin.IsFavorite,
                        UserId = Pin.UserId,
                        Address = Pin.Address
                    });
                    UserDialogs.Instance.ShowLoading("Saving...");
                    await Task.Delay(300);
                    UserDialogs.Instance.HideLoading();
                    await UserDialogs.Instance.AlertAsync("Pin saved");
                    await GoToMainPageListPageCommAsync();
                }
                else
                {
                    Pin.IsFavorite = true;
                    await _pinservice.AddPinAsync(new PinModel()
                    {
                        Label = Label ?? "", 
                        Description = Description,
                        Longitude = StringToDouble(Longitude),
                        Latitude = StringToDouble(Latitude),
                        IsFavorite = true,
                        Address = Pin.Address
                    });
                    UserDialogs.Instance.ShowLoading("Saving...");
                    await Task.Delay(300);
                    UserDialogs.Instance.HideLoading();
                    await UserDialogs.Instance.AlertAsync("Pin saved");
                    await GoToMainPageListPageCommAsync();
            }  }
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
        private async Task GoToPinLocation()
        {
            if (StringToDouble(Latitude) != 0 && StringToDouble(Longitude) != 0)
            {
                Pin.Longitude = StringToDouble(Longitude);
                Pin.Latitude = StringToDouble( Latitude);
                if(Pin.Latitude > 90 || Pin.Latitude < -90)
                {
                    var confirmConfig = new ConfirmConfig()
                    {
                        Message = "Latitude set incorrectly." + "\n" + " Range: (-90 .. 90)",
                        OkText = "Ok",
                        CancelText = ""
                    };
                    var confirm = await UserDialogs.Instance.ConfirmAsync(confirmConfig);
                    if (confirm)
                    {
                    }
                }
                else if(Pin.Longitude >180 || Pin.Longitude < -180)
                {
                    var confirmConfig = new ConfirmConfig()
                    {
                        Message = "Longitude set incorrectly." + "\n" + "Range: (-180 .. 180)",
                        OkText = "Ok",
                        CancelText = ""
                    };
                    var confirm = await UserDialogs.Instance.ConfirmAsync(confirmConfig);
                    if (confirm)
                    {
                    }
                }
                else
                {
                    IsFocus = IsFocus == false;
                }
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
            double outRes = 200;
            if( double.TryParse(s, out double res))
            {
                outRes = res;
            }
            return outRes;
        }
        #endregion
    }
}
