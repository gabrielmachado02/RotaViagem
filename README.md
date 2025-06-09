# Rota Viagem
rode a migration com comando ef database update
Aplicação para encontrar a rota de viagem mais barata independente da quantidade de conexões.

## Estrutura do Projeto

Este projeto segue a arquitetura Clean Architecture:

- **RotaViagem.Domain**: Contém as entidades e regras de negócio
- **RotaViagem.Application**: Contém os serviços e DTOs da aplicação
- **RotaViagem.Infrastructure**: Contém a implementação de persistência e serviços externos
- **RotaViagem.DependencyInjection**: Gerencia as dependências entre as camadas
- **RotaViagem.API**: Projeto WebAPI com controllers REST

## Funcionalidades

1. CRUD de rotas de viagem (origem, destino, valor)
2. Consulta da melhor rota (mais barata) entre dois pontos

## Como executar

### Pré-requisitos
- .NET 8 SDK
- Docker e Docker Compose

### Passos para execução

1. Clone o repositório
2. Na pasta raiz do projeto, execute:
   ```
   docker-compose up -d
   ```
3. Acesse a API em: http://localhost:5000/swagger

## Exemplos de uso

- Consulta da rota: GRU-CDG
  - Resposta: GRU - BRC - SCL - ORL - CDG ao custo de $40
- Consulta da rota: BRC-SCL
  - Resposta: BRC - SCL ao custo de $5
  
## Rotas pré-cadastradas

O sistema é inicializado com 20 rotas pré-cadastradas, incluindo:

### Rotas iniciais (conforme requisito)
- Origem: GRU, Destino: BRC, Valor: 10
- Origem: BRC, Destino: SCL, Valor: 5
- Origem: GRU, Destino: CDG, Valor: 75
- Origem: GRU, Destino: SCL, Valor: 20
- Origem: GRU, Destino: ORL, Valor: 56
- Origem: ORL, Destino: CDG, Valor: 5
- Origem: SCL, Destino: ORL, Valor: 20

### Rotas adicionais
- Origem: CDG, Destino: GRU, Valor: 85
- Origem: SCL, Destino: BRC, Valor: 6
- Origem: SCL, Destino: GRU, Valor: 22
- Origem: ORL, Destino: GRU, Valor: 58
- Origem: CDG, Destino: ORL, Valor: 6
- Origem: BRC, Destino: ORL, Valor: 25
- Origem: BRC, Destino: CDG, Valor: 60
- Origem: GRU, Destino: MEX, Valor: 65
- Origem: MEX, Destino: LAX, Valor: 35
- Origem: LAX, Destino: JFK, Valor: 45
- Origem: JFK, Destino: CDG, Valor: 40
- Origem: CDG, Destino: FRA, Valor: 15
- Origem: FRA, Destino: IST, Valor: 25