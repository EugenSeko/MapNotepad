using System;
using System.Collections.Generic;
using System.Text;
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
    }
}
