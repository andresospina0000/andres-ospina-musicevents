using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MusicEvents.Entities;
using MusicEvents.Entities.Complex;

namespace MusicEvents.DataAccess;
public class MusicalEventsDbContext : IdentityDbContext<MusicEventsUserIdentity>
{
    public MusicalEventsDbContext()
    {
        
    }

    public MusicalEventsDbContext(DbContextOptions<MusicalEventsDbContext> options)
    : base(options)
    {
        
    }

    public DbSet<Genre> Genres { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Esto es FLUENT API

        modelBuilder.Entity<Concert>()
            .Property(p => p.UnitPrice)
            .HasPrecision(8, 2);

        modelBuilder.Entity<Concert>()
            .Property(p => p.Place)
            .HasMaxLength(100);

        modelBuilder.Entity<Sale>()
            .Property(p => p.UnitPrice)
            .HasPrecision(8, 2);
        
        modelBuilder.Entity<Sale>()
            .Property(p => p.TotalSale)
            .HasPrecision(8, 2);

        modelBuilder.Entity<Sale>()
            .HasIndex(p => p.UserId, "IX_Sale_UserId");

        modelBuilder.Entity<SaleInfo>()
            .Property(p => p.TotalSale)
            .HasPrecision(8, 2);
        
        modelBuilder.Entity<SaleInfo>()
            .HasNoKey();

    }
}
