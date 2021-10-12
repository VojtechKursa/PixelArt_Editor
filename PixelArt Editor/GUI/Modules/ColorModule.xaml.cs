using PixelArt_Editor.Functions;
using System;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Media;
using Color = System.Drawing.Color;
using UserControl = System.Windows.Controls.UserControl;

namespace PixelArt_Editor.GUI.Modules
{
    /// <summary>
    /// Interaction logic for ColorModule.xaml
    /// </summary>
    public partial class ColorModule : UserControl
    {
        public Color Color { get; set; } = Color.Black;

        public event EventHandler ColorChanged;

        public ColorModule()
        {
            InitializeComponent();

            Rect_color.Fill = new SolidColorBrush(Converters.ConvertColor(Color));

            ColorChanged += ColorChanged_EventHandler;
        }

        private void B_changeColor_Click(object sender, RoutedEventArgs e)
        {
            ColorDialog colorDialog = new ColorDialog()
            {
                AllowFullOpen = true,
                AnyColor = true,
                FullOpen = true,
                SolidColorOnly = false
            };

            if (colorDialog.ShowDialog() == DialogResult.OK)
            {
                Color = colorDialog.Color;
                Rect_color.Fill = new SolidColorBrush(Converters.ConvertColor(Color));

                ColorChanged.Invoke(this, new EventArgs());
            }
        }

        //Default event handler so exception is not thrown when the event is invoked with no eventhandlers added
        private void ColorChanged_EventHandler(object sender, EventArgs args)
        { }
    }
}
