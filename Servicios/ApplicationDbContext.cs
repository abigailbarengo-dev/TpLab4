namespace TpLab4.Servicios;
using Microsoft.EntityFrameworkCore;
using TpLab4.Models;
using static System.Collections.Specialized.BitVector32;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions options) : base(options)
    {

    }

    public DbSet<Actor> actor { get; set; }
    public DbSet<Genero> genero { get; set; }
    public DbSet<Pelicula> pelicula { get; set; }
    public DbSet<PeliculaActores> peliculaActores { get; set; }


}

