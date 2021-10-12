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
        public int Alpha { get; set; } = 255;
        public Color BaseColor { get; set; } = Color.Black;

        public Color Color { get; set; } = Color.Black;

        public event EventHandler ColorChanged;

        public ColorModule()
        {
            ColorChanged += ColorChanged_EventHandler;

            InitializeComponent();

            Rect_base.Fill = new SolidColorBrush(Converters.ConvertColor(BaseColor));
        }

        public void UpdateBaseColor()
        {
            ColorChanged.Invoke(this, new EventArgs());
        }

        private void ColorChanged_EventHandler(object sender, EventArgs args)
        {
            Rect_base.Fill = new SolidColorBrush(Converters.ConvertColor(BaseColor));
        }

        private void ChangeBaseColor_EventHandler(object sender, RoutedEventArgs e)
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
                BaseColor = colorDialog.Color;
                Color = Color.FromArgb(Alpha, BaseColor);

                ColorChanged.Invoke(this, new EventArgs());
            }
        }

        private void TB_alpha_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            int alpha = -1;
            try
            {
                alpha = Convert.ToInt32(TB_alpha.Text);
            }
            catch
            { }

            if (alpha < 0 || alpha > 255)
                ResetAlpha();
            else
            {
                Alpha = alpha;
                Color = Color.FromArgb(alpha, BaseColor);

                ColorChanged.Invoke(this, new EventArgs());
            }
        }

        private void ResetAlpha()
        {
            TB_alpha.Text = Color.A.ToString();
        }
    }
}
