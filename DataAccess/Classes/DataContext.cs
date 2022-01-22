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
using Data.Models.Chalets.RatingAndReview;

public class DataContext : DbContext, IDataContext
{

    public DataContext(DbContextOptions<DataContext> options)
  : base(options)
    {
        LocalizationConfig.RegisterLocalizationEntity<Language>(l => l.Code);
        LocalizationConfig.RegisterCultureMapper(c => c.Name);

    }

    private IDbContextTransaction _transaction;
    public virtual DbSet<File> Files { get; set; }

    public virtual DbSet<Setting> Settings { get; set; }
    public virtual DbSet<User> Users { get; set; }
    public virtual DbSet<Country> Countries { get; set; }
    public virtual DbSet<City> Cities { get; set; }
    //public virtual DbSet<Neighborhood> Neighborhoods { get; set; }
    //public virtual DbSet<Region> Regions { get; set; }
    public virtual DbSet<Chalet> Chalets { get; set; }
    public virtual DbSet<ChaletImage> ChaletImages { get; set; }
    public virtual DbSet<ChaletUser> ChaletUsers { get; set; }
    //public virtual DbSet<ChaletSetting> ChaletSettings { get; set; }
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
    public virtual DbSet<CityTranslation> CityTranslations { get; set; }
    public virtual DbSet<RegionTranslation> RegionTranslations { get; set; }
    public virtual DbSet<NeighborhoodTranslation> NeighborhoodTranslations { get; set; }
    public virtual DbSet<BankTranslation> BankTranslations { get; set; }
    public virtual DbSet<CountryTranslation> CountryTranslations { get; set; }
    public virtual DbSet<SettingTranslation> SettingTranslations { get; set; }
    public virtual DbSet<ParameterGroupTranslation> ParameterGroupTranslations { get; set; }
    public virtual DbSet<ParameterTranslation> ParameterTranslations { get; set; }
    public virtual DbSet<ContactUs> ContactUss { get; set; }
    public virtual DbSet<ResortParameterValue> ResortParameterValues { get; set; }
    public virtual DbSet<Fiverate> Fiverates { get; set; }
    public virtual DbSet<Rate> Rates { get; set; }
    public virtual DbSet<PaymentTransaction> Invoices { get; set; }

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