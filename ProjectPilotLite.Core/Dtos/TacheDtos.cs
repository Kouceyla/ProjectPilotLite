using ProjectPilotLite.Core.Models;

namespace ProjectPilotLite.Core.Dtos;

// DTOs de la feature "Tâches" — Kouceyla.
public record TacheDto(
    int Id,
    string Titre,
    string Description,
    PrioriteTache Priorite,
    StatutTache Statut,
    int ProjetId);

public record TacheCreateDto(
    string Titre,
    string Description,
    PrioriteTache Priorite);

public record TacheStatutUpdateDto(StatutTache Statut);
