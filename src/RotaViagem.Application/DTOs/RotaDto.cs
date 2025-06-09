namespace RotaViagem.Application.DTOs
{
    public class RotaDto
    {
        public Guid Id { get; set; }
        public string Origem { get; set; } = null!;
        public string Destino { get; set; } = null!;
        public decimal Valor { get; set; }
    }
}