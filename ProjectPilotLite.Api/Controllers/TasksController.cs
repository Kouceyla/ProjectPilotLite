using Microsoft.AspNetCore.Mvc;
using ProjectPilotLite.Api.Data;
using ProjectPilotLite.Core.Dtos;
using ProjectPilotLite.Core.Models;

namespace ProjectPilotLite.Api.Controllers;

/// <summary>
/// Feature "Tâches" — portée par Kouceyla.
/// </summary>
[ApiController]
[Route("api")]
public class TasksController : ControllerBase
{
    private readonly ApplicationDbContext _db;

    public TasksController(ApplicationDbContext db)
    {
        _db = db;
    }

    [HttpGet("projects/{projectId:int}/tasks")]
    public ActionResult<IEnumerable<TacheDto>> GetByProject(int projectId)
    {
        if (!_db.Projets.Any(p => p.Id == projectId))
        {
            return NotFound($"Projet {projectId} introuvable.");
        }

        var taches = _db.Taches
            .Where(t => t.ProjetId == projectId)
            .Select(ToDto)
            .ToList();

        return Ok(taches);
    }

    [HttpGet("tasks/{id:int}")]
    public ActionResult<TacheDto> GetById(int id)
    {
        var tache = _db.Taches.Find(id);
        if (tache is null)
        {
            return NotFound();
        }

        return Ok(ToDto(tache));
    }

    [HttpPost("projects/{projectId:int}/tasks")]
    public ActionResult<TacheDto> Create(int projectId, TacheCreateDto dto)
    {
        var projet = _db.Projets.Find(projectId);
        if (projet is null)
        {
            return NotFound($"Projet {projectId} introuvable.");
        }

        if (string.IsNullOrWhiteSpace(dto.Titre))
        {
            return BadRequest("Le titre de la tâche est obligatoire.");
        }

        var tache = new Tache
        {
            Titre = dto.Titre,
            Description = dto.Description,
            Priorite = dto.Priorite,
            Statut = StatutTache.AFaire,
            ProjetId = projectId
        };

        _db.Taches.Add(tache);
        _db.SaveChanges();

        return CreatedAtAction(nameof(GetById), new { id = tache.Id }, ToDto(tache));
    }

    [HttpPatch("tasks/{id:int}/status")]
    public IActionResult UpdateStatus(int id, TacheStatutUpdateDto dto)
    {
        var tache = _db.Taches.Find(id);
        if (tache is null)
        {
            return NotFound();
        }

        tache.Statut = dto.Statut;
        _db.SaveChanges();

        return NoContent();
    }

    private static TacheDto ToDto(Tache t) => new(
        t.Id, t.Titre, t.Description, t.Priorite, t.Statut, t.ProjetId);
}
