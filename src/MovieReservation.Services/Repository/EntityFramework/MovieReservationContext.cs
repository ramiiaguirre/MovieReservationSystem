using Microsoft.EntityFrameworkCore;
using MovieReservation.Domain;

namespace MovieReservation.Services;

public class MovieReservationContext : DbContext
{
    public MovieReservationContext(DbContextOptions<MovieReservationContext> options)
        : base(options) { }

    public DbSet<User> Users { get; set; }
    public DbSet<Rol> Roles { get; set; }
    public DbSet<Permission> Permissions { get; set; }
    public DbSet<Movie> Movies { get; set; }
    public DbSet<Seat> Seats { get; set; }
    public DbSet<TheaterRoom> TheaterRooms { get; set; }
    public DbSet<ShowTime> ShowTimes { get; set; }
    public DbSet<Reservation> Reservations { get; set; }
    public DbSet<ReservationSeat> ReservationSeats { get; set; }

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
            entity.Property(r => r.CreatedAt);
            entity.Property(r => r.UpdateAt);
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
            entity.Property(r => r.CreateAt);
            entity.Property(r => r.UpdateAt);
        });

        modelBuilder.Entity<Movie>(entity =>
        {
            entity.ToTable("Movies");
            entity.HasKey(m => m.Id);
            entity.Property(m => m.Name).IsRequired();
            entity.Property(m => m.Description).IsRequired(false);
            entity.Property(m => m.Genre).IsRequired().HasConversion<string>();
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

        modelBuilder.Entity<ShowTime>(entity =>
        {
            entity.ToTable("ShowTimes");
            entity.HasKey(show => show.Id);
            entity.Property(show => show.ShowDateTime).IsRequired();
            entity.Property(show => show.Duration).IsRequired();
            entity.Property(show => show.Price).IsRequired().HasPrecision(10, 2);
            entity.Property(show => show.IsActive).HasDefaultValue(true);

            entity.HasOne(show => show.Movie)
                .WithMany(m => m.ShowTimes)
                .HasForeignKey(show => show.MovieId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(show => show.TheaterRoom)
                .WithMany()
                .HasForeignKey(show => show.TheaterRoomId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasIndex(show => new { show.TheaterRoomId, show.ShowDateTime }).IsUnique();
        });

        modelBuilder.Entity<Reservation>(entity =>
        {
            entity.ToTable("Reservations");
            entity.HasKey(r => r.Id);
            entity.Property(r => r.ReservationCode).IsRequired();
            entity.Property(r => r.TotalAmount).IsRequired().HasPrecision(10, 2);
            entity.Property(r => r.Status).IsRequired();
            entity.Property(r => r.CreatedAt).IsRequired();

            entity.HasIndex(r => r.ReservationCode).IsUnique();

            entity.HasOne(r => r.User)
                .WithMany()
                .HasForeignKey(r => r.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(r => r.ShowTime)
                .WithMany()
                .HasForeignKey(r => r.ShowTimeId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<ReservationSeat>(entity =>
        {
            entity.ToTable("ReservationSeats");
            entity.HasKey(rs => rs.Id);
            entity.Property(rs => rs.Price).IsRequired().HasPrecision(10, 2);

            entity.HasIndex(rs => new { rs.SeatId, rs.ShowTimeId }).IsUnique();

            entity.HasOne(rs => rs.Reservation)
                .WithMany(r => r.Seats)
                .HasForeignKey(rs => rs.ReservationId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(rs => rs.Seat)
                .WithMany()
                .HasForeignKey(rs => rs.SeatId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(rs => rs.ShowTime)
                .WithMany(show => show.ReservationSeats)
                .HasForeignKey(rs => rs.ShowTimeId)
                .OnDelete(DeleteBehavior.Restrict);
        });

    }
}
