using Data.Models;
using Data.Models.Chalets;
using Data.Models.General;
using iQuarc.DataLocalization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Text;
using Data.Models.Chalets.ChaletDetails;


public class DataContext : DbContext,IDataContext
{

    public DataContext(DbContextOptions<DataContext> options)
  : base(options)
    {
        LocalizationConfig.RegisterLocalizationEntity<Language>(l => l.Code);
        LocalizationConfig.RegisterCultureMapper(c => c.LCID);

    }

    private IDbContextTransaction _transaction;
    public virtual DbSet<File> Files { get; set; }

    public virtual DbSet<Setting> Settings { get; set; }
    public virtual DbSet<User> Users { get; set; }
    public virtual DbSet<Country> Countries { get; set; }
    public virtual DbSet<City> Cities { get; set; }
    public virtual DbSet<Neighborhood> Neighborhoods { get; set; }
    public virtual DbSet<Region> Regions { get; set; }
    public virtual DbSet<Chalet> Chalets { get; set; }
    public virtual DbSet<ChaletImage> ChaletImages { get; set; }
    public virtual DbSet<ChaletUser> ChaletUsers { get; set; }
    public virtual DbSet<ChaletSetting> ChaletSettings { get; set; }
    public virtual DbSet<Bank> Banks { get; set; }
    public virtual DbSet<ChaletBank> ChaletBanks { get; set; }
    public virtual DbSet<ParameterGroup> ParameterGroups { get; set; }
    public virtual DbSet<Parameter> Parameters { get; set; }
    public virtual DbSet<ChaletParameterValue> ChaletParameterValues { get; set; }
    public virtual DbSet<Unit> Units { get; set; }
    public virtual DbSet<UnitImage> UnitImages { get; set; }
    public virtual DbSet<PricePerDay> PricePerDays { get; set; }
    public virtual DbSet<Offer> Offers { get; set; }
    public virtual DbSet<Reservation> Reservations { get; set; }
        public virtual DbSet<Job> Jobs { get; set; }
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