# ProjectPilot Lite

Application .NET de gestion de projets techniques — API REST ASP.NET Core + client WPF (MVVM).
Projet réalisé dans le cadre de l'évaluation M1 EADL par Ethan, Kouceyla, Ceredine et Bassma.

## Prérequis

- SDK .NET 8.0 (`dotnet --version` doit afficher 8.x).
- Pour le client WPF : un poste **Windows** (WPF ne compile ni ne s'exécute sur macOS/Linux). Sur Mac, seuls les projets `Core` et `Api` peuvent être compilés et lancés.
- Aucune base de données à installer : SQLite est embarqué (fichier `projectpilot.db` généré automatiquement au premier lancement de l'API).

## Lancer l'API

    cd ProjectPilotLite.Api
    dotnet restore
    dotnet run

L'API démarre sur **http://localhost:5123**. La base SQLite est créée et pré-remplie automatiquement (2 projets de démonstration, quelques tâches et livrables) au premier lancement.

Documentation interactive Swagger (en environnement Development) : http://localhost:5123/swagger

Endpoints principaux :

| Ressource | Méthode | Route |
|---|---|---|
| Projets | GET | `/api/projects`, `/api/projects/{id}` |
| Projets | POST | `/api/projects` |
| Projets | PATCH | `/api/projects/{id}/status` |
| Tâches | GET | `/api/projects/{projectId}/tasks`, `/api/tasks/{id}` |
| Tâches | POST | `/api/projects/{projectId}/tasks` |
| Tâches | PATCH | `/api/tasks/{id}/status` |
| Livrables | GET | `/api/projects/{projectId}/deliverables`, `/api/deliverables/{id}` |
| Livrables | POST | `/api/projects/{projectId}/deliverables` |
| Livrables | PATCH | `/api/deliverables/{id}/status` |

## Lancer le client WPF (poste Windows uniquement)

    cd ProjectPilotLite.Wpf
    dotnet run

Le client WPF appelle l'API à l'adresse `http://localhost:5123` par défaut (voir `Services/ApiService.cs`). L'API doit donc être démarrée avant le client. Aucune configuration supplémentaire n'est nécessaire pour un usage local.

## Structure du projet

    ProjectPilotLite.sln
    ├── ProjectPilotLite.Core    → modèles métier (Projet, Tâche, Livrable) et DTOs partagés
    ├── ProjectPilotLite.Api     → API REST ASP.NET Core, EF Core + SQLite, contrôleurs
    ├── ProjectPilotLite.Wpf     → client WPF (MVVM) : Views, ViewModels, service d'appel API
    └── .github/workflows/ci.yml → pipeline d'intégration continue

## Répartition des rôles

| Membre | Feature |
|---|---|
| Ethan | Projets (API + Core + écrans WPF) |
| Kouceyla | Tâches (API + Core + écrans WPF) |
| Bassma | Livrables (API + Core + écrans WPF) + README |
| Ceredine | Shell WPF / navigation, Tableau de bord, pipeline CI/CD |

## Fonctionnalités réalisées

- [x] Modèle de données Projets / Tâches / Livrables (Core).
- [x] API Projets : liste, détail, création, changement de statut (version de base).
- [x] API Tâches : liste par projet, ajout, changement de statut.
- [x] Persistance SQLite via EF Core, avec jeu de données de démonstration.
- [x] Écrans WPF Tâches (Views + ViewModels, MVVM).

## Éléments non terminés / à faire

- [ ] API + écrans WPF Livrables (Bassma).
- [ ] Écrans WPF Projets (Ethan).
- [ ] Tableau de bord (API + écran WPF) — Ceredine.
- [ ] Shell WPF / navigation entre les écrans (MainWindow) — Ceredine.
- [ ] Pipeline CI/CD (`.github/workflows/ci.yml`) — Ceredine.
- [ ] Note de projection cloud (2-3 pages) — Kouceyla.
- [ ] Capture d'écran du pipeline CI/CD en succès (à ajouter ici une fois disponible).

## Pipeline CI/CD

_À compléter avec un lien ou une capture d'écran du pipeline en succès une fois celui-ci mis en place par Ceredine._
