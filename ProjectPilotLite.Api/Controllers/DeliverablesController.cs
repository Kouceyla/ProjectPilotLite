using Microsoft.AspNetCore.Mvc;
using ProjectPilotLite.Api.Data;
using ProjectPilotLite.Core.Dtos;
using ProjectPilotLite.Core.Models;

namespace ProjectPilotLite.Api.Controllers;

/// <summary>
/// Feature "Livrables" — portée par Bassma.
/// </summary>
[ApiController]
[Route("api")]
public class DeliverablesController : ControllerBase
{
    private readonly ApplicationDbContext _db;

    public DeliverablesController(ApplicationDbContext db)
    {
        _db = db;
    }

    [HttpGet("projects/{projectId:int}/deliverables")]
    public ActionResult<IEnumerable<LivrableDto>> GetByProject(int projectId)
    {
        if (!_db.Projets.Any(p => p.Id == projectId))
        {
            return NotFound($"Projet {projectId} introuvable.");
        }

        var livrables = _db.Livrables
            .Where(l => l.ProjetId == projectId)
            .Select(ToDto)
            .ToList();

        return Ok(livrables);
    }

    [HttpGet("deliverables/{id:int}")]
    public ActionResult<LivrableDto> GetById(int id)
    {
        var livrable = _db.Livrables.Find(id);
        if (livrable is null)
        {
            return NotFound();
        }

        return Ok(ToDto(livrable));
    }

    [HttpPost("projects/{projectId:int}/deliverables")]
    public ActionResult<LivrableDto> Create(int projectId, LivrableCreateDto dto)
    {
        var projet = _db.Projets.Find(projectId);
        if (projet is null)
        {
            return NotFound($"Projet {projectId} introuvable.");
        }

        if (string.IsNullOrWhiteSpace(dto.Nom))
        {
            return BadRequest("Le nom du livrable est obligatoire.");
        }

        var livrable = new Livrable
        {
            Nom = dto.Nom,
            Type = dto.Type,
            UrlOuChemin = dto.UrlOuChemin,
            Commentaire = dto.Commentaire,
            Statut = StatutLivrable.Depose,
            ProjetId = projectId
        };

        _db.Livrables.Add(livrable);
        _db.SaveChanges();

        return CreatedAtAction(nameof(GetById), new { id = livrable.Id }, ToDto(livrable));
    }

    /// <summary>
    /// Valide ou refuse un livrable. Volontairement distinct de la création :
    /// on ne peut pas repasser un livrable à "Déposé" via cette route.
    /// </summary>
    [HttpPatch("deliverables/{id:int}/status")]
    public IActionResult UpdateStatus(int id, LivrableStatutUpdateDto dto)
    {
        var livrable = _db.Livrables.Find(id);
        if (livrable is null)
        {
            return NotFound();
        }

        if (dto.Statut == StatutLivrable.Depose)
        {
            return BadRequest("Utilisez ce endpoint uniquement pour valider ou refuser un livrable.");
        }

        livrable.Statut = dto.Statut;
        _db.SaveChanges();

        return NoContent();
    }

    private static LivrableDto ToDto(Livrable l) => new(
        l.Id, l.Nom, l.Type, l.UrlOuChemin, l.Statut, l.Commentaire, l.ProjetId);
}