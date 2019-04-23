using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using HWEnchCalc.Calculators.TitanCalc;
using HWEnchCalc.Titan;
using Microsoft.EntityFrameworkCore;

namespace HWEnchCalc.DB
{
    public class DbOperator : DbContext
    {
        private readonly string _dbConnectionString;
        public DbSet<TitanInfoDbo> TitanInfos { get; set; }
        public DbSet<TitatnArtefactInfoDbo> ArtefactInfos { get; set; }

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
            //var converter = new EnumToStringConverter<ArtefactType>();

            modelBuilder.Entity<TitanInfoDbo>()
                .HasOne(c => c.FirstArtefact);
            modelBuilder.Entity<TitanInfoDbo>()
                .HasOne(c => c.SecondArtefact);
            modelBuilder.Entity<TitanInfoDbo>()
                .HasOne(c => c.SealArtefact);
        }

        public Task AddEssenceCalcInfoAsync(TitanInfo calcInfo)
        {
            return Task.Factory.StartNew(() => AddEssenceCalcInfo(calcInfo));
        }

        public void AddEssenceCalcInfo(TitanInfo calcInfo)
        {
            var titanInfo = new TitanInfoBuilder().GeTitanInfoDbo(calcInfo);
            TitanInfos.Add(titanInfo);
            SaveChanges();
        }

        public Task DeleteEssenceCalcInfoByIdAsync(int id)
        {
            return Task.Factory.StartNew(() => DeleteEssenceCalcInfoById(id));
        }

        public void DeleteEssenceCalcInfoById(int id)
        {
            TitanInfos.Remove(TitanInfos.Find(id));
            SaveChanges();
        }

        public void CreateDbIfNotExist()
        {
            Database.EnsureCreated();
        }

        public Task<List<TitanInfoDbo>> GetTitanCalculatedInfoAsync()
        {
            return Task.Factory.StartNew(GetTitanCalculatedInfo);
        }

        public List<TitanInfoDbo> GetTitanCalculatedInfo()
        {
            return TitanInfos?
                .Include(d => d.FirstArtefact)
                .Include(d => d.SecondArtefact)
                .Include(d => d.SealArtefact)
                .ToList();
        }

        public Task<ObservableCollection<TitanShortInfo>> GetShortCalcInfosAsync()
        {
            return Task.Factory.StartNew(GetShortCalcInfos);
        }

        public ObservableCollection<TitanShortInfo> GetShortCalcInfos()
        {
            var result = TitanInfos.Select(t => new TitanShortInfo(t.Name, t.Id, t.Ticks));
            return new ObservableCollection<TitanShortInfo>(result);
        }
    }
}