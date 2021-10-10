using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using PixelArt_Editor.GUI.Windows;

namespace PixelArt_Editor.GUI.TabItemContents
{
    /// <summary>
    /// Interaction logic for InitialTab.xaml
    /// </summary>
    public partial class InitialTab : TabItemContent
    {
        MainWindow mainWindow;

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
