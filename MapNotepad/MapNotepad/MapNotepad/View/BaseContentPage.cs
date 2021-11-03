using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace MapNotepad.View
{
   public class BaseContentPage : ContentPage
    {

        public BaseContentPage()
        {
            BackgroundColor = Color.White;
            
        }

        #region ---Overrides---
        protected override void OnAppearing()
        {
            base.OnAppearing();
        }
        protected override void OnDisappearing()
        {
            base.OnDisappearing();
        }
        #endregion
    }
}
