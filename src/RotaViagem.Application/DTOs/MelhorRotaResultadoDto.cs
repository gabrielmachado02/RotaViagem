namespace RotaViagem.Application.DTOs
{
    public class MelhorRotaResultadoDto
    {
        public List<string> Caminho { get; set; } = new List<string>();
        public decimal CustoTotal { get; set; }
        public string CaminhoFormatado => string.Join(" - ", Caminho);
        public string ResultadoCompleto => $"{CaminhoFormatado} ao custo de ${CustoTotal}";

        public MelhorRotaResultadoDto(List<string> caminho, decimal custoTotal)
        {
            Caminho = caminho;
            CustoTotal = custoTotal;
        }

        public static MelhorRotaResultadoDto Empty()
        {
            return new MelhorRotaResultadoDto(new List<string>(), 0);
        }
    }
}