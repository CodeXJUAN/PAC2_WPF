using System;
using System.ComponentModel;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace CustomControlsLib
{
    public partial class EmailTextBox : UserControl, INotifyPropertyChanged
    {
        private static readonly Regex EmailRegex = new Regex(@"^[\w\.-]+@([\w-]+\.)+[\w-]{2,4}$", RegexOptions.Compiled);

        private bool _isValid;

        public EmailTextBox()
        {
            InitializeComponent();
            textBox.TextChanged += OnTextBoxTextChanged;
        }

        // DependencyProperty para Email
        public static readonly DependencyProperty EmailProperty =
            DependencyProperty.Register(nameof(Email), typeof(string), typeof(EmailTextBox),
                new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnEmailChanged));

        public string Email
        {
            get => (string)GetValue(EmailProperty);
            set => SetValue(EmailProperty, value);
        }

        // DependencyProperty para BorderColor
        public static readonly DependencyProperty BorderColorProperty =
            DependencyProperty.Register(nameof(BorderColor), typeof(Brush), typeof(EmailTextBox),
            new PropertyMetadata(Brushes.Gray));

        public Brush BorderColor
        {
            get => (Brush)GetValue(BorderColorProperty);
            set => SetValue(BorderColorProperty, value);
        }

        // Propiedad IsValid (solo get)
        public bool IsValid
        {
            get => _isValid;
            private set
            {
                if (_isValid != value)
                {
                    _isValid = value;
                    OnPropertyChanged(nameof(IsValid));
                }
            }
        }

        private static void OnEmailChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = (EmailTextBox)d;
            control.ValidateEmail();
        }

        private void OnTextBoxTextChanged(object sender, TextChangedEventArgs e)
        {
            if (Email != textBox.Text)
            {
                Email = textBox.Text;
            }
        }

        private void ValidateEmail()
        {
            IsValid = EmailRegex.IsMatch(Email);
            BorderColor = IsValid ? Brushes.Gray : Brushes.Red;
            OnPropertyChanged(nameof(BorderColor));
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}