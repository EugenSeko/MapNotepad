using MapNotepad.Model;
using MapNotepad.ViewModel;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MapNotepad.Services.PinService
{
    public interface IPinService
    {
        Task AddPin(string label,string description, double longitude, double latitude,bool isfavorite);
        Task<List<PinModel>> GetPinsAsync();
    }
}
