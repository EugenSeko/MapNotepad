using MapNotepad.Model;
using MapNotepad.Services.PinService;
using MapNotepad.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace MapNotepad.Services.SearchService
{
    public class SearchService : ISearchServise
    {
        private readonly IPinService _pinservice;
        public SearchService(IPinService pinService)
        {
            _pinservice = pinService;
        }
        #region --- Interface Implementation ---
        public ObservableCollection<PinViewModel> Search(string search_query, ObservableCollection<PinViewModel> list)
        {
            List<PinModel> pmList = new List<PinModel>();
            foreach (PinViewModel pvm in list)
            {
                pmList.Add(Extensions.PinExtension.ToPinModel(pvm));
            }
            pmList = Search(search_query, pmList);
            ObservableCollection<PinViewModel> outRes = new ObservableCollection<PinViewModel>();
            foreach (var pvm in list)
            {
                foreach (var pm in pmList)
                {
                    if (pm.Id == pvm.Id)
                    {
                        outRes.Add(pvm);
                        break;
                    }
                }
            }
            return outRes;
        }
        public List<PinModel> Search(string search_query, IEnumerable<PinModel> list)
        {
            List<PinModel> outValue = new List<PinModel>();
            
                var doubleResult = double.TryParse(search_query, out double doubleValue);

           if (doubleResult)
           { 
              var searchResult =  DoubleSearch(doubleValue,list);
               foreach(PinModel pinModel in searchResult)
               {
                   outValue.Add(pinModel);
               }
           }
           else
           {  // NOTE: переписать на String.Contain
               var searchResult = list.Where(x => x.Label == search_query);
                foreach (PinModel pinModel in searchResult)
                {
                   outValue.Add(pinModel);
                }
              if (outValue.Count==0)
              {
                    foreach(var pinModel in list)
                    {
                      var keywords = pinModel.Description?.Split(new Char[] { ' ', ',', '.', ':', ';', '!', '?', '\t' });
                        if (keywords != null)
                        {
                            foreach (string keyword in keywords)
                            {
                                if (keyword == search_query)
                                {
                                    outValue.Add(pinModel);
                                    break;
                                }

                            }
                        }
                      
                    }
              }
           }
            return outValue;
        }
        #endregion
        #region --- Private Helpers---
        private IEnumerable<PinModel> DoubleSearch(double coordinate,IEnumerable<PinModel> list)
        {
            double delta = 0.1; // NOTE: точность может быть измененена или изменяться во времени.

            return list.Where(x => Math.Abs(x.Latitude - coordinate) <= delta || Math.Abs(x.Longitude - coordinate) <= delta);
        }
        #endregion
    }
}
