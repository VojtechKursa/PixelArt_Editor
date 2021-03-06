using PixelArt_Editor.Data;
using PixelArt_Editor.Functions;
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
using DrawingPoint = System.Drawing.Point;
using DrawingSize = System.Drawing.Size;
using MouseEventArgs = System.Windows.Input.MouseEventArgs;
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
        private int LineWidth { get => ToolbarModule.LineWidth; }

        public bool IsReadOnly { get; set; } = false;
        public bool BlackBordersOnly { get; set; } = false;

        public ImageProperties ImageProperties { get; set; } = null;
        public Bitmap Bitmap { get; set; } = null;
        private readonly Timer lastResize = new Timer();

        private int[] dragBeginCoords = null;

        #endregion

        #region Constructors

        public ImageEditorModule()
        {
            InitializeComponent();

            Img_image.Width = 0;
            Img_image.Height = 0;

            lastResize.Interval = 500;
            lastResize.Enabled = false;
            lastResize.Tick += LastResize_Tick;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Initializes the <see cref="ImageEditorModule"/> by resizing the image viewport appropriately and rendering the current <see cref="Bitmap"/>.
        /// </summary>
        public void Init()
        {
            ResizeImageViewport();
            lastResize.Stop();
            RerenderImage();
        }

        /// <summary>
        /// Changes properties of the current image (<see cref="Bitmap"/>).
        /// </summary>
        /// <param name="newProperties">The properties to change to.</param>
        public void ChangeProperties(ImageProperties newProperties)
        {
            if (newProperties.ResizeMode != ImageResizeMode.Unset)
            {
                ToolbarModule.MaxLineWidth = Math.Max(newProperties.Width, newProperties.Height);

                Bitmap newBitmap = Bitmaps.GenerateEmptyBitmap(newProperties.Width, newProperties.Height, newProperties.BackgroundColor);

                if (newProperties.ResizeMode == ImageResizeMode.BottomRight)
                    ResizeBotomRight(Bitmap, newBitmap);
                else if (newProperties.ResizeMode == ImageResizeMode.Centered)
                    ResizeCentered(Bitmap, newBitmap);

                RecolorBackground(newBitmap, ImageProperties.BackgroundColor, newProperties.BackgroundColor);

                Bitmap = newBitmap;
                ImageProperties = newProperties;

                ResizeImageViewport();
                lastResize.Stop();
                RerenderImage();
            }
        }

        private void RecolorBackground(Bitmap bitmap, Color originalBackColor, Color newBackColor)
        {
            if (originalBackColor.ToArgb() != newBackColor.ToArgb())
            {
                int originalColorARGB = originalBackColor.ToArgb();

                for (int y = 0; y < bitmap.Height; y++)
                {
                    for (int x = 0; x < bitmap.Width; x++)
                    {
                        if (bitmap.GetPixel(x, y).ToArgb() == originalColorARGB)
                        {
                            bitmap.SetPixel(x, y, newBackColor);
                        }
                    }
                }
            }
        }

        private void ResizeBotomRight(Bitmap originalBitmap, Bitmap newBitmap)
        {
            int horizontalLimit = newBitmap.Width < originalBitmap.Width ? newBitmap.Width : originalBitmap.Width;
            int verticalLimit = newBitmap.Height < originalBitmap.Height ? newBitmap.Height : originalBitmap.Height;

            for (int y = 0; y < verticalLimit; y++)
            {
                for (int x = 0; x < horizontalLimit; x++)
                {
                    newBitmap.SetPixel(x, y, originalBitmap.GetPixel(x, y));
                }
            }
        }

        private void ResizeCentered(Bitmap originalBitmap, Bitmap newBitmap)
        {
            Thickness margin = CalculateMargin(originalBitmap, newBitmap);

            int horizontalStartPoint = newBitmap.Width < originalBitmap.Width ? 0 : (int)margin.Left;
            int verticalStartPoint = newBitmap.Height < originalBitmap.Height ? 0 : (int)margin.Top;

            int horizontalLimit = Math.Min(originalBitmap.Width, newBitmap.Width) + horizontalStartPoint;
            int verticalLimit = Math.Min(originalBitmap.Height, newBitmap.Height) + verticalStartPoint;

            int horizontalShift = newBitmap.Width < originalBitmap.Width ? (int)margin.Left : -(int)margin.Left;
            int verticalShift = newBitmap.Height < originalBitmap.Height ? (int)margin.Top : -(int)margin.Top; ;

            for (int y = verticalStartPoint; y < verticalLimit; y++)
            {
                for (int x = horizontalStartPoint; x < horizontalLimit; x++)
                {
                    newBitmap.SetPixel(x, y, originalBitmap.GetPixel(x + horizontalShift, y + verticalShift));
                }
            }
        }

        private Thickness CalculateMargin(Bitmap originalBitmap, Bitmap newBitmap)
        {
            int marginHorizontal = Math.Abs(originalBitmap.Width - newBitmap.Width);
            int marginVertical = Math.Abs(originalBitmap.Height - newBitmap.Height);

            int marginLeft;
            int marginRight;
            marginLeft = marginRight = marginHorizontal / 2;
            if (marginHorizontal % 2 != 0)
                marginLeft++;

            int marginTop;
            int marginBottom;
            marginTop = marginBottom = marginVertical / 2;
            if (marginVertical % 2 != 0)
                marginTop++;

            return new Thickness(marginLeft, marginTop, marginRight, marginBottom);
        }

        /// <summary>
        /// Forces rerendering of the current image.
        /// </summary>
        public void RerenderImage()
        {
            Bitmap exportBitmap = CreateExportBitmap();

            Img_image.Source = BitmapToBitmapImage(exportBitmap);
        }

        private Bitmap CreateExportBitmap()
        {
            if (Img_image.Width != 0 && Img_image.Height != 0)
            {
                //Initialize Bitmap and Graphics
                Bitmap exportBitmap = new Bitmap((int)Img_image.Width, (int)Img_image.Height);

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
                if (BlackBordersOnly)
                    pen = new Pen(Color.FromArgb(penAlpha, Color.Black), penThickness);
                else
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

        /// <summary>
        /// Forces resizing of the image viewport.
        /// </summary>
        public void ResizeImageViewport()
        {
            if (Bitmap != null)
            {
                lastResize.Stop();

                double bitmapRatio = Bitmap.Width / (double)Bitmap.Height;
                double canvasRatio = Canvas.ActualWidth / Canvas.ActualHeight;

                double imageWidth;
                double imageHeight;

                if (bitmapRatio > canvasRatio)    //Limited by width of Canvas
                {
                    imageWidth = Canvas.ActualWidth;
                    imageHeight = (1 / bitmapRatio) * imageWidth;
                }
                else    //Limited by height of Canvas
                {
                    imageHeight = Canvas.ActualHeight;
                    imageWidth = bitmapRatio * imageHeight;
                }

                double horizontalMargin = Canvas.ActualWidth / 2 - imageWidth / 2;
                double verticalMargin = Canvas.ActualHeight / 2 - imageHeight / 2;
                Img_image.Margin = new Thickness(horizontalMargin, verticalMargin, horizontalMargin, verticalMargin);
                Img_image.Height = imageHeight;
                Img_image.Width = imageWidth;

                lastResize.Start();
            }
        }

        private int[] GetBitmapClickCoords(MouseEventArgs e)
        {
            Point clickPoint = e.GetPosition(Img_image);

            int x = (int)Math.Floor((clickPoint.X / (double)Img_image.ActualWidth) * ImageProperties.Width);
            int y = (int)Math.Floor((clickPoint.Y / (double)Img_image.ActualHeight) * ImageProperties.Height);

            return new int[] { x, y };
        }

        #endregion

        #region Event handlers

        private void Img_image_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (!IsReadOnly)
            {
                int[] clickCoords = GetBitmapClickCoords(e);

                if (Tool == Tools.Brush)
                {
                    if (e.LeftButton == MouseButtonState.Pressed)
                        Bitmap.SetPixel(clickCoords[0], clickCoords[1], Color);
                    else if (e.RightButton == MouseButtonState.Pressed)
                        Bitmap.SetPixel(clickCoords[0], clickCoords[1], ImageProperties.BackgroundColor);
                }
                else
                {
                    dragBeginCoords = clickCoords;
                }

                RerenderImage();
            }
        }

        private void Img_image_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (!IsReadOnly)
            {
                if (Tool != Tools.Brush)
                {
                    Color appliedColor = e.ChangedButton.Equals(MouseButton.Right) ? ImageProperties.BackgroundColor : Color;

                    int[] dragReleaseCoords = GetBitmapClickCoords(e);

                    Graphics graphics = Graphics.FromImage(Bitmap);

                    if (Tool == Tools.Line)
                    {
                        Pen pen = new Pen(appliedColor, LineWidth);

                        graphics.DrawLine(pen, dragBeginCoords[0], dragBeginCoords[1], dragReleaseCoords[0], dragReleaseCoords[1]);
                    }
                    else if (Tool == Tools.RectangleEmpty || Tool == Tools.RectangleFilled)
                    {
                        DrawingPoint anchor = new DrawingPoint(Math.Min(dragBeginCoords[0], dragReleaseCoords[0]), Math.Min(dragBeginCoords[1], dragReleaseCoords[1]));
                        DrawingSize size = new DrawingSize(Math.Abs(dragBeginCoords[0] - dragReleaseCoords[0]), Math.Abs(dragBeginCoords[1] - dragReleaseCoords[1]));

                        Rectangle rectangle = new Rectangle(anchor, size);

                        if (Tool == Tools.RectangleEmpty)
                        {
                            Pen pen = new Pen(appliedColor, LineWidth);

                            graphics.DrawRectangle(pen, rectangle.X, rectangle.Y, rectangle.Width, rectangle.Height);
                        }
                        else if (Tool == Tools.RectangleFilled)
                        {
                            Brush brush = new SolidBrush(appliedColor);

                            graphics.FillRectangle(brush, rectangle.X, rectangle.Y, rectangle.Width + 1, rectangle.Height + 1);
                        }
                    }

                    RerenderImage();
                }
            }
        }

        private void Img_image_MouseMove(object sender, MouseEventArgs e)
        {
            if (!IsReadOnly)
            {
                if (Tool == Tools.Brush)
                {
                    bool rerender = false;

                    int[] clickCoords = GetBitmapClickCoords(e);
                    int clickedPixelColor = Bitmap.GetPixel(clickCoords[0], clickCoords[1]).ToArgb();

                    if (e.LeftButton == MouseButtonState.Pressed)
                    {
                        if (clickedPixelColor != Color.ToArgb())
                        {
                            Bitmap.SetPixel(clickCoords[0], clickCoords[1], Color);
                            rerender = true;
                        }
                    }
                    else if (e.RightButton == MouseButtonState.Pressed)
                    {
                        if (clickedPixelColor != ImageProperties.BackgroundColor.ToArgb())
                        {
                            Bitmap.SetPixel(clickCoords[0], clickCoords[1], ImageProperties.BackgroundColor);
                            rerender = true;
                        }
                    }

                    if (rerender)
                        RerenderImage();
                }
            }
        }

        private void Canvas_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            ResizeImageViewport();
        }

        private void LastResize_Tick(object sender, EventArgs e)
        {
            lastResize.Stop();

            RerenderImage();
        }

        #endregion
    }
}
