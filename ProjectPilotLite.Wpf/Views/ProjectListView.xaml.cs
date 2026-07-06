using System.Windows.Controls;
using ProjectPilotLite.Wpf.ViewModels;

namespace ProjectPilotLite.Wpf.Views
{
    public partial class ProjectListView : UserControl
    {
        public ProjectListView(ProjectListViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
            Loaded += async (_, _) => await viewModel.ChargerAsync();
        }
    }
}
