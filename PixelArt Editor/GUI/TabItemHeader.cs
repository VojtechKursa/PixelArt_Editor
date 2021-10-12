using System.Windows.Controls;

namespace PixelArt_Editor.GUI
{
    public class TabItemHeader : TabItem
    {
        public TabControl TabControl { get; }

        public TabItemHeader(TabControl parent, string header, TabItemContent content)
        {
            TabControl = parent;
            Header = header;
            Content = content;

            content.Header = this;
        }
    }
}
