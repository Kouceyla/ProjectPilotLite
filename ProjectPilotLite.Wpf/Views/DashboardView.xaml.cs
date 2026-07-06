using System.Windows.Controls;
using ProjectPilotLite.Wpf.Services;
using ProjectPilotLite.Wpf.ViewModels;

namespace ProjectPilotLite.Wpf.Views;

public partial class DashboardView : UserControl
{
    public DashboardView()
    {
        InitializeComponent();
        DataContext = new DashboardViewModel(new ApiService());
    }
}