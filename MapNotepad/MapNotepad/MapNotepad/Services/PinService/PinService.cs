using MapNotepad.Model;
using MapNotepad.Services.Repository;
using MapNotepad.Services.Settings;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MapNotepad.Services.PinService
{
    public class PinService : IPinService
    {
        private readonly IRepository _repository;
        private readonly ISettingsManager _settingsManager;
        public PinService(IRepository repository, ISettingsManager settingsManager)
        {
            _repository = repository;
            _settingsManager = settingsManager;
        }

        public Task AddPin(string label, string description, double longitude, double latitude,bool isfavorite)
        {
            PinModel pm = new PinModel()
            {
                Label = label,
                Description = description,
                UserId = _settingsManager.UserId,
                IsFavorite = isfavorite,
                Latitude=latitude,
                Longitude=longitude
            };
           return _repository.InsertAsync(pm);
        }
        public async Task<List<PinModel>> GetPins()
        {
            var pinlist = await _repository.GetAllAsync<PinModel>();
            return pinlist;
        }
    }
}
