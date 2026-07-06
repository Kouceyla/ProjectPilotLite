using System.Collections.ObjectModel;
using ProjectPilotLite.Core.Dtos;
using ProjectPilotLite.Core.Models;
using ProjectPilotLite.Wpf.Common;
using ProjectPilotLite.Wpf.Services;

namespace ProjectPilotLite.Wpf.ViewModels;

/// <summary>Feature "Tâches" — portée par Kouceyla.</summary>
public class TasksViewModel : ViewModelBase
{
    private readonly ApiService _api;
    private readonly int _projetId;

    public ObservableCollection<TacheDto> Taches { get; } = new();

    private string _nouveauTitre = string.Empty;
    public string NouveauTitre
    {
        get => _nouveauTitre;
        set => SetField(ref _nouveauTitre, value);
    }

    private string _nouvelleDescription = string.Empty;
    public string NouvelleDescription
    {
        get => _nouvelleDescription;
        set => SetField(ref _nouvelleDescription, value);
    }

    public PrioriteTache NouvellePriorite { get; set; } = PrioriteTache.Normale;
    public IEnumerable<PrioriteTache> Priorites => Enum.GetValues<PrioriteTache>();

    private StatutTache _statutCible = StatutTache.EnCours;
    public StatutTache StatutCible
    {
        get => _statutCible;
        set => SetField(ref _statutCible, value);
    }
    public IEnumerable<StatutTache> Statuts => Enum.GetValues<StatutTache>();

    private TacheDto? _tacheSelectionnee;
    public TacheDto? TacheSelectionnee
    {
        get => _tacheSelectionnee;
        set => SetField(ref _tacheSelectionnee, value);
    }

    public RelayCommand AjouterTacheCommand { get; }
    public RelayCommand ChangerStatutCommand { get; }

    public TasksViewModel(ApiService api, int projetId)
    {
        _api = api;
        _projetId = projetId;

        AjouterTacheCommand = new RelayCommand(
            async _ => await AjouterTacheAsync(),
            _ => !string.IsNullOrWhiteSpace(NouveauTitre));

        ChangerStatutCommand = new RelayCommand(
            async _ => await ChangerStatutAsync(),
            _ => TacheSelectionnee != null);

        _ = ChargerTachesAsync();
    }

    public async Task ChargerTachesAsync()
    {
        var taches = await _api.GetListAsync<TacheDto>($"api/projects/{_projetId}/tasks");
        Taches.Clear();
        foreach (var tache in taches)
        {
            Taches.Add(tache);
        }
    }

    private async Task AjouterTacheAsync()
    {
        var dto = new TacheCreateDto(NouveauTitre, NouvelleDescription, NouvellePriorite);
        await _api.PostAsync<TacheDto>($"api/projects/{_projetId}/tasks", dto);

        NouveauTitre = string.Empty;
        NouvelleDescription = string.Empty;
        await ChargerTachesAsync();
    }

    private async Task ChangerStatutAsync()
    {
        if (TacheSelectionnee is null)
        {
            return;
        }

        await _api.PatchAsync($"api/tasks/{TacheSelectionnee.Id}/status", new TacheStatutUpdateDto(StatutCible));
        await ChargerTachesAsync();
    }
}
