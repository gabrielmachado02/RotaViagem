namespace RotaViagem.Application.DTOs
{
    public class RotaInputDto
    {
        public string Origem { get; set; } = null!;
        public string Destino { get; set; } = null!;
        public decimal Valor { get; set; }
    }
}