using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
    /// <summary>
    /// Lógica de interacción para UserControl1.xaml
    /// </summary>
    public partial class MinLengthTextBox : UserControl
    {
        // DependencyProperty per al valor mínim de caràcters requerits
        public static readonly DependencyProperty MinLengthProperty =
            DependencyProperty.Register(
                nameof(MinLength),
                typeof(int),
                typeof(MinLengthTextBox),
                new PropertyMetadata(0, OnMinLengthChanged));

        // Propietat pública per accedir a MinLength
        public int MinLength
        {
            get => (int)GetValue(MinLengthProperty);
            set => SetValue(MinLengthProperty, value);
        }

        // DependencyProperty per saber si el text és vàlid
        private static readonly DependencyPropertyKey IsValidPropertyKey =
            DependencyProperty.RegisterReadOnly(
                nameof(IsValid),
                typeof(bool),
                typeof(MinLengthTextBox),
                new PropertyMetadata(false));

        public static readonly DependencyProperty IsValidProperty = IsValidPropertyKey.DependencyProperty;

        // Propietat pública per accedir a IsValid
        public bool IsValid
        {
            get => (bool)GetValue(IsValidProperty);
            private set => SetValue(IsValidPropertyKey, value);
        }

        // Constructor
        public MinLengthTextBox()
        {
            InitializeComponent();

            // Subscriure's a l'esdeveniment TextChanged del TextBox intern
            InternalTextBox.TextChanged += OnTextChanged;

            // Aplicar un estil personalitzat al TextBox intern
            ApplyCustomStyle();
        }

        // Mètode per gestionar canvis en el valor de MinLength
        private static void OnMinLengthChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is MinLengthTextBox control)
            {
                control.ValidateText();
            }
        }

        // Mètode per gestionar canvis en el text
        private void OnTextChanged(object sender, TextChangedEventArgs e)
        {
            ValidateText();
        }

        // Mètode per validar el text i actualitzar l'estat del control
        private void ValidateText()
        {
            bool isValid = string.IsNullOrEmpty(InternalTextBox.Text) ? false : InternalTextBox.Text.Length >= MinLength;

            // Actualitzar la propietat IsValid
            IsValid = isValid;

            // Resaltar les vores en vermell si el text no és vàlid
            if (!isValid)
            {
                InternalTextBox.BorderBrush = Brushes.Red;
                InternalTextBox.BorderThickness = new Thickness(2);
            }
            else
            {
                InternalTextBox.ClearValue(BorderBrushProperty);
                InternalTextBox.ClearValue(BorderThicknessProperty);
            }
        }

        // Aplicar un estil personalitzat al TextBox intern
        private void ApplyCustomStyle()
        {
            InternalTextBox.Style = new Style(typeof(TextBox))
            {
                Setters =
                {
                    // Establir colors i estils bàsics
                    new Setter(Control.ForegroundProperty, Brushes.Black),
                    new Setter(Control.BackgroundProperty, Brushes.White),
                    new Setter(Control.BorderBrushProperty, Brushes.Gray),
                    new Setter(Control.BorderThicknessProperty, new Thickness(1)),

                    // Desactivar els estats predeterminats (Focus, Hover, etc.)
                    new Setter(Control.TemplateProperty, null)
                }
            };
        }
    }
}
