namespace RotaViagem.Domain.Entities
{
    public class Rota
    {
        public Rota()
        {
        }

        public Rota(string origem, string destino, decimal valor)
        {
            Id = Guid.NewGuid();
            Origem = origem;
            Destino = destino;
            Valor = valor;
        }

        public Guid Id { get; set; }
        public string Origem { get; set; } = null!;
        public string Destino { get; set; } = null!;
        public decimal Valor { get; set; }

        public static (List<string> Caminho, decimal CustoTotal) EncontrarMelhorRota(
            string origem, 
            string destino, 
            IEnumerable<Rota> rotasDisponiveis)
        {
            var grafo = ConstruirGrafo(rotasDisponiveis);
            
            if (!grafo.ContainsKey(origem) || !grafo.ContainsKey(destino))
            {
                return (new List<string>(), 0);
            }

            var custos = new Dictionary<string, decimal>();
            var pais = new Dictionary<string, string?>();
            var visitados = new HashSet<string>();
            var proximos = new PriorityQueue<string, decimal>();
            
            foreach (var aeroporto in grafo.Keys)
            {
                custos[aeroporto] = aeroporto == origem ? 0 : decimal.MaxValue;
                pais[aeroporto] = null;
            }
            
            proximos.Enqueue(origem, 0);
            
            while (proximos.Count > 0)
            {
                var atual = proximos.Dequeue();
                
                if (atual == destino)
                {
                    break;
                }
                
                if (visitados.Contains(atual))
                {
                    continue;
                }
                
                visitados.Add(atual);
                
                foreach (var (vizinho, custo) in grafo[atual])
                {
                    var novoCusto = custos[atual] + custo;
                    
                    if (novoCusto < custos[vizinho])
                    {
                        custos[vizinho] = novoCusto;
                        pais[vizinho] = atual;
                        proximos.Enqueue(vizinho, novoCusto);
                    }
                }
            }
            
            if (custos[destino] == decimal.MaxValue)
            {
                return (new List<string>(), 0);
            }
            
            var caminho = new List<string>();
            var atual2 = destino;
            
            while (atual2 != null)
            {
                caminho.Add(atual2);
                atual2 = pais[atual2];
            }
            
            caminho.Reverse();
            
            return (caminho, custos[destino]);
        }
        
        private static Dictionary<string, List<(string, decimal)>> ConstruirGrafo(IEnumerable<Rota> rotas)
        {
            var grafo = new Dictionary<string, List<(string, decimal)>>();
            
            foreach (var rota in rotas)
            {
                if (!grafo.ContainsKey(rota.Origem))
                {
                    grafo[rota.Origem] = new List<(string, decimal)>();
                }
                
                grafo[rota.Origem].Add((rota.Destino, rota.Valor));
                
                if (!grafo.ContainsKey(rota.Destino))
                {
                    grafo[rota.Destino] = new List<(string, decimal)>();
                }
            }
            
            return grafo;
        }
    }
}