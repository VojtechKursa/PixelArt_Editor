using PixelArt_Editor.Data;
using PixelArt_Editor.GUI.Windows;
using System;
using System.Windows;
using Bitmap = System.Drawing.Bitmap;
using Color = System.Drawing.Color;
using Timer = System.Windows.Forms.Timer;

namespace PixelArt_Editor.GUI.TabItemContents
{
    /// <summary>
    /// Interaction logic for InitialTab.xaml
    /// </summary>
    public partial class InitialTab : TabItemContent
    {
        #region Variables

        private readonly Timer timer = new Timer()
        {
            Enabled = false,
            Interval = 1000
        };
        private readonly Random random = new Random();
        private readonly MainWindow mainWindow;

        #endregion

        #region Constructors

        public InitialTab(MainWindow mainWindow)
        {
            InitializeComponent();

            this.mainWindow = mainWindow;

            imageEditor.ImageProperties = new ImageProperties("Startup screen thingy", 3, 3, Color.Transparent);
            imageEditor.IsReadOnly = true;
            imageEditor.BlackBordersOnly = true;

            GenerateNewImage();

            timer.Tick += Timer_Tick;
            timer.Start();
        }

        #endregion

        #region Methods

        private void GenerateNewImage()
        {
            Bitmap bitmap = new Bitmap(3, 3);
            Color[] colors = new Color[] { Color.White, Color.Yellow, Color.Orange, Color.Blue, Color.Lime, Color.Red };

            for (int y = 0; y < bitmap.Height; y++)
            {
                for (int x = 0; x < bitmap.Width; x++)
                {
                    bitmap.SetPixel(x, y, colors[random.Next(0, colors.Length)]);
                }
            }

            imageEditor.Bitmap = bitmap;
            imageEditor.RerenderImage();
        }

        #endregion

        #region Event handlers

        private void B_new_Click(object sender, RoutedEventArgs e)
        {
            mainWindow.NewPicture();
        }

        private void B_load_Click(object sender, RoutedEventArgs e)
        {
            mainWindow.LoadPicture();
        }

        private void InitialTab_GotFocus(object sender, RoutedEventArgs e)
        {
            GenerateNewImage();
            timer.Start();
        }

        private void InitialTab_LostFocus(object sender, RoutedEventArgs e)
        {
            timer.Stop();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            GenerateNewImage();
        }

        #endregion
    }
}
