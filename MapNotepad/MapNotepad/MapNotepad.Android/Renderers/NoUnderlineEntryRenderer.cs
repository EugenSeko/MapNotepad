using Android.Content;
using MapNotepad.Controls;
using MapNotepad.Droid;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(NoUnderlineEntry), typeof(NoUnderlineEntryRenderer))]
namespace MapNotepad.Droid
{
   class NoUnderlineEntryRenderer : EntryRenderer
    {
        public NoUnderlineEntryRenderer(Context context):base(context)
        {
        }
        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);
            if (Control != null)
            {
                var element = (NoUnderlineEntry)Element;
                if (!element.HasUnderline)
                {
                    Control.Background = null;
                    Control.SetBackgroundColor(Android.Graphics.Color.Transparent);
                }
            }
        
        }
    }
}