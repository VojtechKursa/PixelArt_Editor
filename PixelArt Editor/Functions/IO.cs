namespace PixelArt_Editor.Functions
{
    public static class IO
    {
        public static string GetNameOfFile(string path)
        {
            string[] pathSplit = path.Split('/', '\\');
            return pathSplit[pathSplit.Length - 1];
        }

        public static string GetExtension(string path)
        {
            string[] nameSplit = GetNameOfFile(path).Split('.');
            return nameSplit[nameSplit.Length - 1];
        }
    }
}
