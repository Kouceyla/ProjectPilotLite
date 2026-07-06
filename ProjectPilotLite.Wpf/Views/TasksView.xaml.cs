using System.Windows.Controls;
using ProjectPilotLite.Wpf.Services;
using ProjectPilotLite.Wpf.ViewModels;

namespace ProjectPilotLite.Wpf.Views;

/// <summary>
/// Pas de logique métier ni d'appel réseau ici (contrainte MVVM de l'énoncé) :
/// tout est délégué au TasksViewModel.
/// </summary>
public partial class TasksView : UserControl
{
    public TasksView(int projetId)
    {
        InitializeComponent();
        DataContext = new TasksViewModel(new ApiService(), projetId);
    }
}
