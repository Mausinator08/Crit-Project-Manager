using CritDTO.Identity;
using CritDTO.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using MongoDB.Bson;
using MongoDB.EntityFrameworkCore.Extensions;

namespace CritDataAccess.Contexts;

public class CritDbContext : IdentityDbContext, ICritDbContext
{
    public DbSet<Organization> Organizations { get; set; }
    public DbSet<Email> Emails { get; set; }
    public DbSet<PhoneNumber> PhoneNumbers { get; set; }

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
            entity.Property(collection => collection.Id).HasColumnName("_id").IsRequired(true).ValueGeneratedOnAdd();
            entity.Property(collection => collection.Name).HasColumnName("name").IsRequired(true);
            entity.Property(collection => collection.OwnerUserId).HasColumnName("ownerUserId").IsRequired(true);
            entity.Property(collection => collection.PhoneNumberIds).HasColumnName("phoneNumberIds").IsRequired(false);
            entity.Property(collection => collection.EmailIds).HasColumnName("emailIds").IsRequired(false);
            entity.Property(collection => collection.ProjectIds).HasColumnName("projectIds").IsRequired(false);
            entity.Property(collection => collection.AdminUserIds).HasColumnName("adminUserIds").IsRequired(false);
            entity.Property(collection => collection.MemberUserIds).HasColumnName("memberUserIds").IsRequired(false);
            entity.Property(collection => collection.AffiliatedUserIds).HasColumnName("affiliatedUserIds").IsRequired(false);
        });

        modelBuilder.Entity<Organization>()
        .HasMany(collection => collection.Emails)
        .WithOne(collection => collection.Organization)
        .HasForeignKey(collection => collection.OrganizationId)
        .IsRequired(true);

        modelBuilder.Entity<Organization>()
        .HasMany(collection => collection.PhoneNumbers)
        .WithOne(collection => collection.Organization)
        .HasForeignKey(collection => collection.OrganizationId)
        .IsRequired(true);

        modelBuilder.Entity<Email>(entity =>
        {
            entity.ToCollection("Emails");
            entity.HasKey(collection => collection.Id);
            entity.Property(collection => collection.Id).HasColumnName("_id").IsRequired(true).ValueGeneratedOnAdd();
            entity.Property(collection => collection.EmailAddress).HasColumnName("emailAddress").IsRequired(true);
            entity.Property(collection => collection.UserId).HasColumnName("userId").IsRequired(true);
        });

        modelBuilder.Entity<Email>()
        .HasOne(collection => collection.Organization)
        .WithMany(collection => collection.Emails)
        .HasForeignKey(collection => collection.OrganizationId)
        .IsRequired(true);

        modelBuilder.Entity<PhoneNumber>(entity =>
        {
            entity.ToCollection("PhoneNumbers");
            entity.HasKey(collection => collection.Id);
            entity.Property(collection => collection.Id).HasColumnName("_id").IsRequired(true).ValueGeneratedOnAdd();
            entity.Property(collection => collection.CountryCode).HasColumnName("countryCode").IsRequired(true);
            entity.Property(collection => collection.Number).HasColumnName("number").IsRequired(true);
            entity.Property(collection => collection.Extension).HasColumnName("extension").IsRequired(false);
            entity.Property(collection => collection.Type).HasColumnName("type").IsRequired(true);
        });

        modelBuilder.Entity<PhoneNumber>()
        .HasOne(collection => collection.Organization)
        .WithMany(collection => collection.PhoneNumbers)
        .HasForeignKey(collection => collection.OrganizationId)
        .IsRequired(true);
    }

    private void ConfigureDefault()
    {
        Configure(db =>
        {
            db.EnsureCreated();
        });
    }
}
