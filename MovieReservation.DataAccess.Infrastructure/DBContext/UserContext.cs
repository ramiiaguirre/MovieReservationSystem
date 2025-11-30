using Microsoft.EntityFrameworkCore;
using MovieReservation.Model.Domain;

public static class UserContextCreating
{
    public static void ModelUserCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("User", tb => tb.HasTrigger("User_update_at"));
            entity.HasKey(u => u.Id);
            entity.Property(u => u.Name).IsRequired().HasMaxLength(60);
            entity.Property(u => u.Password).IsRequired().HasMaxLength(100);
            entity.Property(u => u.CreatedAt)
                .HasColumnName("created_at").HasDefaultValueSql("GETDATE()");
            entity.Property(u => u.UpdatedAt)
                .HasColumnName("updated_at").HasDefaultValueSql("GETDATE()");
            entity.HasMany(u => u.Roles)
                .WithMany(u => u.Users)
                .UsingEntity<Dictionary<string, object>>(
                    "UserRole", 
                    j => j.HasOne<Role>().WithMany().HasForeignKey("UserRole_Role_FK"),
                    j => j.HasOne<User>().WithMany().HasForeignKey("UserRole_User_FK")
                );
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.ToTable("Role");
            entity.HasKey(r => r.Id);
            entity.Property(r => r.Name).IsRequired().HasMaxLength(60);
            entity.Property(r => r.Description).IsRequired(false).HasMaxLength(120);
            entity.HasMany(r => r.Permissions)
                .WithMany(r => r.Roles)
                .UsingEntity<Dictionary<string, object>>(
                    "RolePermission",
                    p => p.HasOne<Permission>().WithMany().HasForeignKey("RolePermission_Permission_FK"),
                    p => p.HasOne<Role>().WithMany().HasForeignKey("RolePermission_Role_FK"));
        });

        modelBuilder.Entity<Permission>(entity =>
        {
            entity.ToTable("Permission");
            entity.HasKey(r => r.Id);
            entity.Property(r => r.Name).IsRequired().HasMaxLength(60);
            entity.Property(r => r.Description).IsRequired(false).HasMaxLength(120);
        });
    }
}