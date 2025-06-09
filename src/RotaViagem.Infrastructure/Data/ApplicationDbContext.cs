using Microsoft.EntityFrameworkCore;
using RotaViagem.Domain.Entities;

namespace RotaViagem.Infrastructure.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Rota> Rotas { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Rota>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Origem).HasMaxLength(3).IsRequired();
                entity.Property(e => e.Destino).HasMaxLength(3).IsRequired();
                entity.Property(e => e.Valor).HasPrecision(18, 2).IsRequired();
            });
        }
    }
}