

using MapNotepad.Model;
using MapNotepad.ViewModel;

namespace MapNotepad.Extensions
{
    public static class PinExtension
    {
        public static PinModel ToPinModel(this PinViewModel pinViewModel)
        {
            PinModel pinModel = new PinModel 
            {
                Address = pinViewModel.Address,
                Description = pinViewModel.Description,
                Id = pinViewModel.Id,
                IsFavorite = pinViewModel.IsFavorite,
                Label = pinViewModel.Label,
                Latitude = pinViewModel.Latitude,
                Longitude = pinViewModel.Longitude,
                UserId = pinViewModel.UserId
                
            };
            return pinModel;
        }
        public static PinViewModel ToPinViewModel(this PinModel pinModel)
        {
            PinViewModel pinViewModel = new PinViewModel
            {
                UserId = pinModel.UserId,
                Longitude = pinModel.Longitude,
                Latitude = pinModel.Latitude,
                Label = pinModel.Label,
                IsFavorite = pinModel.IsFavorite,
                Id = pinModel.Id,
                Description = pinModel.Description,
                Address = pinModel.Address,
                        
            };
            return pinViewModel;
        }
    }
}
