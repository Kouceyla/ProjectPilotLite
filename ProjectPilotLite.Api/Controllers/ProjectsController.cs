using Microsoft.AspNetCore.Mvc;
using ProjectPilotLite.Api.Data;
using ProjectPilotLite.Core.Dtos;
using ProjectPilotLite.Core.Models;

namespace ProjectPilotLite.Api.Controllers;

/// <summary>
/// Feature "Projets" — portée par Ethan.
/// Version minimale (CRUD de base + changement de statut) fournie pour que
/// les features Tâches et Livrables puissent être développées/testées.
/// À compléter par Ethan : validations plus fines, éventuel détail enrichi.
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

    [HttpGet]
    public ActionResult<IEnumerable<ProjetDto>> GetAll()
    {
        return Ok(_db.Projets.Select(ToDto).ToList());
    }

    [HttpGet("{id:int}")]
    public ActionResult<ProjetDto> GetById(int id)
    {
        var projet = _db.Projets.Find(id);
        if (projet is null)
        {
            return NotFound();
        }

        return Ok(ToDto(projet));
    }

    [HttpPost]
    public ActionResult<ProjetDto> Create(ProjetCreateDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Nom))
        {
            return BadRequest("Le nom du projet est obligatoire.");
        }

        var projet = new Projet
        {
            Nom = dto.Nom,
            Description = dto.Description,
            Responsable = dto.Responsable,
            DateDebut = dto.DateDebut,
            DateLimite = dto.DateLimite,
            Statut = StatutProjet.Prevu
        };

        _db.Projets.Add(projet);
        _db.SaveChanges();

        return CreatedAtAction(nameof(GetById), new { id = projet.Id }, ToDto(projet));
    }

    [HttpPatch("{id:int}/status")]
    public IActionResult UpdateStatus(int id, ProjetStatutUpdateDto dto)
    {
        var projet = _db.Projets.Find(id);
        if (projet is null)
        {
            return NotFound();
        }

        projet.Statut = dto.Statut;
        _db.SaveChanges();

        return NoContent();
    }

    private static ProjetDto ToDto(Projet p) => new(
        p.Id, p.Nom, p.Description, p.Responsable, p.DateDebut, p.DateLimite, p.Statut);
}
