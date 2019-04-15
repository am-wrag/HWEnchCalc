using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using HWEnchCalc.Calculatros.EssenceCalc;
using Microsoft.EntityFrameworkCore;
using HWEnchCalc.Titan;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace HWEnchCalc.DB
{
    public class DbOperator : DbContext
    {
        private readonly string _dbConnectionString;
        public DbSet<TitanBaseStats> TitanBaseStatsData { get; set; }
        public DbSet<EssenceCalcInfo> EssenceCalcData { get; set; }
        public DbSet<ArtefactInfo> ArtefactInfos { get; set; }

        public DbOperator(string dbConnectionString)
        {
            _dbConnectionString = dbConnectionString;
            CreateDbIfNotExist();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite(_dbConnectionString);
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //можно использовать встроенный конвертер, но то что ниже - более наглядно
            var converter = new EnumToStringConverter<ArtefactType>();

            modelBuilder
                .Entity<ArtefactInfo>()
                .Property(e => e.ArtefactType)
                .HasConversion(
            v => v.ToString(),
            v => (ArtefactType)Enum.Parse(typeof(ArtefactType), v));

            modelBuilder.Entity<EssenceCalcInfo>()
                .HasOne(c => c.AtackArt);
            modelBuilder.Entity<EssenceCalcInfo>()
                .HasOne(c => c.DefArt);
            modelBuilder.Entity<EssenceCalcInfo>()
                .HasOne(c => c.TitanBaseStats);
        }

        public void AddEssenceCalcInfo(EssenceCalcInfo calcInfo)
        {
            calcInfo.Id = new int();
            calcInfo.AtackArt.Id = new int();
            calcInfo.DefArt.Id = new int();
            calcInfo.TitanBaseStats.Id = new int();
            EssenceCalcData.Add(calcInfo);
            SaveChanges();
        }

        public void DeleteEssenceCalcInfoById(int id)
        {
            EssenceCalcData.Remove(EssenceCalcData.First(c => c.Id == id));
            SaveChanges();
        }

        public void CreateDbIfNotExist()
        {
            Database.EnsureCreated();
        }

        public List<EssenceCalcInfo> GetEssenceCalcData()
        {
            return EssenceCalcData?
                .Include(d => d.AtackArt)
                .Include(d => d.DefArt)
                .Include(d => d.TitanBaseStats)
                .ToList();
        }

        public ObservableCollection<EssenceCalcShortInfo> GetShortCalcInfos()
        {
            var calcData = EssenceCalcData
                .Include(d => d.AtackArt)
                .Include(d => d.DefArt)
                .Include(d => d.TitanBaseStats)
                .Select(c => c.ToShortInfo());

            return new ObservableCollection<EssenceCalcShortInfo>(calcData);
        }
    }
}