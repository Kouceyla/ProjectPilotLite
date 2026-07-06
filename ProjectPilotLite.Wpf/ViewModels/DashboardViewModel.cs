using ProjectPilotLite.Core.Dtos;
using ProjectPilotLite.Wpf.Common;
using ProjectPilotLite.Wpf.Services;

namespace ProjectPilotLite.Wpf.ViewModels;

public class DashboardViewModel : ViewModelBase
{
    private readonly ApiService _api;
    private DashboardDto _stats = new(0, 0, 0, 0, 0, 0);
    public DashboardDto Stats
    {
        get => _stats;
        set => SetField(ref _stats, value);
    }

    public RelayCommand RafraichirCommand { get; }

    public DashboardViewModel(ApiService api)
    {
        _api = api;
        RafraichirCommand = new RelayCommand(async _ => await ChargerAsync());
        _ = ChargerAsync();
    }

    private async Task ChargerAsync()
    {
        var stats = await _api.GetAsync<DashboardDto>("api/dashboard");
        if (stats is not null)
        {
            Stats = stats;
        }
    }
}