using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace PixelArt_Editor.Data
{
    public class ImageProperties
    {
        public string Name { get; }
        public int Width { get; }
        public int Height { get; }
        public Color BackgroundColor { get; }

        public ImageProperties(string name, int width, int height, Color backgroundColor)
        {
            Name = name;
            Width = width;
            Height = height;
            BackgroundColor = backgroundColor;
        }
    }
}
