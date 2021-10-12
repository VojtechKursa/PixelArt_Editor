using PixelArt_Editor.Functions;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Color = System.Drawing.Color;

namespace PixelArt_Editor.GUI.Modules
{
    /// <summary>
    /// Interaction logic for ColorSelectorModule.xaml
    /// </summary>
    public partial class ColorSelectorModule : UserControl
    {
        public Color Color { get; set; } = Color.Black;

        public ColorSelectorModule()
        {
            InitializeComponent();

            Rect_color1.Fill = new SolidColorBrush(Converters.ConvertColor(CM_color1.Color));
            Rect_color2.Fill = new SolidColorBrush(Converters.ConvertColor(CM_color2.Color));

            RB_color1.IsChecked = true;
        }

        private void UpdateSelectedColor()
        {
            if ((bool)RB_color1.IsChecked)
                Color = CM_color1.Color;
            else if ((bool)RB_color2.IsChecked)
                Color = CM_color2.Color;
        }

        private void ColorSelectionChanged_EventHandler(object sender, RoutedEventArgs e)
        {
            UpdateSelectedColor();
        }

        private void CM_color1_ColorChanged(object sender, EventArgs e)
        {
            Rect_color1.Fill = new SolidColorBrush(Converters.ConvertColor(CM_color1.Color));

            UpdateSelectedColor();
        }

        private void CM_color2_ColorChanged(object sender, EventArgs e)
        {
            Rect_color2.Fill = new SolidColorBrush(Converters.ConvertColor(CM_color2.Color));

            UpdateSelectedColor();
        }
    }
}
