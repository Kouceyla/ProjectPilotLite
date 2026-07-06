using System.Windows.Controls;
using ProjectPilotLite.Wpf.Services;
using ProjectPilotLite.Wpf.ViewModels;

namespace ProjectPilotLite.Wpf.Views;

public partial class DeliverablesView : UserControl
{
    public DeliverablesView(int projetId)
    {
        InitializeComponent();
        DataContext = new DeliverablesViewModel(new ApiService(), projetId);
    }
}