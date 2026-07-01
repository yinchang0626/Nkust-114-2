using FinancialCsvWeb.Models;
using Microsoft.EntityFrameworkCore;

namespace FinancialCsvWeb.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<FinancialReport> FinancialReports { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<FinancialReport>(entity =>
            {
                entity.HasIndex(x => new { x.Year, x.Quarter, x.CompanyCode })
                    .IsUnique();

                entity.HasIndex(x => x.CompanyName);
                entity.HasIndex(x => x.TotalAssets);
                entity.HasIndex(x => x.NetWorthPerShare);

                entity.Property(x => x.CashAndCashEquivalents).HasPrecision(28, 2);
                entity.Property(x => x.TotalAssets).HasPrecision(28, 2);
                entity.Property(x => x.TotalLiabilities).HasPrecision(28, 2);
                entity.Property(x => x.CapitalStock).HasPrecision(28, 2);
                entity.Property(x => x.CapitalSurplus).HasPrecision(28, 2);
                entity.Property(x => x.RetainedEarnings).HasPrecision(28, 2);
                entity.Property(x => x.TotalEquity).HasPrecision(28, 2);
                entity.Property(x => x.NetWorthPerShare).HasPrecision(28, 2);
            });
        }
    }
}