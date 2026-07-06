using System.Windows.Controls;
using ProjectPilotLite.Wpf.ViewModels;

namespace ProjectPilotLite.Wpf.Views
{
    public partial class ProjectDetailView : UserControl
    {
        private readonly ProjectDetailViewModel _viewModel;

        public ProjectDetailView(ProjectDetailViewModel viewModel)
        {
            InitializeComponent();
            _viewModel = viewModel;
            DataContext = viewModel;
        }

        /// <summary>À appeler par l'écran appelant (ex: depuis la liste) avec l'id sélectionné.</summary>
        public Task ChargerProjetAsync(int id) => _viewModel.ChargerAsync(id);
    }
}
