using ProjectPilotLite.Core.Models;

namespace ProjectPilotLite.Api.Data;

/// <summary>
/// Jeu de données de démarrage : permet de tester tout de suite les endpoints
/// Tâches / Livrables sans attendre que la feature Projets (Ethan) soit terminée.
/// </summary>
public static class SeedData
{
    public static void Initialize(ApplicationDbContext db)
    {
        if (db.Projets.Any())
        {
            return; // déjà initialisé
        }

        var projet1 = new Projet
        {
            Nom = "Refonte intranet",
            Description = "Migration de l'intranet vers une stack .NET moderne.",
            Responsable = "Alice Martin",
            DateDebut = DateTime.Today.AddDays(-10),
            DateLimite = DateTime.Today.AddDays(30),
            Statut = StatutProjet.EnCours
        };

        var projet2 = new Projet
        {
            Nom = "Migration base de données",
            Description = "Passage de l'application legacy vers SQLite/EF Core.",
            Responsable = "Karim Benzarti",
            DateDebut = DateTime.Today.AddDays(-3),
            DateLimite = DateTime.Today.AddDays(45),
            Statut = StatutProjet.Prevu
        };

        db.Projets.AddRange(projet1, projet2);
        db.SaveChanges();

        db.Taches.AddRange(
            new Tache { Titre = "Maquettes UI", Description = "Créer les maquettes des écrans principaux", Priorite = PrioriteTache.Haute, Statut = StatutTache.Termine, ProjetId = projet1.Id },
            new Tache { Titre = "Endpoint /api/projects", Description = "Exposer la liste des projets", Priorite = PrioriteTache.Haute, Statut = StatutTache.EnCours, ProjetId = projet1.Id },
            new Tache { Titre = "Écran WPF Tâches", Description = "Lister et modifier le statut des tâches", Priorite = PrioriteTache.Normale, Statut = StatutTache.AFaire, ProjetId = projet1.Id }
        );

        db.Livrables.AddRange(
            new Livrable { Nom = "Cahier des charges", Type = TypeLivrable.Documentation, UrlOuChemin = "docs/cahier-des-charges.pdf", Statut = StatutLivrable.Valide, Commentaire = "Version validée en réunion de cadrage.", ProjetId = projet1.Id },
            new Livrable { Nom = "Prototype API", Type = TypeLivrable.Code, UrlOuChemin = "https://github.com/groupe/projectpilotlite", Statut = StatutLivrable.Depose, Commentaire = "", ProjetId = projet2.Id }
        );

        db.SaveChanges();
    }
}
