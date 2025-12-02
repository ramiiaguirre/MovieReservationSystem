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
                .UsingEntity<UserRole>(
                    e => e.HasOne(ur => ur.Role).WithMany()
                        .HasForeignKey(ur => ur.UserRole_Role_FK).HasConstraintName("UserRole_Role_FK"),
                    e => e.HasOne(ur => ur.User).WithMany()
                       .HasForeignKey(ur => ur.UserRole_User_FK).HasConstraintName("UserRole_User_FK"),
                    e =>
                    {
                        e.ToTable("UserRole");
                        // e.HasKey(ur => new { ur.UserRole_User_FK, ur.UserRole_Role_FK });
                        e.Property(ur => ur.AssignedAt).HasColumnName("assigned_at").IsRequired();
                        e.Property(ur => ur.AssignedBy).HasColumnName("assigned_by").IsRequired();
                    }
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
                .UsingEntity<RolePermission>(
                    e => e.HasOne(rp => rp.Permission).WithMany()
                        .HasForeignKey(rp => rp.RolePermission_Permission_FK).HasConstraintName("RolePermission_Permission_FK"),
                    e => e.HasOne(rp => rp.Role).WithMany()
                        .HasForeignKey(rp => rp.RolePermission_Role_FK).HasConstraintName("RolePermission_Role_FK"),
                    e =>  e.ToTable("RolePermission")
                );
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