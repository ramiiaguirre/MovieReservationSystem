using Microsoft.EntityFrameworkCore;
using MovieReservation.Model.Domain;

public static class MovieContextCreating
{
    public static void ModelMovieCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Movie>(entity =>
        {
            entity.ToTable("Movie");
            entity.HasKey(u => u.Id);
            entity.Property(u => u.Title).IsRequired().HasMaxLength(100);
            entity.Property(m => m.Description).IsRequired(false).HasMaxLength(120);
        });
    }
}