namespace ProjectPilotLite.Core.Dtos;

public record DashboardDto(
    int TotalProjets,
    int ProjetsEnCours,
    int ProjetsBloques,
    int TotalTaches,
    int TachesTerminees,
    int LivrablesDeposes);