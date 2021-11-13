using Foundation;
using MapNotepad.Controls;
using MapNotepad.iOS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer (typeof(NoUnderlineEntry),typeof(NoUnderlineEntryRenderer))]
namespace MapNotepad.iOS
{
    class NoUnderlineEntryRenderer : EntryRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);
            if (Control != null)
            {
                Control.BorderStyle = UITextBorderStyle.Line;
            }
        }
    }
}