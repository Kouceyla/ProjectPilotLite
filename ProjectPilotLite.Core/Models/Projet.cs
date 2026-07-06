namespace ProjectPilotLite.Core.Models;

public enum StatutProjet
{
    Prevu,
    EnCours,
    Termine,
    Bloque
}

/// <summary>
/// Feature "Projets" — portée par Ethan. Modèle minimal présent ici
/// car les Tâches et les Livrables ont une relation obligatoire vers un Projet.
/// </summary>
public class Projet
{
    public int Id { get; set; }
    public string Nom { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Responsable { get; set; } = string.Empty;
    public DateTime DateDebut { get; set; }
    public DateTime DateLimite { get; set; }
    public StatutProjet Statut { get; set; } = StatutProjet.Prevu;

    public List<Tache> Taches { get; set; } = new();
    public List<Livrable> Livrables { get; set; } = new();
}
