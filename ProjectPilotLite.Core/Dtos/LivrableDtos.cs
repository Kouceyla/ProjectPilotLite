using ProjectPilotLite.Core.Models;

namespace ProjectPilotLite.Core.Dtos;

// DTOs de la feature "Livrables" — Bassma.
public record LivrableDto(
    int Id,
    string Nom,
    TypeLivrable Type,
    string UrlOuChemin,
    StatutLivrable Statut,
    string Commentaire,
    int ProjetId);

public record LivrableCreateDto(
    string Nom,
    TypeLivrable Type,
    string UrlOuChemin,
    string Commentaire);

public record LivrableStatutUpdateDto(StatutLivrable Statut);