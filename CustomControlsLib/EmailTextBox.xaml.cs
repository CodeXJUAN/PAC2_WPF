using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
namespace CustomControlsLib
{
    public partial class EmailTextBox : UserControl
    {
        private static readonly Regex EmailRegex = new Regex(@"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$");

        public EmailTextBox()
        {
            InitializeComponent();
            textBox.TextChanged += TextBox_TextChanged;
        }

        // DependencyProperty per al valor de l'email
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

        // Propietat pública IsValid
        public bool IsValid { get; private set; } = false;

        // Propietat per controlar el color de la vora
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
        }
    }
}