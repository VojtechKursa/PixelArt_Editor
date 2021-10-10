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
using PixelArt_Editor.GUI;
using PixelArt_Editor.GUI.TabItemContents;

namespace PixelArt_Editor.GUI.Windows
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            InitialTab initialTab = new InitialTab(this);
            TabItemHeader initialTabHeader = new TabItemHeader(TC_tabs, "Start screen", initialTab);
            TC_tabs.Items.Add(initialTabHeader);
        }

        public void NewPicture()
        {

        }

        public void LoadPicture()
        {

        }
    }
}
