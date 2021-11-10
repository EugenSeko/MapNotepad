using MapNotepad.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MapNotepad.Services.SearchService
{
    public interface ISearchServise
    {
        List<PinModel> Search(string search_query, IEnumerable<PinModel> list);
    }
}
