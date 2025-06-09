using RotaViagem.Domain.Entities;

namespace RotaViagem.Domain.Interfaces
{
    public interface IRotaRepository
    {
        Task<Rota?> GetByIdAsync(Guid id);
        Task<IEnumerable<Rota>> GetAllAsync();
        Task<Rota> AddAsync(Rota rota);
        Task UpdateAsync(Rota rota);
        Task DeleteAsync(Guid id);
        Task<IEnumerable<Rota>> GetRotasDisponiveisAsync();
    }
}