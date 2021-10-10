using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

            Grid grid = new Grid();
            grid.Children.Add(content);
            Content = grid;

            content.Header = this;
        }
    }
}
