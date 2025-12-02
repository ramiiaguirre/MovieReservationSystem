using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

using MovieReservation.Model.Domain;

public class MovieReservationContext : DbContext
{
    public MovieReservationContext(DbContextOptions<MovieReservationContext> options)
        : base(options) {}

    public DbSet<User> Users { get; set; }
    public DbSet<Movie> Movies { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {

        UserContextCreating.ModelUserCreating(modelBuilder);
        MovieContextCreating.ModelMovieCreating(modelBuilder);
        
    }
}