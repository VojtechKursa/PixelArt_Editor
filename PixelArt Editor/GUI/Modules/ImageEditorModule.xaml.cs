using PixelArt_Editor.Data;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using Brush = System.Drawing.Brush;
using Color = System.Drawing.Color;
using Pen = System.Drawing.Pen;
using Point = System.Windows.Point;
using UserControl = System.Windows.Controls.UserControl;

namespace PixelArt_Editor.GUI.Modules
{
    /// <summary>
    /// Interaction logic for ImageEditorModule.xaml
    /// </summary>
    public partial class ImageEditorModule : UserControl
    {
        #region Variables and Properties

        public ToolbarModule ToolbarModule { get; set; } = null;
        private Color Color { get => ToolbarModule.Color; }
        private Tools Tool { get => ToolbarModule.Tool; }

        public bool IsReadOnly { get; set; } = false;

        public ImageProperties ImageProperties { get; set; } = null;
        public Bitmap Bitmap { get; set; } = null;
        private readonly Timer lastResize = new Timer();

        #endregion

        #region Constructors

        public ImageEditorModule()
        {
            InitializeComponent();

            lastResize.Interval = 500;
            lastResize.Enabled = false;
            lastResize.Tick += LastResize_Tick;
        }

        #endregion

        #region Methods

        public void RefreshImage()
        {
            Bitmap exportBitmap = CreateExportBitmap();

            Img_image.Source = BitmapToBitmapImage(exportBitmap);
        }

        private Bitmap CreateExportBitmap()
        {
            if (Img_image.ActualWidth != 0 && Img_image.ActualHeight != 0)
            {
                //Initialize Bitmap and Graphics
                Bitmap exportBitmap = new Bitmap((int)Img_image.ActualWidth, (int)Img_image.ActualHeight);

                Graphics graphics = Graphics.FromImage(exportBitmap);
                graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.None;


                //Calculate the size of pixels
                float pixelWidth = exportBitmap.Width / (float)Bitmap.Width;
                float pixelHeight = exportBitmap.Height / (float)Bitmap.Height;


                //Declare variables for colors and drawing the pixels
                Color pixelColor;
                Brush brush;
                float penThickness = pixelWidth / 20;
                int penAlpha = 255;
                Pen pen = new Pen(Color.FromArgb(penAlpha, Color.Black), penThickness);
                RectangleF pixel = new RectangleF()
                {
                    Size = new SizeF(pixelWidth, pixelHeight)
                };


                //Create list for pixels that need to have their outline re-drawn later
                List<int[]> whiteOutlinedPixels = new List<int[]>();


                //Draw all pixels and add black outline
                for (int y = 0; y < Bitmap.Height; y++)
                {
                    for (int x = 0; x < Bitmap.Width; x++)
                    {
                        pixelColor = Bitmap.GetPixel(x, y);

                        brush = new SolidBrush(pixelColor);

                        if (pixelColor.GetBrightness() < 0.5)
                            whiteOutlinedPixels.Add(new int[] { x, y });

                        pixel.Location = new PointF(x * pixelWidth, y * pixelHeight);

                        graphics.FillRectangle(brush, pixel);
                        graphics.DrawRectangle(pen, pixel.X, pixel.Y, pixel.Width, pixel.Height);
                    }
                }


                //Redraw outline of pixels that need white outline
                pen = new Pen(Color.FromArgb(penAlpha, Color.White), penThickness);

                foreach (int[] coord in whiteOutlinedPixels)
                {
                    pixel.Location = new PointF(coord[0] * pixelWidth, coord[1] * pixelHeight);

                    graphics.DrawRectangle(pen, pixel.X, pixel.Y, pixel.Width, pixel.Height);
                }


                return exportBitmap;
            }
            else
                return Bitmap;
        }

        private BitmapImage BitmapToBitmapImage(Bitmap bitmap)
        {
            MemoryStream memoryStream = new MemoryStream();

            bitmap.Save(memoryStream, ImageFormat.Png);
            memoryStream.Position = 0;

            BitmapImage bitmapImage = new BitmapImage();
            bitmapImage.BeginInit();
            bitmapImage.StreamSource = memoryStream;
            bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
            bitmapImage.EndInit();

            return bitmapImage;
        }

        #endregion

        #region Event handlers

        private void Img_image_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (!IsReadOnly)
            {
                Point clickPoint = e.GetPosition(Img_image);

                int x = (int)Math.Floor((clickPoint.X / (double)Img_image.ActualWidth) * ImageProperties.Width);
                int y = (int)Math.Floor((clickPoint.Y / (double)Img_image.ActualHeight) * ImageProperties.Height);

                if (e.LeftButton == MouseButtonState.Pressed)
                    Bitmap.SetPixel(x, y, Color);
                else if (e.RightButton == MouseButtonState.Pressed)
                    Bitmap.SetPixel(x, y, ImageProperties.BackgroundColor);

                RefreshImage();
            }
        }

        private void Canvas_SizeChanged(object sender, SizeChangedEventArgs e)  //Resize and reposition Img_image
        {
            if (Bitmap != null)
            {
                lastResize.Stop();

                double bitmapRatio = Bitmap.Width / (double)Bitmap.Height;
                double canvasRatio = Canvas.ActualWidth / Canvas.ActualHeight;

                if (bitmapRatio > canvasRatio)    //Limited by width of Canvas
                {
                    Img_image.Width = Canvas.ActualWidth;
                    Img_image.Height = (1 / bitmapRatio) * Img_image.Width;
                }
                else    //Limited by height of Canvas
                {
                    Img_image.Height = Canvas.ActualHeight;
                    Img_image.Width = bitmapRatio * Img_image.Height;
                }

                double horizontalMargin = Canvas.ActualWidth / 2 - Img_image.Width / 2;
                double verticalMargin = Canvas.ActualHeight / 2 - Img_image.Height / 2;
                Img_image.Margin = new Thickness(horizontalMargin, verticalMargin, horizontalMargin, verticalMargin);

                lastResize.Start();
            }
        }

        private void LastResize_Tick(object sender, EventArgs e)
        {
            lastResize.Stop();

            RefreshImage();
        }

        #endregion
    }
}
