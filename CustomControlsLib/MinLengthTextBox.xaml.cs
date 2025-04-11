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
        public static readonly DependencyProperty MinLengthProperty =
            DependencyProperty.Register(
                nameof(MinLength),
                typeof(int),
                typeof(MinLengthTextBox),
                new PropertyMetadata(0, OnMinLengthChanged));

        
        public int MinLength
        {
            get => (int)GetValue(MinLengthProperty);
            set => SetValue(MinLengthProperty, value);
        }

        
        public bool IsValid
        {
            get => (bool)GetValue(IsValidProperty);
            private set => SetValue(IsValidPropertyKey, value);
        }

        
        private static readonly DependencyPropertyKey IsValidPropertyKey =
            DependencyProperty.RegisterReadOnly(
                nameof(IsValid),
                typeof(bool),
                typeof(MinLengthTextBox),
                new PropertyMetadata(false));

        public static readonly DependencyProperty IsValidProperty = IsValidPropertyKey.DependencyProperty;

        public MinLengthTextBox()
        {
            InitializeComponent();

            InternalTextBox.TextChanged += OnTextChanged;
        }

        private static void OnMinLengthChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is MinLengthTextBox control)
            {
                control.ValidateText();
            }
        }

        private void OnTextChanged(object sender, TextChangedEventArgs e)
        {
            ValidateText();
        }

        private void ValidateText()
        {
            bool isValid = string.IsNullOrEmpty(InternalTextBox.Text) ? false : InternalTextBox.Text.Length >= MinLength;

            IsValid = isValid;

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
    }
}
