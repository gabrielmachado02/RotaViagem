using FluentValidation;
using FluentValidation.Results;
using Moq;
using RotaViagem.Application.DTOs;
using RotaViagem.Application.Services;
using RotaViagem.Domain.Entities;
using RotaViagem.Domain.Interfaces;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace RotaViagem.Tests.Application
{
    public class RotaServiceTests
    {
        private readonly Mock<IRotaRepository> _repositoryMock;
        private readonly Mock<IValidator<RotaInputDto>> _rotaValidatorMock;
        private readonly Mock<IValidator<ConsultaRotaDto>> _consultaValidatorMock;
        private readonly IRotaService _rotaService;

        public RotaServiceTests()
        {
            _repositoryMock = new Mock<IRotaRepository>();
            _rotaValidatorMock = new Mock<IValidator<RotaInputDto>>();
            _consultaValidatorMock = new Mock<IValidator<ConsultaRotaDto>>();
            
            _rotaService = new RotaService(
                _repositoryMock.Object,
                _rotaValidatorMock.Object,
                _consultaValidatorMock.Object);

            _rotaValidatorMock.Setup(v => v.ValidateAsync(It.IsAny<RotaInputDto>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ValidationResult());
            
            _consultaValidatorMock.Setup(v => v.ValidateAsync(It.IsAny<ConsultaRotaDto>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ValidationResult());
        }

        [Fact]
        public async Task GetAllRotasAsync_DeveRetornarTodasRotas()
        {
            var rotasDb = new List<Rota>
            {
                new Rota("GRU", "BRC", 10),
                new Rota("BRC", "SCL", 5)
            };

            _repositoryMock.Setup(r => r.GetAllAsync())
                .ReturnsAsync(rotasDb);

            var result = await _rotaService.GetAllRotasAsync();

            var rotasList = Assert.IsAssignableFrom<IEnumerable<RotaDto>>(result);
            Assert.Equal(2, rotasList.Count());
        }

        [Fact]
        public async Task CreateRotaAsync_ComDadosValidos_DeveRetornarRotaCriada()
        {
            var rotaDto = new RotaInputDto
            {
                Origem = "GRU",
                Destino = "SCL",
                Valor = 20
            };

            var rotaDb = new Rota("GRU", "SCL", 20);
            _repositoryMock.Setup(r => r.AddAsync(It.IsAny<Rota>()))
                .ReturnsAsync(rotaDb);

            var result = await _rotaService.CreateRotaAsync(rotaDto);

            Assert.NotNull(result);
            Assert.Equal(rotaDto.Origem, result.Origem);
            Assert.Equal(rotaDto.Destino, result.Destino);
            Assert.Equal(rotaDto.Valor, result.Valor);
            _repositoryMock.Verify(r => r.AddAsync(It.IsAny<Rota>()), Times.Once);
        }

        [Fact]
        public async Task UpdateRotaAsync_RotaExistente_DeveAtualizarRota()
        {
            var id = Guid.NewGuid();
            var rotaDto = new RotaInputDto
            {
                Origem = "GRU",
                Destino = "SCL",
                Valor = 20
            };

            var rotaDb = new Rota("GRU", "BRC", 10) { Id = id };
            _repositoryMock.Setup(r => r.GetByIdAsync(id))
                .ReturnsAsync(rotaDb);

            await _rotaService.UpdateRotaAsync(id, rotaDto);

            _repositoryMock.Verify(r => r.UpdateAsync(It.Is<Rota>(rota => 
                rota.Id == id && 
                rota.Origem == rotaDto.Origem && 
                rota.Destino == rotaDto.Destino && 
                rota.Valor == rotaDto.Valor)), Times.Once);
        }

        [Fact]
        public async Task DeleteRotaAsync_RotaExistente_DeveDeletarRota()
        {
            var id = Guid.NewGuid();
            var rotaDb = new Rota("GRU", "BRC", 10) { Id = id };
            _repositoryMock.Setup(r => r.GetByIdAsync(id))
                .ReturnsAsync(rotaDb);

            await _rotaService.DeleteRotaAsync(id);

            _repositoryMock.Verify(r => r.DeleteAsync(id), Times.Once);
        }

        [Fact]
        public async Task ConsultarMelhorRotaAsync_ComRotasValidas_DeveRetornarMelhorRota()
        {
            var rotasDisponiveis = new List<Rota>
            {
                new Rota("GRU", "BRC", 10),
                new Rota("BRC", "SCL", 5),
                new Rota("GRU", "CDG", 75),
                new Rota("GRU", "SCL", 20),
                new Rota("GRU", "ORL", 56),
                new Rota("ORL", "CDG", 5),
                new Rota("SCL", "ORL", 20)
            };

            _repositoryMock.Setup(r => r.GetAllAsync())
                .ReturnsAsync(rotasDisponiveis);

            var consultaDto = new ConsultaRotaDto { Origem = "GRU", Destino = "CDG" };

            var result = await _rotaService.ConsultarMelhorRotaAsync(consultaDto);

            Assert.Equal(40, result.CustoTotal);
            Assert.Equal(5, result.Caminho.Count);
            Assert.Equal("GRU - BRC - SCL - ORL - CDG", result.CaminhoFormatado);
            Assert.Equal("GRU - BRC - SCL - ORL - CDG ao custo de $40", result.ResultadoCompleto);
        }
    }
}