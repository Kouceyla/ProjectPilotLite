using System.ComponentModel.DataAnnotations;
using ProjectPilotLite.Core.Models;

namespace ProjectPilotLite.Core.Dtos;

/// <summary>Représentation de base renvoyée par GET /api/projects et /api/projects/{id}.</summary>
public record ProjetDto(
    int Id,
    string Nom,
    string Description,
    string Responsable,
    DateTime DateDebut,
    DateTime DateLimite,
    StatutProjet Statut);

/// <summary>
/// Détail enrichi (GET /api/projects/{id}) : ajoute un résumé des tâches et livrables liés,
/// utile pour l'écran WPF "Détail projet" sans multiplier les appels API.
/// </summary>
public record ProjetDetailDto(
    int Id,
    string Nom,
    string Description,
    string Responsable,
    DateTime DateDebut,
    DateTime DateLimite,
    StatutProjet Statut,
    int NombreTaches,
    int NombreTachesTerminees,
    int NombreLivrables,
    int NombreLivrablesValides);

/// <summary>Payload pour POST /api/projects.</summary>
public class ProjetCreateDto
{
    [Required(ErrorMessage = "Le nom du projet est obligatoire.")]
    [MinLength(2, ErrorMessage = "Le nom doit contenir au moins 2 caractères.")]
    [MaxLength(200)]
    public string Nom { get; set; } = string.Empty;

    [Required(ErrorMessage = "La description est obligatoire.")]
    public string Description { get; set; } = string.Empty;

    [Required(ErrorMessage = "Le responsable est obligatoire.")]
    public string Responsable { get; set; } = string.Empty;

    public DateTime DateDebut { get; set; } = DateTime.Today;

    public DateTime DateLimite { get; set; } = DateTime.Today.AddDays(30);
}

/// <summary>Payload pour PATCH /api/projects/{id}/status.</summary>
public class ProjetStatutUpdateDto
{
    [Required(ErrorMessage = "Le statut est requis.")]
    [EnumDataType(typeof(StatutProjet), ErrorMessage = "Statut invalide.")]
    public StatutProjet Statut { get; set; }
}
