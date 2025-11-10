# ExemploGRPC

[![.NET 8.0](https://img.shields.io/badge/.NET-8.0-blue.svg?logo=dotnet)](https://dotnet.microsoft.com/download/dotnet/8.0)
[![gRPC](https://img.shields.io/badge/gRPC-2.66.0-green.svg)](https://grpc.io/)
[![MySQL](https://img.shields.io/badge/MySQL-8.0-orange.svg)](https://www.mysql.com/)
[![Docker](https://img.shields.io/badge/Docker-Ready-blue.svg)](https://www.docker.com/)

## 📋 Sobre o Projeto

Exemplo completo de um servidor **gRPC** em .NET 8.0 seguindo os padrões **DDD (Domain-Driven Design)**, **SOLID** e **Clean Architecture**, baseado no template [maiconcardozo/CleanTemplateRepository](https://github.com/maiconcardozo/CleanTemplateRepository).

O projeto implementa um CRUD completo para gerenciar **Clientes**, **Cargos** e suas **associações (N para N)** via gRPC.

### 🎯 Objetivos

- ✅ Demonstrar arquitetura limpa em projetos gRPC
- ✅ Implementar padrões DDD (Entities, Aggregates, Repositories, Services)
- ✅ Aplicar princípios SOLID
- ✅ Separação clara de responsabilidades em camadas
- ✅ Pronto para produção com Docker

## 🏗️ Arquitetura

O projeto está organizado em camadas seguindo Clean Architecture:

```
ExemploGRPC/
├── Src/
│   ├── ExemploGRPC.Domain/          # Camada de Domínio (Entities, Interfaces)
│   ├── ExemploGRPC.Application/     # Camada de Aplicação (Services, DTOs)
│   ├── ExemploGRPC.Infrastructure/  # Camada de Infraestrutura (EF Core, Repositories)
│   ├── ExemploGRPC.GRPC/           # Camada de Apresentação (gRPC Services)
│   └── ExemploGRPC.Tests/          # Testes Unitários e de Integração
├── Solution/
│   └── ExemploGRPC.sln             # Arquivo de solução
├── build/
│   └── Directory.Build.props       # Configurações de build
├── docs/                           # Documentação
├── scripts/                        # Scripts de inicialização
├── Dockerfile                      # Multi-stage Dockerfile
└── docker-compose.yml              # Orquestração de containers
```

### 📦 Camadas

#### **Domain Layer** (ExemploGRPC.Domain)
- **Entities**: Cliente, Cargo, ClienteCargo (com padrão Entity Base)
- **Interfaces**: IClienteRepository, ICargoRepository, IClienteCargoRepository, IUnitOfWork
- **Value Objects**: Validações de CPF, Email
- **Aggregates**: Cliente e Cargo como raízes de agregados

#### **Application Layer** (ExemploGRPC.Application)
- **DTOs**: ClienteDto, CargoDto, ClienteCargoDto
- **Services**: ClienteService, CargoService, ClienteCargoService
- **Mappings**: AutoMapper profiles
- **Business Logic**: Regras de negócio isoladas

#### **Infrastructure Layer** (ExemploGRPC.Infrastructure)
- **DbContext**: ApplicationDbContext com EF Core
- **Repositories**: Implementações concretas dos repositórios
- **Configurations**: Fluent API para configuração de entidades
- **Unit of Work**: Gerenciamento de transações

#### **gRPC Layer** (ExemploGRPC.GRPC)
- **Proto Files**: cliente.proto, cargo.proto, clientecargo.proto
- **Services**: GrpcClienteService, GrpcCargoService, GrpcClienteCargoService
- **Dependency Injection**: Configuração de serviços

## 🚀 Começando

### Pré-requisitos

- [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [Docker](https://www.docker.com/) e [Docker Compose](https://docs.docker.com/compose/)
- [MySQL 8.0](https://www.mysql.com/) (opcional, pode usar Docker)

### 🐳 Executando com Docker (Recomendado)

1. **Clone o repositório**
```bash
git clone https://github.com/maiconcardozo/ExemploGRPC.git
cd ExemploGRPC
```

2. **Inicie os containers com Docker Compose**
```bash
docker-compose up -d
```

Isso irá:
- ✅ Criar o banco de dados MySQL
- ✅ Compilar a aplicação .NET
- ✅ Iniciar o servidor gRPC na porta 8080

3. **Verificar os logs**
```bash
docker-compose logs -f grpc-server
```

4. **Parar os containers**
```bash
docker-compose down
```

Para remover também os volumes (dados do banco):
```bash
docker-compose down -v
```

### 💻 Executando Localmente

1. **Clone o repositório**
```bash
git clone https://github.com/maiconcardozo/ExemploGRPC.git
cd ExemploGRPC
```

2. **Configure o banco de dados**

Edite o arquivo `Src/ExemploGRPC.GRPC/appsettings.json` com sua connection string:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Port=3306;Database=exemplograpc;User=root;Password=sua-senha;"
  }
}
```

3. **Restaure os pacotes**
```bash
cd Solution
dotnet restore
```

4. **Crie o banco de dados com migrations**
```bash
cd ../Src/ExemploGRPC.Infrastructure
dotnet ef migrations add InitialCreate --startup-project ../ExemploGRPC.GRPC
dotnet ef database update --startup-project ../ExemploGRPC.GRPC
```

5. **Execute a aplicação**
```bash
cd ../ExemploGRPC.GRPC
dotnet run
```

O servidor gRPC estará disponível em `http://localhost:8080`

## 📡 Endpoints gRPC

### Cliente Service

| Método | Descrição |
|--------|-----------|
| `GetCliente` | Busca cliente por ID |
| `GetClienteByCpf` | Busca cliente por CPF |
| `GetAllClientes` | Lista todos os clientes (com paginação) |
| `CreateCliente` | Cria novo cliente |
| `UpdateCliente` | Atualiza cliente existente |
| `DeleteCliente` | Remove cliente (soft delete) |
| `AddCargoToCliente` | Associa um cargo a um cliente |
| `RemoveCargoFromCliente` | Remove associação entre cliente e cargo |

### Cargo Service

| Método | Descrição |
|--------|-----------|
| `GetCargo` | Busca cargo por ID |
| `GetCargoByNome` | Busca cargo por nome |
| `GetAllCargos` | Lista todos os cargos (com paginação) |
| `CreateCargo` | Cria novo cargo |
| `UpdateCargo` | Atualiza cargo existente |
| `DeleteCargo` | Remove cargo (soft delete) |

### ClienteCargo Service

| Método | Descrição |
|--------|-----------|
| `GetClienteCargo` | Busca associação por ID |
| `GetClienteCargosByClienteId` | Lista associações de um cliente |
| `GetClienteCargosByCargoId` | Lista associações de um cargo |
| `GetAllClienteCargos` | Lista todas as associações |
| `CreateClienteCargo` | Cria nova associação |
| `UpdateClienteCargo` | Atualiza associação existente |
| `DeleteClienteCargo` | Remove associação |

## 🔧 Testando com grpcurl

Instale o [grpcurl](https://github.com/fullstorydev/grpcurl):

```bash
# Linux/Mac
brew install grpcurl

# Windows (com Chocolatey)
choco install grpcurl
```

### Exemplos de Requisições

**1. Criar um Cliente**
```bash
grpcurl -plaintext -d '{
  "nome": "João Silva",
  "email": "joao.silva@example.com",
  "cpf": "12345678901",
  "telefone": "11999999999"
}' localhost:8080 cliente.ClienteService/CreateCliente
```

**2. Listar Clientes**
```bash
grpcurl -plaintext -d '{
  "page_number": 1,
  "page_size": 10
}' localhost:8080 cliente.ClienteService/GetAllClientes
```

**3. Criar um Cargo**
```bash
grpcurl -plaintext -d '{
  "nome": "Desenvolvedor Senior",
  "descricao": "Desenvolvedor com mais de 5 anos de experiência",
  "nivel_salarial": 10000.00
}' localhost:8080 cargo.CargoService/CreateCargo
```

**4. Associar Cargo a Cliente**
```bash
grpcurl -plaintext -d '{
  "cliente_id": "guid-do-cliente",
  "cargo_id": "guid-do-cargo"
}' localhost:8080 cliente.ClienteService/AddCargoToCliente
```

## 🛠️ Tecnologias Utilizadas

- **.NET 8.0** - Framework principal
- **gRPC** - Protocolo de comunicação
- **Entity Framework Core 8.0** - ORM
- **MySQL 8.0** - Banco de dados
- **AutoMapper** - Mapeamento objeto-objeto
- **Docker** - Containerização
- **xUnit** - Framework de testes (em desenvolvimento)

## 🏛️ Padrões e Princípios

### DDD (Domain-Driven Design)
- ✅ Entities com validação
- ✅ Aggregates (Cliente, Cargo)
- ✅ Value Objects (validações)
- ✅ Repositories
- ✅ Domain Services

### SOLID
- **S**ingle Responsibility: Cada classe tem uma única responsabilidade
- **O**pen/Closed: Aberto para extensão, fechado para modificação
- **L**iskov Substitution: Interfaces bem definidas
- **I**nterface Segregation: Interfaces específicas
- **D**ependency Inversion: Dependência de abstrações

### Clean Architecture
- ✅ Independência de frameworks
- ✅ Testável
- ✅ Independência de UI
- ✅ Independência de banco de dados
- ✅ Regras de negócio isoladas

## 📝 Comandos Docker Úteis

**Build da imagem**
```bash
docker build -t exemplograpc:latest .
```

**Executar apenas o banco de dados**
```bash
docker-compose up -d mysql
```

**Ver logs em tempo real**
```bash
docker-compose logs -f
```

**Reconstruir e iniciar**
```bash
docker-compose up -d --build
```

**Parar e remover tudo**
```bash
docker-compose down -v
```

## 🧪 Testes

*(Em desenvolvimento)*

Execute os testes unitários:
```bash
cd Solution
dotnet test
```

## 📚 Documentação Adicional

- [Documentação do gRPC](https://grpc.io/docs/)
- [Clean Architecture](https://blog.cleancoder.com/uncle-bob/2012/08/13/the-clean-architecture.html)
- [Domain-Driven Design](https://martinfowler.com/bliki/DomainDrivenDesign.html)
- [SOLID Principles](https://en.wikipedia.org/wiki/SOLID)

## 🤝 Contribuindo

Contribuições são bem-vindas! Sinta-se à vontade para abrir issues ou pull requests.

1. Fork o projeto
2. Crie uma branch para sua feature (`git checkout -b feature/MinhaFeature`)
3. Commit suas mudanças (`git commit -m 'Adiciona MinhaFeature'`)
4. Push para a branch (`git push origin feature/MinhaFeature`)
5. Abra um Pull Request

## 📄 Licença

Este projeto está sob a licença MIT. Veja o arquivo [LICENSE](LICENSE) para mais detalhes.

## ✨ Autor

**Maicon Cardozo** ([@maiconcardozo](https://github.com/maiconcardozo))

---

⭐ Se este projeto foi útil, considere dar uma estrela!