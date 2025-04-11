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
        private static readonly Regex EmailRegex = new Regex(@"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$");

        public EmailTextBox()
        {
            InitializeComponent();
            textBox.TextChanged += TextBox_TextChanged;
        }

        // DependencyProperty for the email value
        public static readonly DependencyProperty EmailProperty =
            DependencyProperty.Register("Email", typeof(string), typeof(EmailTextBox),
                new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnEmailChanged));

        public string Email
        {
            get => (string)GetValue(EmailProperty);
            set => SetValue(EmailProperty, value);
        }

        private static void OnEmailChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = (EmailTextBox)d;
            if (control.textBox.Text != (string)e.NewValue)
            {
                control.textBox.Text = (string)e.NewValue;
            }
            control.ValidateEmail();
        }

        // Public property IsValid
        public bool IsValid { get; private set; } = false;

        // Property to control the border color
        public Brush BorderColor
        {
            get => (Brush)GetValue(BorderColorProperty);
            set => SetValue(BorderColorProperty, value);
        }

        public static readonly DependencyProperty BorderColorProperty =
            DependencyProperty.Register("BorderColor", typeof(Brush), typeof(EmailTextBox), new PropertyMetadata(Brushes.Gray));

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (Email != textBox.Text)
            {
                Email = textBox.Text;
            }
        }

        private void ValidateEmail()
        {
            if (EmailRegex.IsMatch(Email))
            {
                BorderColor = Brushes.Gray;
                IsValid = true;
            }
            else
            {
                BorderColor = Brushes.Red;
                IsValid = false;
            }
            OnPropertyChanged(nameof(IsValid)); // Notify change of IsValid
        }

        // Implementation of INotifyPropertyChanged
        public event PropertyChangedEventHandler? PropertyChanged; // Marked as nullable to fix CS8618

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void textBox_TextChanged_1(object sender, TextChangedEventArgs e)
        {
            // Empty event handler
        }
    }
}
