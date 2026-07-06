namespace ProjectPilotLite.Core.Models;

public enum PrioriteTache
{
    Basse,
    Normale,
    Haute
}

public enum StatutTache
{
    AFaire,
    EnCours,
    Termine
}

/// <summary>
/// Feature "Tâches" — portée par Kouceyla.
/// </summary>
public class Tache
{
    public int Id { get; set; }
    public string Titre { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public PrioriteTache Priorite { get; set; } = PrioriteTache.Normale;
    public StatutTache Statut { get; set; } = StatutTache.AFaire;

    public int ProjetId { get; set; }
    public Projet? Projet { get; set; }
}
