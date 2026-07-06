using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectPilotLite.Api.Data;
using ProjectPilotLite.Core.Dtos;
using ProjectPilotLite.Core.Models;

namespace ProjectPilotLite.Api.Controllers;

/// <summary>
/// Feature "Projets" — portée par Ethan.
/// Complète la version minimale pushée pour débloquer Tâches/Livrables :
///  - validation du nom, de la description, du responsable (DataAnnotations, cf. ProjetCreateDto)
///  - règle métier : DateLimite doit être postérieure à DateDebut (400 sinon)
///  - changement de statut : 400 si statut absent/invalide (EnumDataType), 404 si projet introuvable
///  - détail enrichi : nombre de tâches / livrables et de ceux déjà terminés/validés
/// </summary>
[ApiController]
[Route("api/projects")]
public class ProjectsController : ControllerBase
{
    private readonly ApplicationDbContext _db;

    public ProjectsController(ApplicationDbContext db)
    {
        _db = db;
    }

    // GET /api/projects
    [HttpGet]
    public ActionResult<IEnumerable<ProjetDto>> GetAll()
    {
        var projets = _db.Projets.Select(ToDto).ToList();
        return Ok(projets);
    }

    // GET /api/projects/{id} — détail enrichi (résumé tâches/livrables)
    [HttpGet("{id:int}")]
    public ActionResult<ProjetDetailDto> GetById(int id)
    {
        var projet = _db.Projets
            .Include(p => p.Taches)
            .Include(p => p.Livrables)
            .FirstOrDefault(p => p.Id == id);

        if (projet is null)
        {
            return NotFound(new { message = $"Aucun projet trouvé avec l'id {id}." });
        }

        return Ok(ToDetailDto(projet));
    }

    // POST /api/projects
    [HttpPost]
    public ActionResult<ProjetDto> Create(ProjetCreateDto dto)
    {
        // Avec [ApiController], un ModelState invalide (Nom/Description/Responsable manquants,
        // Nom trop court...) déclenche déjà automatiquement un 400 avant d'arriver ici.
        if (dto.DateLimite <= dto.DateDebut)
        {
            return BadRequest(new { message = "La date limite doit être postérieure à la date de début." });
        }

        var projet = new Projet
        {
            Nom = dto.Nom.Trim(),
            Description = dto.Description.Trim(),
            Responsable = dto.Responsable.Trim(),
            DateDebut = dto.DateDebut,
            DateLimite = dto.DateLimite,
            Statut = StatutProjet.Prevu
        };

        _db.Projets.Add(projet);
        _db.SaveChanges();

        return CreatedAtAction(nameof(GetById), new { id = projet.Id }, ToDto(projet));
    }

    // PATCH /api/projects/{id}/status
    [HttpPatch("{id:int}/status")]
    public ActionResult<ProjetDto> UpdateStatus(int id, ProjetStatutUpdateDto dto)
    {
        // [EnumDataType] + [ApiController] renvoient déjà un 400 si le statut est absent ou
        // hors de l'enum StatutProjet.
        var projet = _db.Projets.Find(id);
        if (projet is null)
        {
            return NotFound(new { message = $"Aucun projet trouvé avec l'id {id}." });
        }

        projet.Statut = dto.Statut;
        _db.SaveChanges();

        return Ok(ToDto(projet));
    }

    private static ProjetDto ToDto(Projet p) => new(
        p.Id, p.Nom, p.Description, p.Responsable, p.DateDebut, p.DateLimite, p.Statut);

    private static ProjetDetailDto ToDetailDto(Projet p) => new(
        p.Id, p.Nom, p.Description, p.Responsable, p.DateDebut, p.DateLimite, p.Statut,
        NombreTaches: p.Taches.Count,
        NombreTachesTerminees: p.Taches.Count(t => t.Statut == StatutTache.Termine),
        NombreLivrables: p.Livrables.Count,
        NombreLivrablesValides: p.Livrables.Count(l => l.Statut == StatutLivrable.Valide));
}
