using FluentValidation;
using RotaViagem.Application.DTOs;
using RotaViagem.Domain.Entities;
using RotaViagem.Domain.Interfaces;

namespace RotaViagem.Application.Services
{
    public class RotaService : IRotaService
    {
        private readonly IRotaRepository _rotaRepository;
        private readonly IValidator<RotaInputDto> _rotaValidator;
        private readonly IValidator<ConsultaRotaDto> _consultaValidator;

        public RotaService(
            IRotaRepository rotaRepository,
            IValidator<RotaInputDto> rotaValidator,
            IValidator<ConsultaRotaDto> consultaValidator)
        {
            _rotaRepository = rotaRepository;
            _rotaValidator = rotaValidator;
            _consultaValidator = consultaValidator;
        }

        public async Task<RotaDto> CreateRotaAsync(RotaInputDto rotaDto)
        {
            var validationResult = await _rotaValidator.ValidateAsync(rotaDto);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            var rota = new Rota(rotaDto.Origem, rotaDto.Destino, rotaDto.Valor);
            var novRota = await _rotaRepository.AddAsync(rota);

            return new RotaDto
            {
                Id = novRota.Id,
                Origem = novRota.Origem,
                Destino = novRota.Destino,
                Valor = novRota.Valor
            };
        }

        public async Task DeleteRotaAsync(Guid id)
        {
            var rota = await _rotaRepository.GetByIdAsync(id);
            if (rota == null)
            {
                throw new KeyNotFoundException($"Rota com ID {id} não encontrada.");
            }

            await _rotaRepository.DeleteAsync(id);
        }

        public async Task<IEnumerable<RotaDto>> GetAllRotasAsync()
        {
            var rotas = await _rotaRepository.GetAllAsync();
            return rotas.Select(r => new RotaDto
            {
                Id = r.Id,
                Origem = r.Origem,
                Destino = r.Destino,
                Valor = r.Valor
            });
        }

        public async Task<RotaDto?> GetRotaByIdAsync(Guid id)
        {
            var rota = await _rotaRepository.GetByIdAsync(id);
            if (rota == null)
            {
                return null;
            }

            return new RotaDto
            {
                Id = rota.Id,
                Origem = rota.Origem,
                Destino = rota.Destino,
                Valor = rota.Valor
            };
        }

        public async Task UpdateRotaAsync(Guid id, RotaInputDto rotaDto)
        {
            var validationResult = await _rotaValidator.ValidateAsync(rotaDto);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            var rota = await _rotaRepository.GetByIdAsync(id);
            if (rota == null)
            {
                throw new KeyNotFoundException($"Rota com ID {id} não encontrada.");
            }

            rota.Origem = rotaDto.Origem;
            rota.Destino = rotaDto.Destino;
            rota.Valor = rotaDto.Valor;

            await _rotaRepository.UpdateAsync(rota);
        }

        public async Task<MelhorRotaResultadoDto> ConsultarMelhorRotaAsync(ConsultaRotaDto consultaDto)
        {
            var validationResult = await _consultaValidator.ValidateAsync(consultaDto);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            var rotasDisponiveis = await _rotaRepository.GetAllAsync();
            var (caminho, custo) = Rota.EncontrarMelhorRota(
                consultaDto.Origem,
                consultaDto.Destino,
                rotasDisponiveis);

            if (caminho.Count == 0)
            {
                return MelhorRotaResultadoDto.Empty();
            }

            return new MelhorRotaResultadoDto(caminho, custo);
        }
    }
}