using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Text.RegularExpressions;
using System.Windows.Data;
using System.Globalization;

namespace CustomControlsLib
{
    public partial class DNITextBox : UserControl
    {
        private static readonly Regex DniRegex = new(@"^\d{8}[A-Za-z]$");
        private static readonly string ValidLetters = "TRWAGMYFPDXBNJZSQVHLCKE";

        public DNITextBox()
        {
            InitializeComponent();
        }

        #region Dependency Properties
        public static readonly DependencyProperty DNIValueProperty =
            DependencyProperty.Register("DNIValue", typeof(string), typeof(DNITextBox),
                new PropertyMetadata(string.Empty, OnDNIValueChanged));

        public static readonly DependencyProperty IsValidProperty =
            DependencyProperty.Register("IsValid", typeof(bool), typeof(DNITextBox),
                new PropertyMetadata(false));

        public static readonly DependencyProperty TextColorProperty =
            DependencyProperty.Register("TextColor", typeof(Brush), typeof(DNITextBox),
                new PropertyMetadata(Brushes.Black));

        public static readonly DependencyProperty ErrorMessageProperty =
            DependencyProperty.Register("ErrorMessage", typeof(string), typeof(DNITextBox),
                new PropertyMetadata(string.Empty));

        public static readonly DependencyProperty ShowErrorMessageProperty =
            DependencyProperty.Register("ShowErrorMessage", typeof(bool), typeof(DNITextBox),
                new PropertyMetadata(false));
        #endregion

        #region Properties
        public string DNIValue
        {
            get => (string)GetValue(DNIValueProperty);
            set => SetValue(DNIValueProperty, value);
        }

        public bool IsValid
        {
            get => (bool)GetValue(IsValidProperty);
            private set => SetValue(IsValidProperty, value);
        }

        public Brush TextColor
        {
            get => (Brush)GetValue(TextColorProperty);
            private set => SetValue(TextColorProperty, value);
        }

        public string ErrorMessage
        {
            get => (string)GetValue(ErrorMessageProperty);
            set => SetValue(ErrorMessageProperty, value);
        }

        public bool ShowErrorMessage
        {
            get => (bool)GetValue(ShowErrorMessageProperty);
            set => SetValue(ShowErrorMessageProperty, value);
        }
        #endregion

        private static void OnDNIValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is DNITextBox control) control.ValidateDNI();
        }

        private void ValidateDNI()
        {
            if (string.IsNullOrEmpty(DNIValue))
            {
                TextColor = Brushes.Red;
                IsValid = false;
                ErrorMessage = "DNI no puede estar vacío";
                ShowErrorMessage = true;
                return;
            }

            if (DNIValue.Length > 0 && DNIValue.Length != 9)
            {
                TextColor = Brushes.Red;
                IsValid = false;
                ErrorMessage = "El DNI debe tener 9 caracteres (8 números + 1 letra)";
                ShowErrorMessage = true;
                return;
            }

            if (DniRegex.IsMatch(DNIValue))
            {
                bool isValidLetter = IsCheckLetterValid();
                TextColor = isValidLetter ? Brushes.Green : Brushes.Red;
                IsValid = isValidLetter;

                if (!isValidLetter)
                {
                    ErrorMessage = "La letra de control no es válida";
                    ShowErrorMessage = true;
                }
                else
                {
                    ShowErrorMessage = false;
                }
            }
            else
            {
                TextColor = Brushes.Red;
                IsValid = false;
                ErrorMessage = "Formato de DNI inválido. Debe ser 8 números seguidos de una letra";
                ShowErrorMessage = true;
            }
        }

        private bool IsCheckLetterValid()
        {
            if (DNIValue.Length != 9) return false;
            if (!int.TryParse(DNIValue[..8], out int dniNumber)) return false;

            char providedLetter = char.ToUpper(DNIValue[8]);
            return ValidLetters[dniNumber % 23] == providedLetter;
        }
    }

}