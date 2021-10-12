using PixelArt_Editor.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows;
using Color = System.Drawing.Color;

namespace PixelArt_Editor.GUI.TabItemContents
{
    /// <summary>
    /// Interaction logic for ImageEditor.xaml
    /// </summary>
    public partial class ImageEditor : TabItemContent
    {
        #region Variables and Properties

        public string SaveLocation { get; set; } = null;
        public ImageFormat SaveFormat { get; set; } = null;

        #endregion

        #region Constructors

        public ImageEditor(ImageProperties imageProperties)
        {
            Bitmap bitmap = GenerateEmptyBitmap(imageProperties.Width, imageProperties.Height, imageProperties.BackgroundColor);

            InitializeComponent();

            CommonConstructor(imageProperties, bitmap);
        }

        public ImageEditor(Bitmap bitmap, string name)
        {
            ImageProperties imageProperties = new ImageProperties(name, bitmap.Width, bitmap.Height, Color.White);

            InitializeComponent();

            CommonConstructor(imageProperties, bitmap);
        }

        #region Supportive methods

        private void CommonConstructor(ImageProperties imageProperties, Bitmap bitmap)
        {
            imageEditorModule.Bitmap = bitmap;
            imageEditorModule.ImageProperties = imageProperties;
            imageEditorModule.ToolbarModule = toolbar;

            imageEditorModule.RefreshImage();
        }

        private Bitmap GenerateEmptyBitmap(int width, int height, Color backgroundColor)
        {
            Bitmap bitmap = new Bitmap(width, height);

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    bitmap.SetPixel(x, y, backgroundColor);
                }
            }

            return bitmap;
        }

        #endregion

        #endregion

        #region Methods

        public bool SaveImage()
        {
            try
            {
                imageEditorModule.Bitmap.Save(SaveLocation, SaveFormat);
            }
            catch
            {
                return false;
            }

            return true;
        }

        #endregion

        #region Event handlers

        private void ImageEditor_GotFocus(object sender, RoutedEventArgs e)
        {
            imageEditorModule.RefreshImage();
        }

        #endregion
    }
}
