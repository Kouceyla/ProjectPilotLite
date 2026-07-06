using Microsoft.EntityFrameworkCore;
using ProjectPilotLite.Core.Models;

namespace ProjectPilotLite.Api.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<Projet> Projets => Set<Projet>();
    public DbSet<Tache> Taches => Set<Tache>();
    public DbSet<Livrable> Livrables => Set<Livrable>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Tache>()
            .HasOne(t => t.Projet)
            .WithMany(p => p.Taches)
            .HasForeignKey(t => t.ProjetId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Livrable>()
            .HasOne(l => l.Projet)
            .WithMany(p => p.Livrables)
            .HasForeignKey(l => l.ProjetId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
