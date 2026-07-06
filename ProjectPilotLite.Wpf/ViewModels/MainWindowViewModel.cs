using System.Collections.ObjectModel;
using ProjectPilotLite.Core.Dtos;
using ProjectPilotLite.Wpf.Common;
using ProjectPilotLite.Wpf.Services;

namespace ProjectPilotLite.Wpf.ViewModels;

/// <summary>Shell / navigation — Ceredine.</summary>
public class MainWindowViewModel : ViewModelBase
{
    private readonly ApiService _api;

    public ObservableCollection<ProjetDto> Projets { get; } = new();

    private ProjetDto? _projetSelectionne;
    public ProjetDto? ProjetSelectionne
    {
        get => _projetSelectionne;
        set
        {
            if (SetField(ref _projetSelectionne, value))
            {
                ProjetChange?.Invoke(this, value);
            }
        }
    }

    public event EventHandler<ProjetDto?>? ProjetChange;

    public MainWindowViewModel(ApiService api)
    {
        _api = api;
        _ = ChargerProjetsAsync();
    }

    public async Task ChargerProjetsAsync()
    {
        var projets = await _api.GetListAsync<ProjetDto>("api/projects");
        Projets.Clear();
        foreach (var p in projets)
        {
            Projets.Add(p);
        }
        ProjetSelectionne = Projets.FirstOrDefault();
    }
}