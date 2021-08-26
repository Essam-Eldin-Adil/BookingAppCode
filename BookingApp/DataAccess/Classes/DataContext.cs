using Data.Models;
using Data.Models.General;
using iQuarc.DataLocalization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Text;


public class DataContext : IdentityDbContext<User, Role, Guid, IdentityUserClaim<Guid>, UserRole, IdentityUserLogin<Guid>, IdentityRoleClaim<Guid>, IdentityUserToken<Guid>>, IDataContext
{
    public DataContext(DbContextOptions<DataContext> options) : base(options)
    {
        LocalizationConfig.RegisterLocalizationEntity<Language>(l => l.Code);
        LocalizationConfig.RegisterCultureMapper(c => c.LCID);
    }
    public DataContext()
    {

    }
    private IDbContextTransaction _transaction;

    public virtual DbSet<Group> Groups { get; set; }
    public virtual DbSet<GroupRole> GroupRoles { get; set; }
    public virtual DbSet<GroupTranslation> GroupTranslations { get; set; }
    public virtual DbSet<RoleTranslation> RoleTranslations { get; set; }
    public virtual DbSet<Notification> Notifications { get; set; }
    public virtual DbSet<NotificationUser> NotificationUsers { get; set; }
    public virtual DbSet<File> Files { get; set; }
    public virtual DbSet<Language> Languages { get; set; }
    public virtual DbSet<LanguageTranslation> LanguageTranslations { get; set; }
    public virtual DbSet<Organization> Organizations { get; set; }
    public virtual DbSet<OrganizationTranslation> OrganizationTranslations { get; set; }
    public virtual DbSet<TemporaryFile> TemporaryFiles { get; set; }

    public virtual DbSet<Setting> Settings { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<User>().ToTable("Users");
        builder.Entity<Role>().ToTable("Roles");
        builder.Entity<UserRole>().ToTable("UserRoles");
        builder.Entity<IdentityRoleClaim<Guid>>().ToTable("RoleClaims");
        builder.Entity<IdentityUserClaim<Guid>>().ToTable("UserClaims");
        builder.Entity<IdentityUserLogin<Guid>>().ToTable("UserLogins");
        builder.Entity<IdentityUserToken<Guid>>().ToTable("UserTokens");

        builder.Entity<UserRole>(userRole =>
        {
            userRole.HasKey(ur => new { ur.UserId, ur.RoleId });

            userRole.HasOne(ur => ur.Role)
                .WithMany(r => r.UserRoles)
                .HasForeignKey(ur => ur.RoleId)
                .IsRequired();

            userRole.HasOne(ur => ur.User)
              .WithMany(r => r.UserRoles)
              .HasForeignKey(ur => ur.UserId)
              .IsRequired();

        });
    }

    public void BeginTransaction()
    {
        _transaction = Database.BeginTransaction();
    }

    public void Migrate()
    {
        Database.Migrate();
    }


    public void Commit()
    {
        try
        {
            SaveChanges();
            _transaction.Commit();
        }
        catch
        {
            Rollback();
        }
        finally
        {
            _transaction.Dispose();
        }
    }

    public void Rollback()
    {
        _transaction.Rollback();
        _transaction.Dispose();
    }
}