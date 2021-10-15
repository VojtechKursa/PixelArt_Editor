using System;
using System.Windows;
using System.Windows.Controls;

namespace PixelArt_Editor.GUI.Modules
{
    /// <summary>
    /// Interaction logic for LineWidthModule.xaml
    /// </summary>
    public partial class LineWidthModule : UserControl
    {
        /// <summary>
        /// Returns the currently selected line width.
        /// </summary>
        public int LineWidth { get => (int)Slider_lineWidth.Value; }

        /// <summary>
        /// Gets or sets the maximum line width selectable by this control.
        /// </summary>
        public int MaxLineWidth
        {
            get => (int)Slider_lineWidth.Maximum;
            set
            {
                if (Slider_lineWidth.Value > value)
                    Slider_lineWidth.Value = value;

                Slider_lineWidth.Maximum = value;
            }
        }

        public LineWidthModule()
        {
            InitializeComponent();
        }

        private void Slider_lineWidth_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            TB_lineWidth.Text = LineWidth.ToString();
        }

        private void TB_lineWidth_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (Slider_lineWidth != null)
            {
                bool isNumber = false;
                int number = 0;

                try
                {
                    number = Convert.ToInt32(TB_lineWidth.Text);
                    isNumber = true;
                }
                catch
                { }

                if (isNumber)
                    Slider_lineWidth.Value = number;
                else
                    TB_lineWidth.Text = LineWidth.ToString();
            }
        }
    }
}
