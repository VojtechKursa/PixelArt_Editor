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

        /// <summary>
        /// Gets the path to a file the image is being saved to.
        /// </summary>
        public string SaveLocation { get; set; } = null;

        /// <summary>
        /// Gets the <see cref="ImageFormat"/> the image is being saved to.
        /// </summary>
        public ImageFormat SaveFormat { get; set; } = null;

        /// <summary>
        /// Gets the <see cref="ImageEditorModule"/> inside this <see cref="ImageEditor"/>.
        /// </summary>
        public ImageEditorModule EditorModule { get => imageEditorModule; }

        #endregion

        #region Constructors

        private ImageEditor()
        {
            InitializeComponent();
        }

        public ImageEditor(ImageProperties imageProperties) : this()
        {
            Bitmap bitmap = Bitmaps.GenerateEmptyBitmap(imageProperties.Width, imageProperties.Height, imageProperties.BackgroundColor);

            PostInit(imageProperties, bitmap);
        }

        public ImageEditor(Bitmap bitmap, string name) : this()
        {
            ImageProperties imageProperties = new ImageProperties(name, bitmap.Width, bitmap.Height, Color.White);

            PostInit(imageProperties, bitmap);
        }

        #region Supportive methods

        private void PostInit(ImageProperties imageProperties, Bitmap bitmap)
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

        /// <summary>
        /// Saves the current bitmap (in <see cref="ImageEditorModule.Bitmap"/>) into the selected <see cref="SaveLocation"/> and <see cref="SaveFormat"/>.
        /// </summary>
        /// <returns>True if successful, otherwise false.</returns>
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
