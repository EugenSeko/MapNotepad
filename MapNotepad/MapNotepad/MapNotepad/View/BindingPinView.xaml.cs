using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MapNotepad.View
{
    public partial class BindingPinView : StackLayout
    {
        private string _display;

        public BindingPinView(string display)
        {
            InitializeComponent();
            _display = display;
            BindingContext = this;
        }

        public string Display
        {
            get { return _display; }
        }
    }
}