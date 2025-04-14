using dweb.Models;

namespace dweb.Data;
using Microsoft.EntityFrameworkCore;


public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) 
        : base(options) { }

    public DbSet<Filme> Filme { get; set; }
    public DbSet<Utilizador> Utilizador { get; set; }
    public DbSet<Utilizador_Filme> Utilizador_Filme { get; set; }
    public DbSet<Filme_Actor> Filme_Actor { get; set; }
    public DbSet<Actor> Actor { get; set; }
    public DbSet<Director> Director { get; set; }
    public DbSet<Filme_Director> Filme_Director { get; set; }
    public DbSet<Genero> Genero { get; set; }
    public DbSet<Filme_Genero> Filme_Genero { get; set; }
    public DbSet<Review> Review { get; set; }
    
    
}