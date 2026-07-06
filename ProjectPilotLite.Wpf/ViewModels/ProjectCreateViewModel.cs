using ProjectPilotLite.Core.Dtos;
using ProjectPilotLite.Wpf.Common;
using ProjectPilotLite.Wpf.Services;

namespace ProjectPilotLite.Wpf.ViewModels
{
    /// <summary>Écran "Nouveau projet".</summary>
    public class ProjectCreateViewModel : ViewModelBase
    {
        private readonly ProjectsApiService _api;

        private string _nom = string.Empty;
        public string Nom
        {
            get => _nom;
            set => SetField(ref _nom, value);
        }

        private string _description = string.Empty;
        public string Description
        {
            get => _description;
            set => SetField(ref _description, value);
        }

        private string _responsable = string.Empty;
        public string Responsable
        {
            get => _responsable;
            set => SetField(ref _responsable, value);
        }

        private DateTime _dateDebut = DateTime.Today;
        public DateTime DateDebut
        {
            get => _dateDebut;
            set => SetField(ref _dateDebut, value);
        }

        private DateTime _dateLimite = DateTime.Today.AddDays(30);
        public DateTime DateLimite
        {
            get => _dateLimite;
            set => SetField(ref _dateLimite, value);
        }

        private string? _messageErreur;
        public string? MessageErreur
        {
            get => _messageErreur;
            set => SetField(ref _messageErreur, value);
        }

        public ProjetDto? ProjetCree { get; private set; }

        public event EventHandler? ProjetCreeAvecSucces;

        public RelayCommand CreerCommand { get; }

        public ProjectCreateViewModel(ProjectsApiService api)
        {
            _api = api;
            CreerCommand = new RelayCommand(CreerAsync);
        }

        private async Task CreerAsync()
        {
            MessageErreur = null;

            if (string.IsNullOrWhiteSpace(Nom) || string.IsNullOrWhiteSpace(Description) || string.IsNullOrWhiteSpace(Responsable))
            {
                MessageErreur = "Nom, description et responsable sont obligatoires.";
                return;
            }

            if (DateLimite <= DateDebut)
            {
                MessageErreur = "La date limite doit être postérieure à la date de début.";
                return;
            }

            var dto = new ProjetCreateDto
            {
                Nom = Nom.Trim(),
                Description = Description.Trim(),
                Responsable = Responsable.Trim(),
                DateDebut = DateDebut,
                DateLimite = DateLimite
            };

            var resultat = await _api.CreerAsync(dto);

            if (resultat.Succes)
            {
                ProjetCree = resultat.Valeur;
                Nom = string.Empty;
                Description = string.Empty;
                Responsable = string.Empty;
                DateDebut = DateTime.Today;
                DateLimite = DateTime.Today.AddDays(30);
                ProjetCreeAvecSucces?.Invoke(this, EventArgs.Empty);
            }
            else
            {
                MessageErreur = resultat.MessageErreur;
            }
        }
    }
}
