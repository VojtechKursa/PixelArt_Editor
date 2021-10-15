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
        public Color Color { get => CSM_colors.Color; }
        public Tools Tool { get => TM_tools.ActiveTool; }
        public int LineWidth { get => LWM_lineWidth.LineWidth; }
        public int MaxLineWidth { get => LWM_lineWidth.MaxLineWidth; set => LWM_lineWidth.MaxLineWidth = value; }

        public ToolbarModule()
        {
            InitializeComponent();
        }
    }
}
