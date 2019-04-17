using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using HWEnchCalc.Calculators.EssenceCalc;
using Microsoft.EntityFrameworkCore;
using HWEnchCalc.Titan;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace HWEnchCalc.DB
{
    public class DbOperator : DbContext
    {
        private readonly string _dbConnectionString;
        public DbSet<TitanStats> TitanBaseStatsData { get; set; }
        public DbSet<EssenceCalcResult> EssenceCalcData { get; set; }
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

            modelBuilder.Entity<EssenceCalcResult>()
                .HasOne(c => c.AttackArt);
            modelBuilder.Entity<EssenceCalcResult>()
                .HasOne(c => c.DefArt);
            modelBuilder.Entity<EssenceCalcResult>()
                .HasOne(c => c.TitanBaseStats);
        }

        public void AddEssenceCalcInfo(EssenceCalcResult calcInfo)
        {
            calcInfo.Id = new int();
            calcInfo.AttackArt.Id = new int();
            calcInfo.DefArt.Id = new int();
            calcInfo.TitanBaseStats.Id = new int();
            EssenceCalcData.Add(calcInfo);
            SaveChanges();
        }

        public void DeleteEssenceCalcInfoById(int id)
        {
            EssenceCalcData.Remove(EssenceCalcData.Find(id));
            SaveChanges();
        }

        public void CreateDbIfNotExist()
        {
            Database.EnsureCreated();
        }

        public List<EssenceCalcResult> GetEssenceCalcData()
        {
            return EssenceCalcData?
                .Include(d => d.AttackArt)
                .Include(d => d.DefArt)
                .Include(d => d.TitanBaseStats)
                .ToList();
        }

        public ObservableCollection<EssenceCalcResultShort> GetShortCalcInfos()
        {
            var calcData = EssenceCalcData
                .Include(d => d.AttackArt)
                .Include(d => d.DefArt)
                .Include(d => d.TitanBaseStats)
                .Select(c => c.ToShortInfo());

            return new ObservableCollection<EssenceCalcResultShort>(calcData);
        }
    }
}