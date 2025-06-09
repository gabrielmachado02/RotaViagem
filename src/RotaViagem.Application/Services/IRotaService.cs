using RotaViagem.Application.DTOs;

namespace RotaViagem.Application.Services
{
    public interface IRotaService
    {
        Task<IEnumerable<RotaDto>> GetAllRotasAsync();
        Task<RotaDto?> GetRotaByIdAsync(Guid id);
        Task<RotaDto> CreateRotaAsync(RotaInputDto rotaDto);
        Task UpdateRotaAsync(Guid id, RotaInputDto rotaDto);
        Task DeleteRotaAsync(Guid id);
        Task<MelhorRotaResultadoDto> ConsultarMelhorRotaAsync(ConsultaRotaDto consultaDto);
    }
}