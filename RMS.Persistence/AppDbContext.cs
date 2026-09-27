using Microsoft.EntityFrameworkCore;
namespace RMS.Persistence;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<User> Users { get; set; }

    public DbSet<Roles> Roles { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Roles>(entity =>
        {
            entity.HasKey(x => x.Id);

            entity.Property(x => x.Id).UseIdentityByDefaultColumn();

            entity.Property(x => x.Name).IsRequired().HasMaxLength(50);

            entity.HasIndex(x => x.Name).IsUnique();

            entity.Property(x => x.Description).HasMaxLength(250);

            entity.Property(x => x.IsActive).IsRequired();

            entity.HasData(
                        new Roles
                        {
                            Id = 1,
                            Name = "SuperAdmin",
                            Description = "Full system access",
                            IsActive = true
                        },
                        new Roles
                        {
                            Id = 2,
                            Name = "Admin",
                            Description = "Administrative access",
                            IsActive = true
                        },
                        new Roles
                        {
                            Id = 3,
                            Name = "Cashier",
                            Description = "Cashier access",
                            IsActive = true
                        }
                    );
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(x => x.Id);

            entity.Property(x => x.Id).UseIdentityByDefaultColumn();

            entity.Property(x => x.Username).IsRequired().HasMaxLength(50);

            entity.HasIndex(x => x.Username).IsUnique();

            entity.Property(x => x.Email).IsRequired().HasMaxLength(150);

            entity.HasIndex(x => x.Email).IsUnique();

            entity.Property(x => x.PasswordHash).IsRequired();

            entity.Property(x => x.RoleId).IsRequired();

            entity.HasIndex(x => x.RoleId);

            entity.Property(x => x.IsActive).IsRequired();

            entity.Property(x => x.FailedAttempts).IsRequired();

            entity.Property(x => x.IsLocked).IsRequired();

            entity.Property(x => x.LockReason).HasMaxLength(250);

            entity.Property(x => x.CreatedBy).IsRequired(false);

            entity.Property(x => x.UpdatedBy).IsRequired(false);

            entity.HasOne(x => x.Role).WithMany().HasForeignKey(x => x.RoleId).OnDelete(DeleteBehavior.Restrict);
        });
    }
}