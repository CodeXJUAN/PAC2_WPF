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
    public partial class DNITextBox : UserControl
    {
        // Regular expression to validate DNI format (8 digits + 1 letter)
        private static readonly Regex DniRegex = new Regex(@"^\d{8}[A-Za-z]$");

        // Letters for DNI validation
        private static readonly string ValidLetters = "TRWAGMYFPDXBNJZSQVHLCKE";

        public DNITextBox()
        {
            InitializeComponent();
            // Default color will be gray until user starts typing
            BorderColor = Brushes.Gray;
        }

        #region Dependency Properties

        public static readonly DependencyProperty DNIValueProperty =
            DependencyProperty.Register("DNIValue", typeof(string), typeof(DNITextBox),
                new PropertyMetadata(string.Empty, OnDNIValueChanged));

        public static readonly DependencyProperty IsValidProperty =
            DependencyProperty.Register("IsValid", typeof(bool), typeof(DNITextBox),
                new PropertyMetadata(false));

        public static readonly DependencyProperty BorderColorProperty =
            DependencyProperty.Register("BorderColor", typeof(Brush), typeof(DNITextBox),
                new PropertyMetadata(Brushes.Gray));

        #endregion

        #region Properties

        public string DNIValue
        {
            get { return (string)GetValue(DNIValueProperty); }
            set { SetValue(DNIValueProperty, value); }
        }

        public bool IsValid
        {
            get { return (bool)GetValue(IsValidProperty); }
            private set { SetValue(IsValidProperty, value); }
        }

        public Brush BorderColor
        {
            get { return (Brush)GetValue(BorderColorProperty); }
            private set { SetValue(BorderColorProperty, value); }
        }

        #endregion

        #region Event Handlers

        private static void OnDNIValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is DNITextBox control)
            {
                control.ValidateDNI();
            }
        }

        #endregion

        #region Methods

        private void ValidateDNI()
        {
            bool isValid = false;

            // If empty, set to default gray
            if (string.IsNullOrEmpty(DNIValue))
            {
                BorderColor = Brushes.Gray;
                IsValid = false;
                return;
            }

            // Check if the format is valid (8 digits + 1 letter)
            if (DniRegex.IsMatch(DNIValue))
            {
                // Always validate the check letter
                isValid = IsCheckLetterValid();
            }

            // Update IsValid property
            IsValid = isValid;

            // Update border color based on validation
            BorderColor = isValid ? Brushes.Green : Brushes.Red;
        }

        private bool IsCheckLetterValid()
        {
            if (DNIValue.Length != 9)
                return false;

            // Extract number and letter
            if (!int.TryParse(DNIValue.Substring(0, 8), out int dniNumber))
                return false;

            // Get the provided check letter (convert to uppercase)
            char providedLetter = char.ToUpper(DNIValue[8]);

            // Calculate the correct check letter
            int remainder = dniNumber % 23;
            char correctLetter = ValidLetters[remainder];

            // Compare the letters
            return correctLetter == providedLetter;
        }

        #endregion
    }
