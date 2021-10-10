using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using PixelArt_Editor.Data;
using Color = System.Drawing.Color;
using System.IO;
using System.Drawing.Imaging;
using Point = System.Windows.Point;
using Brush = System.Drawing.Brush;
using Pen = System.Drawing.Pen;
using Rectangle = System.Drawing.Rectangle;

namespace PixelArt_Editor.GUI.TabItemContents
{
    /// <summary>
    /// Interaction logic for ImageEditor.xaml
    /// </summary>
    public partial class ImageEditor : TabItemContent
    {
        ImageProperties imageProperties;
        Bitmap bitmap;
        Color currentColor = Color.Black;

        public ImageEditor(ImageProperties imageProperties)
        {
            this.imageProperties = imageProperties;
            bitmap = GenerateEmptyBitmap(imageProperties.Width, imageProperties.Height, imageProperties.BackgroundColor);

            InitializeComponent();

            RefreshImage();
        }

        public ImageEditor(Bitmap bitmap, string name)
        {
            imageProperties = new ImageProperties(name, bitmap.Width, bitmap.Height, Color.White);
            this.bitmap = bitmap;

            InitializeComponent();

            RefreshImage();
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

        private void RefreshImage()
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


                //Declare variables for colors and drawing the pixels
                Color pixelColor;
                Brush brush;
                int penThickness = 3;
                Pen pen = new Pen(Color.Black, penThickness);
                RectangleF pixel = new RectangleF();


                //Calculate the size of the pixels
                float pixelWidth = exportBitmap.Width / (float)bitmap.Width;
                float pixelHeight = exportBitmap.Height / (float)bitmap.Height;

                pixel.Size = new SizeF(pixelWidth, pixelHeight);


                //Create list for pixels that need to have their outline re-drawn later
                List<int[]> whiteOutlinedPixels = new List<int[]>();


                //Draw all pixels and add black outline
                for (int y = 0; y < bitmap.Height; y++)
                {
                    for (int x = 0; x < bitmap.Width; x++)
                    {
                        pixelColor = bitmap.GetPixel(x, y);

                        brush = new SolidBrush(pixelColor);

                        if (pixelColor.GetBrightness() < 0.5)
                            whiteOutlinedPixels.Add(new int[] { x, y });

                        pixel.Location = new PointF(x * pixelWidth, y * pixelHeight);

                        graphics.FillRectangle(brush, pixel);
                        graphics.DrawRectangle(pen, pixel.X, pixel.Y, pixel.Width, pixel.Height);
                    }
                }


                //Redraw outline of pixels that need white outline
                pen = new Pen(Color.White, penThickness);

                foreach (int[] coord in whiteOutlinedPixels)
                {
                    pixel.Location = new PointF(coord[0] * pixelWidth, coord[1] * pixelHeight);

                    graphics.DrawRectangle(pen, pixel.X, pixel.Y, pixel.Width, pixel.Height);
                }


                return exportBitmap;
            }
            else
                return bitmap;
        }

        private BitmapImage BitmapToBitmapImage(Bitmap bitmap)
        {
            MemoryStream memoryStream = new MemoryStream();

            bitmap.Save(memoryStream, ImageFormat.Bmp);
            memoryStream.Position = 0;

            BitmapImage bitmapImage = new BitmapImage();
            bitmapImage.BeginInit();
            bitmapImage.StreamSource = memoryStream;
            bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
            bitmapImage.EndInit();

            return bitmapImage;
        }


        private void Img_image_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Point clickPoint = e.GetPosition(Img_image);

            int x = (int)Math.Floor((clickPoint.X / (double)Img_image.ActualWidth) * imageProperties.Width);
            int y = (int)Math.Floor((clickPoint.Y / (double)Img_image.ActualHeight) * imageProperties.Height);

            bitmap.SetPixel(x, y, currentColor);

            RefreshImage();
        }

        private void Canvas_SizeChanged(object sender, SizeChangedEventArgs e)  //Resize and reposition Img_image
        {
            double bitmapRatio = bitmap.Width / (double)bitmap.Height;
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
        }

        private void ImageEditor_GotFocus(object sender, RoutedEventArgs e)
        {
            RefreshImage();
        }
    }
}
