using MapNotepad.Model;
using MapNotepad.Services.PinService;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace MapNotepad.ViewModel
{
    class PinsListViewModel : BaseViewModel
    {
        // TODO: Пользователь нажимает на вкладу Список пинов и видит список пинов, название,
        // описание и локацию.По тапу на пин пользователь переходит на карту с фокусировкой на выбранном пине.
        

        private readonly IPinService _pinservice;
        public PinsListViewModel(INavigationService navigationService, IPinService pinService):base(navigationService)
        {
            _pinservice = pinService;
            InitAsync();

        }
        #region ---Command---
        public ICommand OnAddButtonTap => new Command(GoToAddPinPage);
        #endregion
        #region ---Public Properties---
        private ObservableCollection<PinModel> _pinList;
        public ObservableCollection<PinModel> PinList
        {
            get => _pinList;
            set => SetProperty(ref _pinList, value);
        }
        #endregion
        #region ---Privat Helpers---
        private async void InitAsync()
        {
            var pl = new ObservableCollection<PinModel>();
            var l = await _pinservice.GetPins();
            foreach (var v in l)
            {
                pl.Add(v);
            }
              PinList = pl;
        }
        #endregion
    }
}
