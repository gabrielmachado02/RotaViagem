using Microsoft.EntityFrameworkCore;
using RotaViagem.Domain.Entities;
using RotaViagem.Domain.Interfaces;
using RotaViagem.Infrastructure.Data;

namespace RotaViagem.Infrastructure.Repositories
{
    public class RotaRepository : IRotaRepository
    {
        private readonly ApplicationDbContext _context;

        public RotaRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Rota?> GetByIdAsync(Guid id)
        {
            return await _context.Rotas.FindAsync(id);
        }

        public async Task<IEnumerable<Rota>> GetAllAsync()
        {
            return await _context.Rotas.ToListAsync();
        }

        public async Task<Rota> AddAsync(Rota rota)
        {
            _context.Rotas.Add(rota);
            await _context.SaveChangesAsync();
            return rota;
        }

        public async Task UpdateAsync(Rota rota)
        {
            _context.Entry(rota).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var rota = await _context.Rotas.FindAsync(id);
            if (rota != null)
            {
                _context.Rotas.Remove(rota);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Rota>> GetRotasDisponiveisAsync()
        {
            return await _context.Rotas.ToListAsync();
        }
    }
}