using PixelArt_Editor.Data;
using System.Windows.Controls;
using Color = System.Drawing.Color;

namespace PixelArt_Editor.GUI.Modules
{
    /// <summary>
    /// Interaction logic for ToolbarModule.xaml
    /// </summary>
    public partial class ToolbarModule : UserControl
    {
        public Color Color
        {
            get { return CSM_colors.Color; }
        }

        public Tools Tool { get; }

        public ToolbarModule()
        {
            InitializeComponent();
        }
    }
}
