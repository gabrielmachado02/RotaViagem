using RotaViagem.Domain.Entities;
using Xunit;

namespace RotaViagem.Tests.Domain
{
    public class RotaDomainTests
    {
        private readonly List<Rota> _rotasDisponiveis;

        public RotaDomainTests()
        {
            _rotasDisponiveis = new List<Rota>
            {
                new Rota("GRU", "BRC", 10),
                new Rota("BRC", "SCL", 5),
                new Rota("GRU", "CDG", 75),
                new Rota("GRU", "SCL", 20),
                new Rota("GRU", "ORL", 56),
                new Rota("ORL", "CDG", 5),
                new Rota("SCL", "ORL", 20)
            };
        }

        [Theory]
        [InlineData("GRU", "CDG", 40)]
        [InlineData("BRC", "SCL", 5)]
        [InlineData("GRU", "ORL", 45)]
        public void EncontrarMelhorRota_DeveRetornarRotaMaisBarata(string origem, string destino, decimal custoEsperado)
        {
            var (caminho, custo) = Rota.EncontrarMelhorRota(origem, destino, _rotasDisponiveis);

            Assert.Equal(custoEsperado, custo);
            Assert.NotEmpty(caminho);
            Assert.Equal(origem, caminho.First());
            Assert.Equal(destino, caminho.Last());
        }

        [Fact]
        public void EncontrarMelhorRota_GRUparaCDG_DeveRetornarCaminhoCorreto()
        {
            var (caminho, custo) = Rota.EncontrarMelhorRota("GRU", "CDG", _rotasDisponiveis);

            Assert.Equal(40, custo);
            
            var caminhoEsperado = new List<string> { "GRU", "BRC", "SCL", "ORL", "CDG" };
            Assert.Equal(caminhoEsperado, caminho);
        }

        [Fact]
        public void EncontrarMelhorRota_OrigemInexistente_DeveRetornarCaminhoVazio()
        {
            var (caminho, custo) = Rota.EncontrarMelhorRota("XXX", "CDG", _rotasDisponiveis);

            Assert.Empty(caminho);
            Assert.Equal(0, custo);
        }

        [Fact]
        public void EncontrarMelhorRota_DestinoInexistente_DeveRetornarCaminhoVazio()
        {
            var (caminho, custo) = Rota.EncontrarMelhorRota("GRU", "XXX", _rotasDisponiveis);

            Assert.Empty(caminho);
            Assert.Equal(0, custo);
        }

        [Fact]
        public void EncontrarMelhorRota_DestinoInalcancavel_DeveRetornarCaminhoVazio()
        {
            var rotasIncompletas = new List<Rota>
            {
                new Rota("GRU", "BRC", 10),
                new Rota("BRC", "SCL", 5),
            };

            var (caminho, custo) = Rota.EncontrarMelhorRota("GRU", "CDG", rotasIncompletas);

            Assert.Empty(caminho);
            Assert.Equal(0, custo);
        }
    }
}