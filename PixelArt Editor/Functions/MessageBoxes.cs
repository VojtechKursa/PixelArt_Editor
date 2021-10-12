using System.Windows;

namespace PixelArt_Editor.Functions
{
    public static class MessageBoxes
    {
        public static void ShowError(string message)
        {
            MessageBox.Show(message, "Error", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK);
        }
    }
}
