using System.Windows.Controls;
using ProjectPilotLite.Wpf.ViewModels;

namespace ProjectPilotLite.Wpf.Views
{
    public partial class ProjectCreateView : UserControl
    {
        public ProjectCreateView(ProjectCreateViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }
    }
}
