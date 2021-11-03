using Android.Content;
using MapNotepad.Controls;
using MapNotepad.Droid;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(MyEntry), typeof(MyEntryRenderer))]
namespace MapNotepad.Droid
{
   class MyEntryRenderer : EntryRenderer
    {
        public MyEntryRenderer(Context context):base(context)
        {
        }
        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);
            if (Control != null)
            {
                var element = (MyEntry)Element;
                if (!element.HasUnderline)
                {
                    Control.Background = null;
                    Control.SetBackgroundColor(Android.Graphics.Color.Transparent);
                }
            }
        
        }
    }
}