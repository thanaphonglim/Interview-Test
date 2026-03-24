using Interview_Test.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Interview_Test.Infrastructure;

public class InterviewTestDbContext : DbContext
{
    public InterviewTestDbContext(DbContextOptions<InterviewTestDbContext> options) : base(options)
    {
    }
    
    public DbSet<UserModel> UserTb { get; set; }
    public DbSet<UserProfileModel> UserProfileTb { get; set; }
    public DbSet<RoleModel> RoleTb { get; set; }
    public DbSet<UserRoleMappingModel> UserRoleMappingTb { get; set; }
    public DbSet<PermissionModel> PermissionTb { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<UserRoleMappingModel>(entity =>
        {
            entity.HasOne(urm => urm.User)
                  .WithMany(u => u.UserRoleMappings)
                  .HasForeignKey(urm => urm.UserId)
                  .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(urm => urm.Role)
                  .WithMany(r => r.UserRoleMappings)
                  .HasForeignKey(urm => urm.RoleId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<UserModel>()
                    .HasOne(u => u.UserProfile)
                    .WithOne(up => up.User)
                    .HasForeignKey<UserProfileModel>(up => up.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
        modelBuilder.Entity<UserProfileModel>()
                    .HasOne(up => up.User)
                    .WithOne(u => u.UserProfile)  
                    .HasForeignKey<UserProfileModel>(up => up.UserId)
                    .IsRequired();
        modelBuilder.Entity<PermissionModel>()
                    .HasOne(p => p.Role)
                    .WithMany(r => r.Permissions)
                    .HasForeignKey(p => p.RoleId)
                    .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<RoleModel>()
                    .Property(r => r.RoleId).ValueGeneratedNever();
    }
}

public class InterviewTestDbContextDesignFactory : IDesignTimeDbContextFactory<InterviewTestDbContext>
{
    public InterviewTestDbContext CreateDbContext(string[] args)
    {
        string connectionString = "Data Source=localhost,1433;Initial Catalog=InterviewTestDb;User ID=admin;Password=admin;TrustServerCertificate=True;";
        var optionsBuilder = new DbContextOptionsBuilder<InterviewTestDbContext>()
            .UseSqlServer(connectionString, opts => opts.CommandTimeout(600));

        return new InterviewTestDbContext(optionsBuilder.Options);
    }
}