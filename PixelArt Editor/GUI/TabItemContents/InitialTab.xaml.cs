using PixelArt_Editor.GUI.Windows;
using System.Windows;

namespace PixelArt_Editor.GUI.TabItemContents
{
    /// <summary>
    /// Interaction logic for InitialTab.xaml
    /// </summary>
    public partial class InitialTab : TabItemContent
    {
        private readonly MainWindow mainWindow;

        public InitialTab(MainWindow mainWindow)
        {
            InitializeComponent();

            this.mainWindow = mainWindow;
        }

        private void B_new_Click(object sender, RoutedEventArgs e)
        {
            mainWindow.NewPicture();
        }

        private void B_load_Click(object sender, RoutedEventArgs e)
        {
            mainWindow.LoadPicture();
        }
    }
}
