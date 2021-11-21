using MapNotepad.Services.PinService;
using Prism.Navigation;
using System;
using MapNotepad.Helpers;
using System.Threading.Tasks;
using System.Windows.Input;
using MapNotepad.Model;
using Xamarin.Forms;
using System.ComponentModel;

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
                NavigationParameter = null;
            }
            else
            {
               // Pin = new PinModel();
            }
        }
        private async Task SavePin()
        {
            if (isEdit)
            {
                await _pinservice.UpdatePinAsync(new PinModel()
                {
                    Label = Pin.Label,
                    Description = Pin.Description,
                    Longitude = Pin.Longitude,
                    Latitude = Pin.Latitude,
                    Id = Pin.Id,
                    IsFavorite = Pin.IsFavorite,
                    UserId = Pin.UserId,
                    Address = Pin.Address
                });
            }
            else
            {
                Pin.IsFavorite = true;
                await _pinservice.AddPinAsync(Pin);
            }
        }
        private void OnMapLongClick(PinModel pin)
        {
            Pin.Longitude = pin.Longitude;
            Pin.Latitude = pin.Latitude;
        }
        private void OnGoToPinLocation()
        {
            if(Pin == null && !isEdit)
            {
                Pin = new PinModel();
            }
            if (Pin?.Latitude != 0 && Pin?.Longitude != 0)
            {
                IsFocus = IsFocus == false;
            }
        }
        private async Task GoToMainPageListPageCommAsync()
        {
            NavigationParameter = null;
            Pin = null;
            await GoToMainPageListPageAsync();
        }
        #endregion
    }
}
