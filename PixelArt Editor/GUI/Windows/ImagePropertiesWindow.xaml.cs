using PixelArt_Editor.Data;
using PixelArt_Editor.Functions;
using System;
using System.Windows;
using System.Windows.Media;
using Color = System.Drawing.Color;

namespace PixelArt_Editor.GUI.Windows
{
    /// <summary>
    /// Interaction logic for ImagePropertiesWindow.xaml
    /// </summary>
    public partial class ImagePropertiesWindow : Window
    {
        #region Variables

        private readonly MainWindow mainWindow;
        private Color defaultBackgroundColor;

        #endregion

        #region Constructors

        public ImagePropertiesWindow(MainWindow mainWindow)
        {
            InitializeComponent();

            this.mainWindow = mainWindow;
            defaultBackgroundColor = Color.White;

            CM_backColor.UpdateColor(Color.White);
            CM_backColor.ColorChanged += CM_backColor_ColorChanged;
        }

        public ImagePropertiesWindow(MainWindow mainWindow, ImageProperties properties) : this(mainWindow)
        {
            defaultBackgroundColor = properties.BackgroundColor;
            CM_backColor.UpdateColor(properties.BackgroundColor);
            UpdateRectangleColor();

            TB_name.Text = properties.Name;
            TB_height.Text = properties.Height.ToString();
            TB_width.Text = properties.Width.ToString();

            if (properties.ResizeMode != ImageResizeMode.Unset)
            {
                if (properties.ResizeMode == ImageResizeMode.Centered)
                    CB_resizeMode.SelectedIndex = 0;
                else
                    CB_resizeMode.SelectedIndex = 1;
            }

            CB_resizeMode.IsEnabled = true;
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

        private void CM_backColor_ColorChanged(object sender, EventArgs e)
        {
            defaultBackgroundColor = CM_backColor.Color;
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

            if (!CB_resizeMode.IsEnabled)
                return new ImageProperties(TB_name.Text, width, height, defaultBackgroundColor);
            else
            {
                ImageResizeMode resizeMode;

                if (CB_resizeMode.SelectedIndex == 0)
                    resizeMode = ImageResizeMode.Centered;
                else
                    resizeMode = ImageResizeMode.BottomRight;

                return new ImageProperties(TB_name.Text, width, height, defaultBackgroundColor, resizeMode);
            }
        }

        private void UpdateRectangleColor()
        {
            Rect_backColor.Fill = new SolidColorBrush(Converters.ConvertColor(defaultBackgroundColor));
        }

        #endregion Methods
    }
}
