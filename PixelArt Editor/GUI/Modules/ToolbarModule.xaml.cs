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
        #region Variables and Properties

        /// <summary>
        /// Gets the currently selected color.
        /// </summary>
        public Color Color { get => CSM_colors.Color; }

        /// <summary>
        /// Gets the currently selected tool.
        /// </summary>
        public Tools Tool { get => TM_tools.ActiveTool; }

        /// <summary>
        /// Gets the currently selected line width.
        /// </summary>
        public int LineWidth { get => LWM_lineWidth.LineWidth; }

        /// <summary>
        /// Gets or sets the maximum selectable line width.
        /// </summary>
        public int MaxLineWidth { get => LWM_lineWidth.MaxLineWidth; set => LWM_lineWidth.MaxLineWidth = value; }

        #endregion

        public ToolbarModule()
        {
            InitializeComponent();
        }
    }
}
