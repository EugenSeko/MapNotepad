using MapNotepad.Controls.StateContainer.Animation;
using Xamarin.Forms;

namespace MapNotepad.Controls.StateContainer
{
    [ContentProperty("Content")]
    public class StateCondition
    {
        public object Is { get; set; }
        public object IsNot { get; set; }
        public View Content { get; set; }

        public AnimationBase Appearing { get; set; }
        public AnimationBase Disappearing { get; set; }
    }
}