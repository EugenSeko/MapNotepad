using Xamarin.Essentials;

namespace MapNotepad.Services.Settings
{
     public class SettingsManager : ISettingsManager
    {
        public string UserId 
        {
            get => Preferences.Get(nameof(UserId), null);
            set => Preferences.Set(nameof(UserId), value); 
        }
        public string UserName
        {
            get => Preferences.Get(nameof(UserName), null);
            set => Preferences.Set(nameof(UserName), value);
        }
    }
}
