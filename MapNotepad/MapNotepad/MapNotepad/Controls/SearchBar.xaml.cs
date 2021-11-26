using MapNotepad.Helpers;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace MapNotepad.Controls
{
    public partial class SearchBar : StackLayout
    {
        public SearchBar()
        {
            InitializeComponent();
        }
        #region -- Public properties --
        private ICommand _goBackButtonCommand;
        public ICommand GoBackButtonCommand => _goBackButtonCommand ??= SingleExecutionCommand.FromFunc(OnGoBackButtonCommandAsync);
        private ICommand _clearButtonCommand;
        public ICommand ClearButtonCommand => _clearButtonCommand ??= SingleExecutionCommand.FromFunc(OnClearButtonCommandAsync);
        public ICommand LeftButtonCommand
        {
            set => SetValue(LeftButtonCommandProperty, value);
            get => (ICommand)GetValue(LeftButtonCommandProperty);
        }
        public static readonly BindableProperty LeftButtonCommandProperty = BindableProperty.Create(
          propertyName: nameof(LeftButtonCommand),
          returnType: typeof(ICommand),
          declaringType: typeof(SearchBar),
          defaultValue: null,
          defaultBindingMode: BindingMode.TwoWay);
        public static readonly BindableProperty RightButtonCommandProperty = BindableProperty.Create(
            propertyName: nameof(RightButtonCommand),
            returnType: typeof(ICommand),
            declaringType: typeof(SearchBar),
            defaultValue: null,
            defaultBindingMode: BindingMode.TwoWay);
        public ICommand RightButtonCommand
        {
            set => SetValue(RightButtonCommandProperty, value);
            get => (ICommand)GetValue(RightButtonCommandProperty);
        }
        public static readonly BindableProperty TextProperty = BindableProperty.Create(
            propertyName: nameof(Text),
            returnType: typeof(string),
            declaringType: typeof(SearchBar),
            defaultValue: string.Empty,
            defaultBindingMode: BindingMode.TwoWay);
        public string Text
        {
            set => SetValue(TextProperty, value);
            get => (string)GetValue(TextProperty);
        }

        public static readonly BindableProperty TextColorProperty = BindableProperty.Create(
            propertyName: nameof(TextColor),
            returnType: typeof(Color),
            declaringType: typeof(SearchBar),
            defaultValue: Color.Black,
            defaultBindingMode: BindingMode.TwoWay);
        public Color TextColor
        {
            set => SetValue(TextColorProperty, value);
            get => (Color)GetValue(TextColorProperty);
        }
        public static readonly BindableProperty FontFamilyProperty = BindableProperty.Create(
            propertyName: nameof(FontFamily),
            returnType: typeof(string),
            declaringType: typeof(SearchBar),
            defaultValue: string.Empty,
            defaultBindingMode: BindingMode.TwoWay);
        public string FontFamily
        {
            set => SetValue(FontFamilyProperty, value);
            get => (string)GetValue(FontFamilyProperty);
        }
        public static readonly BindableProperty PlaceholderProperty = BindableProperty.Create(
            propertyName: nameof(Placeholder),
            returnType: typeof(string),
            declaringType: typeof(SearchBar),
            defaultValue: string.Empty,
            defaultBindingMode: BindingMode.TwoWay);
        public string Placeholder
        {
            set => SetValue(PlaceholderProperty, value);
            get => (string)GetValue(PlaceholderProperty);
        }
        public static readonly BindableProperty PlaceholderColorProperty = BindableProperty.Create(
            propertyName: nameof(PlaceholderColor),
            returnType: typeof(Color),
            declaringType: typeof(SearchBar),
            defaultValue: Color.Silver,
            defaultBindingMode: BindingMode.TwoWay);
        public Color PlaceholderColor
        {
            set => SetValue(PlaceholderColorProperty, value);
            get => (Color)GetValue(PlaceholderColorProperty);
        }
        public static readonly BindableProperty EntryBackgroundColorProperty = BindableProperty.Create(
            propertyName: nameof(EntryBackgroundColor),
            returnType: typeof(Color),
            declaringType: typeof(SearchBar),
            defaultValue: Color.Gray,
            defaultBindingMode: BindingMode.TwoWay);
        public Color EntryBackgroundColor
        {
            set => SetValue(EntryBackgroundColorProperty, value);
            get => (Color)GetValue(EntryBackgroundColorProperty);
        }
        public static readonly BindableProperty LeftImageSourceProperty = BindableProperty.Create(
            propertyName: nameof(LeftImageSource),
            returnType: typeof(string),
            declaringType: typeof(SearchBar),
            defaultValue: string.Empty,
            defaultBindingMode: BindingMode.TwoWay);
        public string LeftImageSource
        {
            set => SetValue(LeftImageSourceProperty, value);
            get => (string)GetValue(LeftImageSourceProperty);
        }
        public static readonly BindableProperty RightImageSourceProperty = BindableProperty.Create(
            propertyName: nameof(RightImageSource),
            returnType: typeof(string),
            declaringType: typeof(SearchBar),
            defaultValue: string.Empty,
            defaultBindingMode: BindingMode.TwoWay);
        public string RightImageSource
        {
            set => SetValue(RightImageSourceProperty, value);
            get => (string)GetValue(RightImageSourceProperty);
        }

        public static readonly BindableProperty GoBackImageSourceProperty = BindableProperty.Create(
            propertyName: nameof(GoBackImageSource),
            returnType: typeof(string),
            declaringType: typeof(SearchBar),
            defaultValue: string.Empty,
            defaultBindingMode: BindingMode.TwoWay);
        public string GoBackImageSource
        {
            set => SetValue(GoBackImageSourceProperty, value);
            get => (string)GetValue(GoBackImageSourceProperty);
        }
        public static readonly BindableProperty ClearImageSourceProperty = BindableProperty.Create(
            propertyName: nameof(ClearImageSource),
            returnType: typeof(string),
            declaringType: typeof(SearchBar),
            defaultValue: string.Empty,
            defaultBindingMode: BindingMode.TwoWay);
        public string ClearImageSource
        {
            set => SetValue(ClearImageSourceProperty, value);
            get => (string)GetValue(ClearImageSourceProperty);
        }
        public static readonly BindableProperty IsEntryFocusedProperty = BindableProperty.Create(
            propertyName: nameof(IsEntryFocused),
            returnType: typeof(bool),
            declaringType: typeof(SearchBar),
            defaultValue: false,
            defaultBindingMode: BindingMode.TwoWay);
        public bool IsEntryFocused
        {
            set => SetValue(IsEntryFocusedProperty, value);
            get => (bool)GetValue(IsEntryFocusedProperty);
        }
        public static readonly BindableProperty LineColorProperty = BindableProperty.Create(
            propertyName: nameof(LineColor),
            returnType: typeof(Color),
            declaringType: typeof(SearchBar),
            defaultValue: Color.White,
            defaultBindingMode: BindingMode.TwoWay);
        public Color LineColor
        {
            set => SetValue(LineColorProperty, value);
            get => (Color)GetValue(LineColorProperty);
        }
        #endregion
        #region -- Private helpers --
        private Task OnGoBackButtonCommandAsync()
        {
            IsEntryFocused = false;
            Text = string.Empty;

            return Task.CompletedTask;
        }
        private Task OnClearButtonCommandAsync()
        {
            Text = string.Empty;

            return Task.CompletedTask;
        }
        #endregion
    }
}