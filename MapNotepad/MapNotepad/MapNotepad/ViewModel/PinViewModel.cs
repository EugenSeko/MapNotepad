using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms.GoogleMaps;

namespace MapNotepad.ViewModel
{
    public class PinViewModel : BindableBase
    {

        #region --- Public Properties ---
        private ICommand _moveToPinLocationCommand;
        public ICommand MoveToPinLocationCommand
        {
            get => _moveToPinLocationCommand;
            set => SetProperty(ref _moveToPinLocationCommand, value);
        }
        private ICommand _deleteCommand;
        public ICommand DeleteCommand
        {
            get => _deleteCommand;
            set => SetProperty(ref _deleteCommand, value);
        }
        private ICommand _editCommand;
        public ICommand EditCommand
        {
            get => _editCommand;
            set => SetProperty(ref _editCommand, value);
        }
        private ICommand _favoritChangeCommand;
        public ICommand FavoritChangeCommand
        {
            get => _favoritChangeCommand;
            set => SetProperty(ref _favoritChangeCommand, value);
        }
        private int _id;
        public int Id 
        {   
            get => _id;
            set => SetProperty(ref _id, value);
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
        private string _label;
        public string Label 
        {
            get => _label;
            set => SetProperty(ref _label, value);
        }
        private string _address;
        public string Address 
        {
            get => _address;
            set => SetProperty(ref _address, value);
        }
        private string _description;
        public string Description 
        {
            get => _description;
            set => SetProperty(ref _description, value);
        }
        private bool _isFavorite;
        public bool IsFavorite 
        {
            get => _isFavorite;
            set => SetProperty(ref _isFavorite, value);
        }
        private string _userId;
        public string UserId 
        {
            get => _userId;
            set => SetProperty(ref _userId, value);
        }
        #endregion
        #region --- Overrides ---
        #endregion
        #region --- Private Helpers ---
        #endregion
    }
}
