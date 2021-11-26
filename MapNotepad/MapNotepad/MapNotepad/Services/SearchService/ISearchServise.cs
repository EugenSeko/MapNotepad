using MapNotepad.Model;
using MapNotepad.ViewModel;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace MapNotepad.Services.SearchService
{
    public interface ISearchServise
    {
        List<PinModel> Search(string search_query, IEnumerable<PinModel> list);
        ObservableCollection<PinViewModel> Search(string search_query, ObservableCollection<PinViewModel> list);
    }
}
