using System.Drawing;
using Color = System.Drawing.Color;

namespace PixelArt_Editor.Functions
{
    public static class Bitmaps
    {
        public static Bitmap GenerateEmptyBitmap(int width, int height, Color backgroundColor)
        {
            Bitmap bitmap = new Bitmap(width, height);

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    bitmap.SetPixel(x, y, backgroundColor);
                }
            }

            return bitmap;
        }
    }
}
