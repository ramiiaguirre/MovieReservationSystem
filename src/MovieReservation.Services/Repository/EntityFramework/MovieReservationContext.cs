using Microsoft.EntityFrameworkCore;
using MovieReservation.Domain;

namespace MovieReservation.Services;

public class MovieReservationContext : DbContext
{
    public MovieReservationContext(DbContextOptions<MovieReservationContext> options)
        : base(options) { }

    public DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("Users");
            entity.HasKey(u => u.Id);
            entity.Property(u => u.Name).IsRequired().HasMaxLength(60);
            entity.Property(u => u.Password).IsRequired(false).HasMaxLength(100);
            entity.HasMany(u => u.Roles)
                .WithMany(u => u.Users)
                .UsingEntity<Dictionary<string, object>>(
                    "User_Rol", 
                    j => j.HasOne<Rol>().WithMany().HasForeignKey("RolId"),
                    j => j.HasOne<User>().WithMany().HasForeignKey("UserId")
                );
        });

        modelBuilder.Entity<Rol>(entity =>
        {
            entity.ToTable("Roles");
            entity.HasKey(r => r.Id);
            entity.Property(r => r.Name).IsRequired().HasMaxLength(60);
            entity.Property(r => r.Description).IsRequired(false).HasMaxLength(120);
            entity.Property(r => r.CreatedAt).IsRequired();
            entity.Property(r => r.UpdateAt).IsRequired();
            // Rol ↔ Permission (muchos a muchos)
            entity.HasMany(r => r.Permissions)
                .WithMany()
                .UsingEntity<Dictionary<string, object>>(
                    "Rol_Permission",
                    j => j.HasOne<Permission>().WithMany().HasForeignKey("PermissionId"),
                    j => j.HasOne<Rol>().WithMany().HasForeignKey("RolId")
                );
        });

        modelBuilder.Entity<Permission>(entity =>
        {
            entity.ToTable("Permissions");
            entity.HasKey(r => r.Id);
            entity.Property(r => r.Name).IsRequired().HasMaxLength(60);
            entity.Property(r => r.Description).IsRequired(false).HasMaxLength(120);
            entity.Property(r => r.CreateAt).IsRequired();
            entity.Property(r => r.UpdateAt).IsRequired();
        });

        modelBuilder.Entity<Movie>(entity =>
        {
            entity.ToTable("Movies");
            entity.HasKey(m => m.Id);
            entity.Property(m => m.Name).IsRequired();
            entity.Property(m => m.Description).IsRequired(false);
            entity.Property(m => m.ShowTime).IsRequired();
        });
    }
}
