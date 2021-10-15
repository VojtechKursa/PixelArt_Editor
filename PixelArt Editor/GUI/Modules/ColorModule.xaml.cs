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
        #region Variables and Properties

        /// <summary>
        /// Gets the Alpha value of the currently selected <see cref="Color"/>.
        /// </summary>
        public int Alpha { get; set; } = 255;

        /// <summary>
        /// Gets the base color (without the <see cref="Alpha"/> value) of the currently selected <see cref="Color"/>.
        /// </summary>
        public Color BaseColor { get; set; } = Color.Black;

        /// <summary>
        /// Gets the currently selected Color.
        /// </summary>
        public Color Color { get; set; } = Color.Black;

        #endregion

        #region Events

        /// <summary>
        /// Event that's called when the selected color changes.
        /// </summary>
        public event EventHandler ColorChanged;

        #endregion

        #region Constructors

        public ColorModule()
        {
            ColorChanged += ColorChanged_EventHandler;

            InitializeComponent();

            Rect_base.Fill = new SolidColorBrush(Converters.ConvertColor(BaseColor));
        }

        #endregion

        #region Methods

        /// <summary>
        /// Sets the <see cref="Color"/> and updates the GUI.
        /// </summary>
        /// <param name="color">Color to set.</param>
        public void UpdateColor(Color color)
        {
            BaseColor = Color.FromArgb(color.R, color.G, color.B);
            TB_alpha.Text = "0";    //By changing TB_alpha back and forth, it's certain that TB_alpha_TextChanged will be called, updating Alpha and Color and calling the ColorChanged event
            TB_alpha.Text = color.A.ToString();
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

        #endregion
    }
}
