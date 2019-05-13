using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace HWEnchCalc.DB
{
    public class DbOperator : DbContext
    {
        private readonly string _dbConnectionString;

        public DbSet<TitanInfoDbo> TitanInfos { get; set; }

        public DbOperator( string dbConnectionString)
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
                .HasOne(c => c.ElementalOffenceArtefact);
            modelBuilder.Entity<TitanInfoDbo>()
                .HasOne(c => c.ElementalDefenceAtrefact);
            modelBuilder.Entity<TitanInfoDbo>()
                .HasOne(c => c.SealArtefact);
            modelBuilder.Entity<TitanInfoDbo>()
                .HasMany(c => c.Guises);
        }

        public Task AddTitanInfoAsync(TitanInfoDbo calcInfo)
        {
            return Task.Factory.StartNew(() => AddTitanCalcInfo(calcInfo));
        }

        public void AddTitanCalcInfo(TitanInfoDbo calcInfo)
        {
            TitanInfos.Add(calcInfo);
            SaveChanges();
        }

        public Task DeleteTitanInfoByIdAsync(int id)
        {
            return Task.Factory.StartNew(() => DeleteTitanInfoById(id));
        }

        public void DeleteTitanInfoById(int id)
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
                .Include(d => d.ElementalOffenceArtefact)
                .Include(d => d.ElementalDefenceAtrefact)
                .Include(d => d.SealArtefact)
                .Include(d => d.Guises)
                .ToList();
        }
    }
}