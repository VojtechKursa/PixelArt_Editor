using PixelArt_Editor.Data;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace PixelArt_Editor.GUI.Modules
{
    /// <summary>
    /// Interaction logic for ToolsModule.xaml
    /// </summary>
    public partial class ToolsModule : UserControl
    {
        protected Brush UnselectedButtonBrush { get; set; } = new SolidColorBrush(Color.FromRgb(221, 221, 221));
        protected Brush SelectedButtonBrush { get; set; } = new SolidColorBrush(Color.FromRgb(156, 160, 255));
        protected List<Button> ButtonList { get; } = new List<Button>();
        public Tools ActiveTool { get; protected set; } = Tools.Brush;

        public ToolsModule()
        {
            InitializeComponent();

            ButtonList.Add(B_brush);
            ButtonList.Add(B_line);
            ButtonList.Add(B_rectangleEmpty);
            ButtonList.Add(B_rectangleFilled);

            Recolor();
        }

        public void Recolor()
        {
            foreach (Button button in ButtonList)
            {
                button.Background = UnselectedButtonBrush;
            }

            switch (ActiveTool)
            {
                case Tools.Brush: B_brush.Background = SelectedButtonBrush; break;
                case Tools.Line: B_line.Background = SelectedButtonBrush; break;
                case Tools.RectangleEmpty: B_rectangleEmpty.Background = SelectedButtonBrush; break;
                case Tools.RectangleFilled: B_rectangleFilled.Background = SelectedButtonBrush; break;
            }
        }

        private void B_brush_Click(object sender, RoutedEventArgs e)
        {
            ActiveTool = Tools.Brush;

            Recolor();
        }

        private void B_line_Click(object sender, RoutedEventArgs e)
        {
            ActiveTool = Tools.Line;

            Recolor();
        }

        private void B_rectangleEmpty_Click(object sender, RoutedEventArgs e)
        {
            ActiveTool = Tools.RectangleEmpty;

            Recolor();
        }

        private void B_rectangleFilled_Click(object sender, RoutedEventArgs e)
        {
            ActiveTool = Tools.RectangleFilled;

            Recolor();
        }
    }
}
