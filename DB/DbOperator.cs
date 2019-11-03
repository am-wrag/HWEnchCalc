using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace HWEnchCalc.DB
{
    public class DbOperator : DbContext
    {
        private readonly string _dbConnectionString;

        public DbSet<TitanInfoDto> TitanInfos { get; set; }

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

            modelBuilder.Entity<TitanInfoDto>()
                .HasOne(c => c.ElementalOffenceArtefact);
            modelBuilder.Entity<TitanInfoDto>()
                .HasOne(c => c.ElementalDefenceAtrefact);
            modelBuilder.Entity<TitanInfoDto>()
                .HasOne(c => c.SealArtefact);
            modelBuilder.Entity<TitanInfoDto>()
                .HasMany(c => c.Guises);
        }

        public Task AddTitanInfoAsync(TitanInfoDto calcInfo)
        {
            return Task.Factory.StartNew(() => AddTitanCalcInfo(calcInfo));
        }

        public void AddTitanCalcInfo(TitanInfoDto calcInfo)
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

        public Task<List<TitanInfoDto>> GetTitanCalculatedInfoAsync()
        {
            return Task.Factory.StartNew(GetTitanCalculatedInfo);
        }

        public List<TitanInfoDto> GetTitanCalculatedInfo()
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