using Microsoft.Win32;
using PixelArt_Editor.Data;
using PixelArt_Editor.Functions;
using PixelArt_Editor.GUI.TabItemContents;
using System.Drawing;
using System.Windows;
using ImageFormat = System.Drawing.Imaging.ImageFormat;

namespace PixelArt_Editor.GUI.Windows
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Variables and Properties

        public ImageProperties ImageProperties { get; set; }

        #endregion

        #region Constructors

        public MainWindow()
        {
            InitializeComponent();

            InitialTab initialTab = new InitialTab(this);
            TabItemHeader initialTabHeader = new TabItemHeader(TC_tabs, "Start screen", initialTab);
            TC_tabs.Items.Add(initialTabHeader);
        }

        #endregion

        #region Methods

        #region Picture handing methods

        public void NewPicture()
        {
            ImagePropertiesWindow dialog = new ImagePropertiesWindow(this);

            if ((bool)dialog.ShowDialog())
            {
                ImageEditor editor = new ImageEditor(ImageProperties);

                AddImageEditor(editor, ImageProperties.Name);

                ImageProperties = null;
            }
        }

        public void LoadPicture()
        {
            OpenFileDialog dialog = new OpenFileDialog()
            {
                CheckFileExists = true,
                DereferenceLinks = true,
                Multiselect = false,
                Title = "Select a file to load...",
                Filter = "PNG (*.png)|*.png|JPEG (*.jpg; *.jpeg)|*.jpg;*.jpeg|Bitmap (*.bmp)|*.bmp|GIF (*.gif)|*.gif|ICON (*.icon)|*.icon|TIFF (*.tiff; *.tif)|*.tiff;*.tif|All files (*.*)|*.*"
            };

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
                    string name = IO.GetNameOfFile(dialog.FileName);
                    ImageEditor editor = new ImageEditor(bitmap, name);
                    editor.SaveLocation = dialog.FileName;
                    editor.SaveFormat = DetermineImageFormat(name);

                    AddImageEditor(editor, name);
                }
            }
        }

        private void Save()
        {
            ImageEditor selectedImageEditor = GetSelectedImageEditor();

            if (selectedImageEditor != null)
            {
                if (selectedImageEditor.SaveLocation == null || selectedImageEditor.SaveFormat == null)
                    SaveAs();
                else
                {
                    if (!selectedImageEditor.SaveImage())
                        MessageBoxes.ShowError("Saving the image was unsuccessful");
                }
            }
        }

        private void SaveAs()
        {
            ImageEditor selectedImageEditor = GetSelectedImageEditor();

            if (selectedImageEditor != null)
            {
                SaveFileDialog dialog = new SaveFileDialog()
                {
                    AddExtension = true,
                    CreatePrompt = false,
                    OverwritePrompt = true,
                    Filter = "PNG (*.png)|*.png|JPEG (*.jpg; *.jpeg)|*.jpg;*.jpeg|Bitmap (*.bmp)|*.bmp|GIF (*.gif)|*.gif|ICON (*.icon)|*.icon|TIFF (*.tiff; *.tif)|*.tiff;*.tif",
                    ValidateNames = true,
                    Title = "Select a save location..."
                };

                if ((bool)dialog.ShowDialog())
                {
                    selectedImageEditor.SaveLocation = dialog.FileName;
                    selectedImageEditor.Header.Header = IO.GetNameOfFile(dialog.FileName);

                    switch (dialog.FilterIndex)
                    {
                        case 1: selectedImageEditor.SaveFormat = ImageFormat.Png; break;
                        case 2: selectedImageEditor.SaveFormat = ImageFormat.Jpeg; break;
                        case 3: selectedImageEditor.SaveFormat = ImageFormat.Bmp; break;
                        case 4: selectedImageEditor.SaveFormat = ImageFormat.Gif; break;
                        case 5: selectedImageEditor.SaveFormat = ImageFormat.Icon; break;
                        case 6: selectedImageEditor.SaveFormat = ImageFormat.Tiff; break;
                        default: selectedImageEditor.SaveFormat = ImageFormat.Png; break;
                    }

                    Save();
                }
            }
        }

        #endregion

        #region Supportive methods

        private ImageEditor GetSelectedImageEditor()
        {
            if (TC_tabs.SelectedIndex != -1)
            {
                TabItemContent selectedTab = (TabItemContent)((TabItemHeader)TC_tabs.SelectedItem).Content;

                if (selectedTab is ImageEditor editor)
                    return editor;
            }

            return null;
        }

        private ImageFormat DetermineImageFormat(string filename)
        {
            switch (IO.GetExtension(filename).ToLower())
            {
                case "png": return ImageFormat.Png;
                case "jpg":
                case "jpeg": return ImageFormat.Jpeg;
                case "bmp": return ImageFormat.Bmp;
                case "gif": return ImageFormat.Gif;
                case "icon": return ImageFormat.Icon;
                case "tiff":
                case "tif": return ImageFormat.Tiff;
                default: return null;
            }
        }

        private void AddImageEditor(ImageEditor editor, string name)
        {
            TabItemHeader header = new TabItemHeader(TC_tabs, name, editor);
            TC_tabs.Items.Add(header);

            TC_tabs.SelectedIndex = TC_tabs.Items.Count - 1;
        }

        #endregion

        #endregion

        #region Event handlers

        private void Menu_File_New_Click(object sender, RoutedEventArgs e)
        {
            NewPicture();
        }

        private void Menu_File_Load_Click(object sender, RoutedEventArgs e)
        {
            LoadPicture();
        }

        private void Menu_File_Save_Click(object sender, RoutedEventArgs e)
        {
            Save();
        }

        private void Menu_File_SaveAs_Click(object sender, RoutedEventArgs e)
        {
            SaveAs();
        }

        private void Menu_File_CloseProgram_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Menu_Image_Properties_Click(object sender, RoutedEventArgs e)
        {
            ImageEditor editor = GetSelectedImageEditor();

            if (editor != null)
            {
                ImagePropertiesWindow window = new ImagePropertiesWindow(this, editor.EditorModule.ImageProperties);

                if ((bool)window.ShowDialog())
                {
                    editor.EditorModule.ChangeProperties(ImageProperties);

                    editor.Header.Header = ImageProperties.Name;

                    ImageProperties = null;
                }
            }
        }

        private void TC_tabs_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (GetSelectedImageEditor() != null)
            {
                Menu_File_Save.IsEnabled = true;
                Menu_File_SaveAs.IsEnabled = true;
                Menu_Image.IsEnabled = true;
            }
            else
            {
                Menu_File_Save.IsEnabled = false;
                Menu_File_SaveAs.IsEnabled = false;
                Menu_Image.IsEnabled = false;
            }
        }

        #endregion
    }
}
