﻿namespace PixelArt_Editor.Functions
{
    public static class IO
    {
        public static string GetNameOfFile(string path)
        {
            string[] pathSplit = path.Split('/', '\\');
            return pathSplit[pathSplit.Length - 1];
        }
    }
}
