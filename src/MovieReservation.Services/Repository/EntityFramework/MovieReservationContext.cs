using Microsoft.EntityFrameworkCore;
using MovieReservation.Domain;
using MovieReservation.Domain.Entities;

namespace MovieReservation.Services;

public class MovieReservationContext : DbContext
{
    public MovieReservationContext(DbContextOptions<MovieReservationContext> options)
        : base(options) { }

    public DbSet<User> Users { get; set; }
    public DbSet<Seat> Seats { get; set; }
    public DbSet<TheaterRoom> TheaterRooms { get; set; }

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
            entity.Property(r => r.Description).HasMaxLength(120);
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
        });

        modelBuilder.Entity<TheaterRoom>(entity =>
        {
            entity.ToTable("TheaterRooms");
            entity.HasKey(tr => tr.Id);
            entity.Property(tr => tr.Name).IsRequired().HasMaxLength(100);
            entity.Property(tr => tr.Description).HasMaxLength(200);
            entity.Property(tr => tr.RoomType).IsRequired().HasMaxLength(50);
            entity.Property(tr => tr.Capacity).IsRequired();;
        });

        modelBuilder.Entity<Seat>(entity =>
        {
            entity.ToTable("Seats");
            entity.HasKey(s => s.Id);
            entity.Property(s => s.RowLetter).IsRequired();
            entity.Property(s => s.SeatNumber).IsRequired();
            entity.Property(s => s.SeatType);
            entity.Property(s => s.IsActive);
            entity.HasOne<TheaterRoom>(s => s.TheaterRoom)
                .WithMany(tr => tr.Seats)
                .HasForeignKey(tr => tr.TheaterRoomId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasIndex(s => new {s.TheaterRoomId, s.RowLetter, s.SeatNumber}).IsUnique();
        });

        // modelBuilder.Entity<ShowTime>(entity =>
        // {
        //     entity.ToTable("ShowTimes");
        //     entity.HasKey(show => show.Id);
        //     entity.Property(show => show.ShowDateTime).IsRequired();
        //     entity.Property(show => show.Duration).IsRequired();
        //     entity.Property(show => show.Price).HasPrecision(10, 2);
        //     entity.Property(show => show.IsActive).HasDefaultValue(true);
        // });

    }
}
