using System.Drawing;

namespace PixelArt_Editor.Data
{
    public enum ImageResizeMode { Centered, BottomRight, Unset }

    public class ImageProperties
    {
        public string Name { get; }
        public int Width { get; }
        public int Height { get; }
        public Color BackgroundColor { get; }
        public ImageResizeMode ResizeMode { get; } = ImageResizeMode.Unset;

        public ImageProperties(string name, int width, int height, Color backgroundColor)
        {
            Name = name;
            Width = width;
            Height = height;
            BackgroundColor = backgroundColor;
        }

        public ImageProperties(string name, int width, int height, Color backgroundColor, ImageResizeMode resizeMode) : this(name, width, height, backgroundColor)
        {
            ResizeMode = resizeMode;
        }
    }
}
