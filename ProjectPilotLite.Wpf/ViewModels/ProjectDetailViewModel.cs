using ProjectPilotLite.Core.Dtos;
using ProjectPilotLite.Core.Models;
using ProjectPilotLite.Wpf.Common;
using ProjectPilotLite.Wpf.Services;

namespace ProjectPilotLite.Wpf.ViewModels
{
    /// <summary>Écran "Détail projet" — affichage enrichi + changement de statut.</summary>
    public class ProjectDetailViewModel : ViewModelBase
    {
        private readonly ProjectsApiService _api;

        private ProjetDetailDto? _projet;
        public ProjetDetailDto? Projet
        {
            get => _projet;
            private set => SetField(ref _projet, value);
        }

        public IReadOnlyList<StatutProjet> StatutsDisponibles { get; } =
            Enum.GetValues<StatutProjet>();

        private StatutProjet _statutSelectionne;
        public StatutProjet StatutSelectionne
        {
            get => _statutSelectionne;
            set => SetField(ref _statutSelectionne, value);
        }

        private string? _messageErreur;
        public string? MessageErreur
        {
            get => _messageErreur;
            set => SetField(ref _messageErreur, value);
        }

        private string? _messageConfirmation;
        public string? MessageConfirmation
        {
            get => _messageConfirmation;
            set => SetField(ref _messageConfirmation, value);
        }

        public RelayCommand ChangerStatutCommand { get; }

        public ProjectDetailViewModel(ProjectsApiService api)
        {
            _api = api;
            ChangerStatutCommand = new RelayCommand(ChangerStatutAsync, () => Projet is not null);
        }

        public async Task ChargerAsync(int id)
        {
            MessageErreur = null;
            MessageConfirmation = null;

            var resultat = await _api.ObtenirParIdAsync(id);

            if (resultat.Succes)
            {
                Projet = resultat.Valeur;
                StatutSelectionne = Projet!.Statut;
            }
            else
            {
                MessageErreur = resultat.MessageErreur;
            }
        }

        private async Task ChangerStatutAsync()
        {
            if (Projet is null) return;

            MessageErreur = null;
            MessageConfirmation = null;

            var resultat = await _api.ChangerStatutAsync(Projet.Id, StatutSelectionne);

            if (resultat.Succes)
            {
                // Le PATCH renvoie un ProjetDto (pas le détail enrichi) : on recharge le
                // détail complet pour garder les compteurs Tâches/Livrables à jour.
                await ChargerAsync(Projet.Id);
                MessageConfirmation = "Statut mis à jour.";
            }
            else
            {
                MessageErreur = resultat.MessageErreur;
            }
        }
    }
}
