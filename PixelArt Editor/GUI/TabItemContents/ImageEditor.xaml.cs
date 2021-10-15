using PixelArt_Editor.Data;
using PixelArt_Editor.Functions;
using PixelArt_Editor.GUI.Modules;
using System;
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
        public ImageEditorModule EditorModule { get => imageEditorModule; }

        #endregion

        #region Constructors

        public ImageEditor(ImageProperties imageProperties)
        {
            Bitmap bitmap = Bitmaps.GenerateEmptyBitmap(imageProperties.Width, imageProperties.Height, imageProperties.BackgroundColor);

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
            toolbar.MaxLineWidth = Math.Max(bitmap.Height, bitmap.Width);

            imageEditorModule.RerenderImage();
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
            imageEditorModule.Init();
        }

        #endregion
    }
}
