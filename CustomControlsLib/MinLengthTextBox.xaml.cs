using System.Windows;
using System.Windows.Controls;

namespace CustomControlsLib
{
    public partial class MinLengthTextBox : UserControl
    {
        public MinLengthTextBox()
        {
            InitializeComponent();
            System.Diagnostics.Debug.WriteLine($"MinLength inicialitzat: {MinLength}");
        }

        #region Dependency Properties

        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register(
                nameof(Text),
                typeof(string),
                typeof(MinLengthTextBox),
                new PropertyMetadata(string.Empty, OnTextChanged));

        public string Text
        {
            get => (string)GetValue(TextProperty);
            set => SetValue(TextProperty, value);
        }

        public static readonly DependencyProperty MinLengthProperty =
            DependencyProperty.Register(
                nameof(MinLength),
                typeof(int),
                typeof(MinLengthTextBox),
                new PropertyMetadata(0, OnTextChanged));

        public int MinLength
        {
            get => (int)GetValue(MinLengthProperty);
            set => SetValue(MinLengthProperty, value);
        }

        public static readonly DependencyProperty IsValidMinLengthProperty =
            DependencyProperty.Register(
                nameof(IsValidMinLength),
                typeof(bool),
                typeof(MinLengthTextBox),
                new PropertyMetadata(true));

        public bool IsValidMinLength
        {
            get => (bool)GetValue(IsValidMinLengthProperty);
            private set => SetValue(IsValidMinLengthProperty, value);
        }

        #endregion

        #region Event Handlers

        private static void OnTextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = (MinLengthTextBox)d;
            control.ValidateText();
            System.Diagnostics.Debug.WriteLine($"Text canviat: '{control.Text}', IsValidMinLength: {control.IsValidMinLength}");
        }

        private void ValidateText()
        {
            IsValidMinLength = string.IsNullOrEmpty(Text) || Text.Length >= MinLength;
        }

        #endregion
    }
}