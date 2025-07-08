using dweb.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace dweb.Data;
using Microsoft.EntityFrameworkCore;


public class AppDbContext : IdentityDbContext<Utilizador>
{
    public AppDbContext(DbContextOptions<AppDbContext> options) 
        : base(options) { }

    public DbSet<Filme> Filme { get; set; }
    public DbSet<Utilizador> Utilizador { get; set; }
    public DbSet<Actor> Actor { get; set; }
    public DbSet<Director> Director { get; set; }
    public DbSet<Genero> Genero { get; set; }
    public DbSet<Review> Review { get; set; }
    
    public DbSet<Mensagem> Mensagem { get; set; }
    
    public DbSet<FilmeUtilizador> FilmeUtilizador { get; set; }
    
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder); 

        modelBuilder.Entity<Mensagem>()
            .HasOne(m => m.User)
            .WithMany(u => u.Mensagem) 
            .HasForeignKey(m => m.UserID)
            .OnDelete(DeleteBehavior.Restrict); 
        
        modelBuilder.Entity<FilmeUtilizador>()
            .HasKey(fu => new { fu.FilmeId, fu.UtilizadorId });

        modelBuilder.Entity<FilmeUtilizador>()
            .HasOne(fu => fu.Filme)
            .WithMany(f => f.FilmeUtilizador)
            .HasForeignKey(fu => fu.FilmeId);

        modelBuilder.Entity<FilmeUtilizador>()
            .HasOne(fu => fu.Utilizador)
            .WithMany(u => u.FilmeUtilizador)
            .HasForeignKey(fu => fu.UtilizadorId);
       
    }
    
}