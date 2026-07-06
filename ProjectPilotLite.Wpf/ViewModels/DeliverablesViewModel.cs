using System.Collections.ObjectModel;
using ProjectPilotLite.Core.Dtos;
using ProjectPilotLite.Core.Models;
using ProjectPilotLite.Wpf.Common;
using ProjectPilotLite.Wpf.Services;

namespace ProjectPilotLite.Wpf.ViewModels;

/// <summary>Feature "Livrables" — portée par Bassma.</summary>
public class DeliverablesViewModel : ViewModelBase
{
    private readonly ApiService _api;
    private readonly int _projetId;

    public ObservableCollection<LivrableDto> Livrables { get; } = new();

    private string _nouveauNom = string.Empty;
    public string NouveauNom
    {
        get => _nouveauNom;
        set => SetField(ref _nouveauNom, value);
    }

    private string _nouvelleUrl = string.Empty;
    public string NouvelleUrl
    {
        get => _nouvelleUrl;
        set => SetField(ref _nouvelleUrl, value);
    }

    private string _nouveauCommentaire = string.Empty;
    public string NouveauCommentaire
    {
        get => _nouveauCommentaire;
        set => SetField(ref _nouveauCommentaire, value);
    }

    public TypeLivrable NouveauType { get; set; } = TypeLivrable.Code;
    public IEnumerable<TypeLivrable> Types => Enum.GetValues<TypeLivrable>();

    private LivrableDto? _livrableSelectionne;
    public LivrableDto? LivrableSelectionne
    {
        get => _livrableSelectionne;
        set => SetField(ref _livrableSelectionne, value);
    }

    public RelayCommand AjouterLivrableCommand { get; }
    public RelayCommand ValiderCommand { get; }
    public RelayCommand RefuserCommand { get; }

    public DeliverablesViewModel(ApiService api, int projetId)
    {
        _api = api;
        _projetId = projetId;

        AjouterLivrableCommand = new RelayCommand(
            async _ => await AjouterLivrableAsync(),
            _ => !string.IsNullOrWhiteSpace(NouveauNom));

        ValiderCommand = new RelayCommand(
            async _ => await ChangerStatutAsync(StatutLivrable.Valide),
            _ => LivrableSelectionne != null);

        RefuserCommand = new RelayCommand(
            async _ => await ChangerStatutAsync(StatutLivrable.Refuse),
            _ => LivrableSelectionne != null);

        _ = ChargerLivrablesAsync();
    }

    public async Task ChargerLivrablesAsync()
    {
        var livrables = await _api.GetListAsync<LivrableDto>($"api/projects/{_projetId}/deliverables");
        Livrables.Clear();
        foreach (var livrable in livrables)
        {
            Livrables.Add(livrable);
        }
    }

    private async Task AjouterLivrableAsync()
    {
        var dto = new LivrableCreateDto(NouveauNom, NouveauType, NouvelleUrl, NouveauCommentaire);
        await _api.PostAsync<LivrableDto>($"api/projects/{_projetId}/deliverables", dto);

        NouveauNom = string.Empty;
        NouvelleUrl = string.Empty;
        NouveauCommentaire = string.Empty;
        await ChargerLivrablesAsync();
    }

    private async Task ChangerStatutAsync(StatutLivrable statut)
    {
        if (LivrableSelectionne is null) return;
        await _api.PatchAsync($"api/deliverables/{LivrableSelectionne.Id}/status", new LivrableStatutUpdateDto(statut));
        await ChargerLivrablesAsync();
    }
}