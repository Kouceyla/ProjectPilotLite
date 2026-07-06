using Microsoft.AspNetCore.Mvc;
using ProjectPilotLite.Api.Data;
using ProjectPilotLite.Core.Dtos;
using ProjectPilotLite.Core.Models;

namespace ProjectPilotLite.Api.Controllers;

[ApiController]
[Route("api/dashboard")]
public class DashboardController : ControllerBase
{
    private readonly ApplicationDbContext _db;

    public DashboardController(ApplicationDbContext db) => _db = db;

    [HttpGet]
    public ActionResult<DashboardDto> Get()
    {
        var dto = new DashboardDto(
            TotalProjets: _db.Projets.Count(),
            ProjetsEnCours: _db.Projets.Count(p => p.Statut == StatutProjet.EnCours),
            ProjetsBloques: _db.Projets.Count(p => p.Statut == StatutProjet.Bloque),
            TotalTaches: _db.Taches.Count(),
            TachesTerminees: _db.Taches.Count(t => t.Statut == StatutTache.Termine),
            LivrablesDeposes: _db.Livrables.Count(l => l.Statut == StatutLivrable.Depose));

        return Ok(dto);
    }
}