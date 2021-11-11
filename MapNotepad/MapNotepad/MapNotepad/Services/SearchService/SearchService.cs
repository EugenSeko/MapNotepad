using MapNotepad.Model;
using MapNotepad.Services.PinService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapNotepad.Services.SearchService
{
    public class SearchService : ISearchServise
    {
        private readonly IPinService _pinservice;

        public SearchService(IPinService pinService)
        {
            _pinservice = pinService;
        }




        public List<PinModel> Search(string search_query, IEnumerable<PinModel> list)
        {
            //double doubleValue;
            List<PinModel> outValue = new List<PinModel>();
            try
            {
                var doubleResult = double.TryParse(search_query, out double doubleValue);
              //  doubleValue = Convert.ToDouble(search_query);
                System.Console.WriteLine(
                    "The string as a double is {0}.", doubleValue);
                if (doubleResult)
                {
                    var v =  DoubleSearch(doubleValue,list);
                    foreach(PinModel pm in v)
                    {
                        outValue.Add(pm);
                    }
                }
              
            }
            catch (OverflowException)
            {
                Console.WriteLine(
                    "The conversion from string to decimal overflowed.");
            }
            catch (FormatException)
            {
                Console.WriteLine(
                    "The string is not formatted as a decimal.");
            }
            catch (ArgumentNullException)
            {
                Console.WriteLine(
                    "The string is null.");
            }

            return outValue;
        }
        #region --- Private Helpers---
        private IEnumerable<PinModel> DoubleSearch(double coordinate,IEnumerable<PinModel> list)
        {
            double delta = 0.1; // NOTE: точность может быть измененена или изменяться во времени.

            return list.Where(x => Math.Abs(x.Latitude - coordinate) <= delta || Math.Abs(x.Longitude - coordinate) <= delta);
        }
        #endregion
    }
}
