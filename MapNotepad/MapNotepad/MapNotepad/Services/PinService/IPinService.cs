using MapNotepad.Model;
using MapNotepad.ViewModel;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MapNotepad.Services.PinService
{
    public interface IPinService
    {
        Task AddPinAsync(PinModel pin);
        Task<List<PinModel>> GetPinsAsync();
        Task<bool> DeletePinAsync(PinModel pin);
        Task UpdatePinAsync(PinModel pin);
    }
}
