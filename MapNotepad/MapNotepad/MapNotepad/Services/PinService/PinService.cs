using MapNotepad.Extensions;
using MapNotepad.Model;
using MapNotepad.Services.Repository;
using MapNotepad.Services.Settings;
using MapNotepad.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

        public async Task AddPinAsync(PinModel pin)
        {
           await _repository.InsertAsync(pin);
        }

        public async Task UpdatePinAsync(PinModel pin)
        {
            await _repository.UpdateAsync(pin);
        } 

        public async Task<bool> DeletePinAsync(PinModel pin)
        {
           await _repository.DeleteAsync(pin);
            return true;
        }

        public async Task<List<PinModel>> GetPinsAsync()
        {
            var pinlist = await _repository.GetAllAsync<PinModel>();
            return pinlist;
        }
        
    }
}
