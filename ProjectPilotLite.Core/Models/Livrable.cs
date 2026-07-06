namespace ProjectPilotLite.Core.Models;

public enum TypeLivrable
{
    Code,
    Documentation,
    Presentation,
    Autre
}

public enum StatutLivrable
{
    Depose,
    Valide,
    Refuse
}

/// <summary>
/// Feature "Livrables" — portée par Bassma.
/// </summary>
public class Livrable
{
    public int Id { get; set; }
    public string Nom { get; set; } = string.Empty;
    public TypeLivrable Type { get; set; } = TypeLivrable.Code;
    public string UrlOuChemin { get; set; } = string.Empty;
    public StatutLivrable Statut { get; set; } = StatutLivrable.Depose;
    public string Commentaire { get; set; } = string.Empty;

    public int ProjetId { get; set; }
    public Projet? Projet { get; set; }
}
