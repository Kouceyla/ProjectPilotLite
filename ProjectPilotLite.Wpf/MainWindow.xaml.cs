using System.Windows;
using ProjectPilotLite.Wpf.Services;
using ProjectPilotLite.Wpf.ViewModels;
using ProjectPilotLite.Wpf.Views;

namespace ProjectPilotLite.Wpf;

public partial class MainWindow : Window
{
    private readonly ApiService _api = new();
    private readonly MainWindowViewModel _viewModel;

    public MainWindow()
    {
        InitializeComponent();
        _viewModel = new MainWindowViewModel(_api);
        DataContext = _viewModel;

        DashboardTab.Content = new DashboardView();
        
        // --- LES 3 LIGNES CORRIGÉES SONT ICI ---
        var projectsApiService = new ProjectsApiService();
        var viewModel = new ProjectListViewModel(projectsApiService);
        ProjectsTab.Content = new ProjectListView(viewModel);
        // ---------------------------------------

        _viewModel.ProjetChange += (_, projet) => RefreshProjectTabs(projet?.Id);
    }

    private void RefreshProjectTabs(int? projetId)
    {
        if (projetId is null)
        {
            return;
        }

        TasksTab.Content = new TasksView(projetId.Value);
        DeliverablesTab.Content = new DeliverablesView(projetId.Value);
    }
}