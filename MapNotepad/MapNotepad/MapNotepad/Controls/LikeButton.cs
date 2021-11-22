using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace MapNotepad.Controls
{
    class LikeButton : ImageButton
    {
        public static readonly BindableProperty IsFavoriteProperty = BindableProperty.Create(
            propertyName: nameof(IsFavorite),
            returnType: typeof(bool),
            declaringType: typeof(LikeButton),
            defaultValue: true,
            defaultBindingMode: BindingMode.TwoWay);

        public bool IsFavorite
        {
            set => SetValue(IsFavoriteProperty, value);
            get => (bool)GetValue(IsFavoriteProperty);
        }
    }
}
