using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.GoogleMaps;
using Xamarin.Forms.Xaml;

namespace MapNotepad.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MapPage : ContentPage
    {
        public MapPage()
        {
            InitializeComponent();
            map.UiSettings.MyLocationButtonEnabled = true;
            map.MapLongClicked += AddPin;
        }
        void AddPin(object sender, MapLongClickedEventArgs e)
        {
            var pin = new Pin()
            {
                Type = PinType.Place,
                Label = "myPin",
                Position = e.Point,
                
            };
            map.Pins.Add(pin);
        }
    }
}