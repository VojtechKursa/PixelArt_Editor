using PixelArt_Editor.Data;
using PixelArt_Editor.Functions;
using System;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Media;
using Color = System.Drawing.Color;

namespace PixelArt_Editor.GUI.Windows
{
    /// <summary>
    /// Interaction logic for PicturePropertiesWindow.xaml
    /// </summary>
    public partial class PicturePropertiesWindow : Window
    {
        #region Variables

        private readonly MainWindow mainWindow;
        private Color defaultBackgroundColor;

        #endregion

        #region Constructors

        public PicturePropertiesWindow(MainWindow mainWindow)
        {
            InitializeComponent();

            this.mainWindow = mainWindow;
            defaultBackgroundColor = Color.White;
        }

        #endregion

        #region Event handlers

        private void B_cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private void B_confirm_Click(object sender, RoutedEventArgs e)
        {
            ImageProperties properties = MakeProperties();

            if (properties != null)
            {
                mainWindow.ImageProperties = properties;

                DialogResult = true;
                Close();
            }
        }

        private void B_changeBackColor_Click(object sender, RoutedEventArgs e)
        {
            ColorDialog dialog = new ColorDialog();
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                defaultBackgroundColor = dialog.Color;
                UpdateRectangleColor();
            }
        }

        private void B_changeBackColor_ToTransparent_Click(object sender, RoutedEventArgs e)
        {
            defaultBackgroundColor = Color.FromArgb(0, 255, 255, 255);
            UpdateRectangleColor();
        }

        #endregion

        #region Methods

        private ImageProperties MakeProperties()
        {
            if (TB_name.Text == "")
            {
                MessageBoxes.ShowError("Name must not be empty");
                return null;
            }

            int width = -1;
            int height = -1;
            try
            {
                width = Convert.ToInt32(TB_width.Text);
                height = Convert.ToInt32(TB_height.Text);
            }
            catch
            { }

            if (width < 1 && height < 1)
            {
                MessageBoxes.ShowError("Invalid height or width");
                return null;
            }

            return new ImageProperties(TB_name.Text, width, height, defaultBackgroundColor);
        }

        private void UpdateRectangleColor()
        {
            Rect_backColor.Fill = new SolidColorBrush(Converters.ConvertColor(defaultBackgroundColor));
        }

        #endregion Methods
    }
}
