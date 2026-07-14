using KreditService.Models;
using Microsoft.EntityFrameworkCore;

namespace KreditService.Data;

public class KreditDbContext : DbContext
{
    public KreditDbContext(DbContextOptions<KreditDbContext> options) : base(options) { }

    public DbSet<PengajuanKredit> PengajuanKredits { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<PengajuanKredit>(entity =>
        {
            // Apply requested explicit database schema indexing
            entity.HasIndex(e => e.Plafon).HasDatabaseName("idx_plafon");
            entity.HasIndex(e => e.Tenor).HasDatabaseName("idx_tenor");
            
            // Set up compatible default value fallbacks for PostgreSQL
            entity.Property(e => e.Id).HasDefaultValueSql("gen_random_uuid()");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.UpdatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
        });
    }
}