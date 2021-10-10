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
using PixelArt_Editor.Data;
using System.IO;
using Microsoft.Win32;
using System.Drawing;
using PixelArt_Editor.Functions;

namespace PixelArt_Editor.GUI.Windows
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public ImageProperties ImageProperties { get; set; }

        public MainWindow()
        {
            InitializeComponent();

            InitialTab initialTab = new InitialTab(this);
            TabItemHeader initialTabHeader = new TabItemHeader(TC_tabs, "Start screen", initialTab);
            TC_tabs.Items.Add(initialTabHeader);
        }

        public void NewPicture()
        {
            PicturePropertiesWindow dialog = new PicturePropertiesWindow(this);

            if ((bool)dialog.ShowDialog())
            {
                ImageEditor editor = new ImageEditor(ImageProperties);

                AddImageEditor(editor, ImageProperties.Name);

                ImageProperties = null;
            }
        }

        public void LoadPicture()
        {
            OpenFileDialog dialog = new OpenFileDialog();
            //Add filter and stuff later

            if ((bool)dialog.ShowDialog())
            {
                Bitmap bitmap = null;

                try
                {
                    bitmap = new Bitmap(dialog.FileName);
                }
                catch
                {
                    MessageBoxes.ShowError("Error during image loading.");
                }

                if (bitmap != null)
                {
                    string name = Functions.IO.GetNameOfFile(dialog.FileName);
                    ImageEditor editor = new ImageEditor(bitmap, name);

                    AddImageEditor(editor, name);
                }
            }
        }

        private void Menu_File_New_Click(object sender, RoutedEventArgs e)
        {
            NewPicture();
        }

        private void Menu_File_Load_Click(object sender, RoutedEventArgs e)
        {
            LoadPicture();
        }

        private void AddImageEditor(ImageEditor editor, string name)
        {
            TabItemHeader header = new TabItemHeader(TC_tabs, name, editor);
            TC_tabs.Items.Add(header);

            TC_tabs.SelectedIndex = TC_tabs.Items.Count - 1;
        }
    }
}
