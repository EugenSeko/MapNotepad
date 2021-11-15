using MapNotepad.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.GoogleMaps;

namespace MapNotepad.Controls
{
    public class CustomMap : Map
    {
        public CustomMap()
        {
            UiSettings.MyLocationButtonEnabled = true;
            MyLocationEnabled = true;
            UiSettings.ZoomControlsEnabled = false;
            PinClicked += CustomMap_PinClicked;
            MapClicked += CustomMapClicked;
        }

        private void CustomMapClicked(object sender, MapClickedEventArgs e)
        {
            MapClickedCommand?.Execute(null);
        }

        private void CustomMap_PinClicked(object sender, PinClickedEventArgs e)
        {
            var model = PinSource?.FirstOrDefault(item => item.Label == e.Pin.Label);
            if(model != null)   
                PinClickedCommand?.Execute(model);
        }

        public static readonly BindableProperty MapClickedCommandProperty =
          BindableProperty.Create(nameof(MapClickedCommand), typeof(ICommand), typeof(CustomMap), null, BindingMode.TwoWay);
        public ICommand MapClickedCommand
        {
            get { return (ICommand)GetValue(MapClickedCommandProperty); }
            set { SetValue(MapClickedCommandProperty, value); }
        }

        public static readonly BindableProperty PinClickedCommandProperty =
          BindableProperty.Create(nameof(PinClickedCommand), typeof(ICommand), typeof(CustomMap), null, BindingMode.TwoWay);

        public ICommand PinClickedCommand
        {
            get { return (ICommand)GetValue(PinClickedCommandProperty); }
            set { SetValue(PinClickedCommandProperty, value); }
        }

        public static readonly BindableProperty PinClickedCommandParameterProperty =
         BindableProperty.Create(nameof(PinClickedCommandParameter), typeof(PinModel), typeof(CustomMap), null, BindingMode.TwoWay);

        public PinModel PinClickedCommandParameter
        {
            get { return (PinModel)GetValue(PinClickedCommandParameterProperty); }
            set { SetValue(PinClickedCommandParameterProperty, value); }
        }


        public static readonly BindableProperty PinSourceProperty =
            BindableProperty.Create(nameof(PinSource), typeof(List<PinModel>), typeof(CustomMap), null);

        public List<PinModel> PinSource
        {
            get { return (List<PinModel>)GetValue(PinSourceProperty); }
            set { SetValue(PinSourceProperty, value);  }
        }

        public PinClickedEventArgs PinClickArg //my
        {
            get { return (PinClickedEventArgs)GetValue(PinClickArgProperty); }
            set { SetValue(PinClickArgProperty, value); }
        }

        public static readonly BindableProperty PinClickArgProperty =
            BindableProperty.Create(nameof(PinClickArg), typeof(PinClickedEventArgs), typeof(CustomMap), null, BindingMode.TwoWay);

        protected override void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            base.OnPropertyChanged(propertyName);

            if (propertyName == nameof(PinSource))
            {
                if (PinSource != null)
                {
                    Pins.Clear();

                    foreach (PinModel item in PinSource)
                    {

                        var pin = new Pin()
                        {
                            Label = item.Label,
                            Position = new Position(item.Latitude, item.Longitude),
                        };

                        this.Pins.Add(pin);
                    }
                }
            }
        }
    }
}
