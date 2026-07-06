using Microsoft.EntityFrameworkCore;
using ProjectPilotLite.Api.Data;

var builder = WebApplication.CreateBuilder(args);

// Base de données locale SQLite (recommandée par l'énoncé, aucune installation de serveur nécessaire).
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// CORS ouvert : l'API n'est consommée localement que par le client WPF.
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy => policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
});

var app = builder.Build();

// Création de la base + jeu de données de démo au démarrage (pas de migration EF séparée
// nécessaire pour ce projet volontairement limité : EnsureCreated() suffit).
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    db.Database.EnsureCreated();
    SeedData.Initialize(db);
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors();
app.MapControllers();

// HTTP local uniquement, pas de certificat à gérer (cf. énoncé section 6).
app.Run("http://localhost:5123");
