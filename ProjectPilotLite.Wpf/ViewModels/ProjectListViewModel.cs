using System.Collections.ObjectModel;
using ProjectPilotLite.Core.Dtos;
using ProjectPilotLite.Wpf.Common;
using ProjectPilotLite.Wpf.Services;

namespace ProjectPilotLite.Wpf.ViewModels
{
    /// <summary>Écran "Liste des projets".</summary>
    public class ProjectListViewModel : ViewModelBase
    {
        private readonly ProjectsApiService _api;

        public ObservableCollection<ProjetDto> Projets { get; } = new();

        private ProjetDto? _projetSelectionne;
        public ProjetDto? ProjetSelectionne
        {
            get => _projetSelectionne;
            set => SetField(ref _projetSelectionne, value);
        }

        private string? _messageErreur;
        public string? MessageErreur
        {
            get => _messageErreur;
            set => SetField(ref _messageErreur, value);
        }

        private bool _chargementEnCours;
        public bool ChargementEnCours
        {
            get => _chargementEnCours;
            set => SetField(ref _chargementEnCours, value);
        }

        public RelayCommand ChargerCommand { get; }

        public ProjectListViewModel(ProjectsApiService api)
        {
            _api = api;
            ChargerCommand = new RelayCommand(ChargerAsync);
        }

        public async Task ChargerAsync()
        {
            ChargementEnCours = true;
            MessageErreur = null;

            var resultat = await _api.ObtenirTousAsync();

            if (resultat.Succes)
            {
                Projets.Clear();
                foreach (var projet in resultat.Valeur!)
                {
                    Projets.Add(projet);
                }
            }
            else
            {
                MessageErreur = resultat.MessageErreur;
            }

            ChargementEnCours = false;
        }
    }
}
