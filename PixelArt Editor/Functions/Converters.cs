using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PixelArt_Editor.Functions
{
    public static class Converters
    {
        public static System.Drawing.Color ConvertColor(System.Windows.Media.Color originalColor)
        {
            return System.Drawing.Color.FromArgb(originalColor.A, originalColor.R, originalColor.G, originalColor.B);
        }

        public static System.Windows.Media.Color ConvertColor(System.Drawing.Color originalColor)
        {
            return System.Windows.Media.Color.FromArgb(originalColor.A, originalColor.R, originalColor.G, originalColor.B);
        }
    }
}
