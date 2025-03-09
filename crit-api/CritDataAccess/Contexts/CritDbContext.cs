using CritDTO.Identity;
using CritDTO.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using MongoDB.Bson;
using MongoDB.EntityFrameworkCore.Extensions;

namespace CritDataAccess.Contexts;

public class CritDbContext : DbContext, ICritDbContext
{
    public DbSet<Organization> Organizations { get; set; }

    public CritDbContext(DbContextOptions options) : base(options)
    {
        ConfigureDefault();
    }

    protected CritDbContext()
    {
        ConfigureDefault();
    }

    public void Configure(Action<DatabaseFacade> databaseAction)
    {
        databaseAction.Invoke(Database);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Organization>(entity =>
        {
            entity.ToCollection("Organizations");
            entity.HasKey(collection => collection.Id);
            entity.Property(collection => collection.Id).HasColumnName("_id").IsRequired(false).ValueGeneratedOnAdd();
            entity.Property(collection => collection.Name).HasColumnName("name").IsRequired(true);
            entity.Property(collection => collection.OwnerUserId).HasColumnName("ownerUserId").IsRequired(true);
            entity.Property(collection => collection.PhoneNumbers).HasColumnName("phoneNumbers").IsRequired(false);
            entity.Property(collection => collection.Emails).HasColumnName("emails").IsRequired(false);
            entity.Property(collection => collection.ProjectIds).HasColumnName("projectIds").IsRequired(false);
            entity.Property(collection => collection.AdminUserIds).HasColumnName("adminUserIds").IsRequired(false);
            entity.Property(collection => collection.MemberUserIds).HasColumnName("memberUserIds").IsRequired(false);
            entity.Property(collection => collection.affiliatedUserIds).HasColumnName("affiliatedUserIds").IsRequired(false);
        });
    }

    private void ConfigureDefault()
    {
        Configure(db =>
        {
            db.EnsureCreated();
        });
    }
}
