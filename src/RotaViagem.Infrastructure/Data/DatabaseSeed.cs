using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using RotaViagem.Domain.Entities;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace RotaViagem.Infrastructure.Data
{
    public static class DatabaseSeed
    {
        public static async Task SeedDatabaseAsync(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            //await dbContext.Database.MigrateAsync();

            if (await dbContext.Rotas.AnyAsync())
            {
                return;
            }

            var rotasIniciais = new[]
            {
                new Rota("GRU", "BRC", 10),
                new Rota("BRC", "SCL", 5),
                new Rota("GRU", "CDG", 75),
                new Rota("GRU", "SCL", 20),
                new Rota("GRU", "ORL", 56),
                new Rota("ORL", "CDG", 5),
                new Rota("SCL", "ORL", 20)
            };

            var rotasAdicionais = new[]
            {
                new Rota("CDG", "GRU", 85),
                new Rota("SCL", "BRC", 6),
                new Rota("SCL", "GRU", 22),
                new Rota("ORL", "GRU", 58),
                new Rota("CDG", "ORL", 6),
                
                new Rota("BRC", "ORL", 25),
                new Rota("BRC", "CDG", 60),
                
                new Rota("GRU", "MEX", 65),
                new Rota("MEX", "LAX", 35),
                new Rota("LAX", "JFK", 45),
                new Rota("JFK", "CDG", 40),
                new Rota("CDG", "FRA", 15),
                new Rota("FRA", "IST", 25)
            };

            var todasRotas = rotasIniciais.Concat(rotasAdicionais);

            await dbContext.Rotas.AddRangeAsync(todasRotas);
            await dbContext.SaveChangesAsync();
        }
    }
}