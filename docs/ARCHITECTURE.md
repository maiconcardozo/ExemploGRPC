# Arquitetura do ExemploGRPC

## Visão Geral

Este documento descreve a arquitetura do projeto ExemploGRPC, que implementa um servidor gRPC seguindo os princípios de **Clean Architecture**, **DDD (Domain-Driven Design)** e **SOLID**.

## Diagrama de Camadas

```
┌─────────────────────────────────────────────────────────────┐
│                     GRPC Layer                               │
│  (ExemploGRPC.GRPC - Apresentação)                          │
│  - Proto Files (.proto)                                      │
│  - GRPC Services (GrpcClienteService, etc)                   │
│  - Dependency Injection                                      │
└──────────────────┬──────────────────────────────────────────┘
                   │ Depende de
┌──────────────────▼──────────────────────────────────────────┐
│                Application Layer                             │
│  (ExemploGRPC.Application - Casos de Uso)                   │
│  - Services (ClienteService, CargoService, etc)              │
│  - DTOs (Data Transfer Objects)                              │
│  - Mappings (AutoMapper Profiles)                            │
└──────────────────┬──────────────────────────────────────────┘
                   │ Depende de
┌──────────────────▼──────────────────────────────────────────┐
│                   Domain Layer                               │
│  (ExemploGRPC.Domain - Núcleo do Negócio)                   │
│  - Entities (Cliente, Cargo, ClienteCargo)                   │
│  - Interfaces (Repositories, Unit of Work)                   │
│  - Business Rules & Validations                              │
└──────────────────┬──────────────────────────────────────────┘
                   │ Implementado por
┌──────────────────▼──────────────────────────────────────────┐
│              Infrastructure Layer                            │
│  (ExemploGRPC.Infrastructure - Detalhes Técnicos)           │
│  - DbContext (Entity Framework Core)                         │
│  - Repositories (Implementações Concretas)                   │
│  - Configurations (Fluent API)                               │
│  - Unit of Work                                              │
└─────────────────────────────────────────────────────────────┘
```

## Princípios Aplicados

### 1. Clean Architecture

A arquitetura segue o princípio da **Dependency Rule**:
- Camadas internas não conhecem camadas externas
- Domain não depende de ninguém
- Application depende apenas do Domain
- Infrastructure depende de Domain e Application
- GRPC depende de Application e Infrastructure

### 2. DDD (Domain-Driven Design)

**Entities (Entidades)**
- `EntityBase`: Classe base com ID, timestamps, soft delete
- `Cliente`: Aggregate Root para domínio de clientes
- `Cargo`: Aggregate Root para domínio de cargos
- `ClienteCargo`: Entity para relacionamento N-N

**Aggregates**
- Cliente é um agregado que gerencia suas próprias associações com cargos
- Cargo é um agregado que gerencia suas próprias associações com clientes

**Value Objects**
- Validações de CPF, Email implementadas nas entidades

**Repositories**
- `IClienteRepository`, `ICargoRepository`, `IClienteCargoRepository`
- Abstrações no domínio, implementações na infraestrutura

**Services**
- Domain Services: Lógica de negócio complexa
- Application Services: Orquestração de casos de uso

### 3. SOLID

**S - Single Responsibility Principle**
- Cada classe tem uma única responsabilidade
- Exemplo: `ClienteRepository` só gerencia persistência de Cliente

**O - Open/Closed Principle**
- Classes abertas para extensão, fechadas para modificação
- Exemplo: `EntityBase` pode ser estendido sem modificação

**L - Liskov Substitution Principle**
- Implementações podem substituir interfaces
- Exemplo: `ClienteRepository` substitui `IClienteRepository`

**I - Interface Segregation Principle**
- Interfaces específicas e coesas
- Exemplo: Cada repositório tem sua própria interface

**D - Dependency Inversion Principle**
- Dependência de abstrações, não de implementações
- Exemplo: Services dependem de interfaces, não de classes concretas

## Fluxo de Dados

### Exemplo: Criar um Cliente

```
1. Cliente gRPC → grpcurl/client
2. Request → GrpcClienteService (GRPC Layer)
3. DTO Mapping → CreateClienteDto
4. Service Call → ClienteService.CreateAsync (Application Layer)
5. Business Logic → Validações e regras de negócio
6. Entity Creation → new Cliente() (Domain Layer)
7. Repository Call → IClienteRepository.AddAsync
8. Persistence → ClienteRepository.AddAsync (Infrastructure Layer)
9. Database → Entity Framework Core → MySQL
10. Unit of Work → CommitAsync
11. Response Mapping → ClienteDto → ClienteResponse
12. Return → gRPC Response
```

## Estrutura de Pastas

```
ExemploGRPC/
├── Src/
│   ├── ExemploGRPC.Domain/
│   │   ├── Entities/
│   │   │   ├── Base/
│   │   │   │   └── EntityBase.cs
│   │   │   └── Implementation/
│   │   │       ├── Cliente.cs
│   │   │       ├── Cargo.cs
│   │   │       └── ClienteCargo.cs
│   │   └── Interfaces/
│   │       ├── IClienteRepository.cs
│   │       ├── ICargoRepository.cs
│   │       ├── IClienteCargoRepository.cs
│   │       └── IUnitOfWork.cs
│   │
│   ├── ExemploGRPC.Application/
│   │   ├── DTOs/
│   │   │   ├── ClienteDto.cs
│   │   │   ├── CargoDto.cs
│   │   │   └── ClienteCargoDto.cs
│   │   ├── Mappings/
│   │   │   └── MappingProfile.cs
│   │   └── Services/
│   │       ├── Interfaces/
│   │       │   ├── IClienteService.cs
│   │       │   ├── ICargoService.cs
│   │       │   └── IClienteCargoService.cs
│   │       └── Implementation/
│   │           ├── ClienteService.cs
│   │           ├── CargoService.cs
│   │           └── ClienteCargoService.cs
│   │
│   ├── ExemploGRPC.Infrastructure/
│   │   └── Data/
│   │       ├── ApplicationDbContext.cs
│   │       ├── Configurations/
│   │       │   ├── ClienteConfiguration.cs
│   │       │   ├── CargoConfiguration.cs
│   │       │   └── ClienteCargoConfiguration.cs
│   │       └── Repositories/
│   │           ├── ClienteRepository.cs
│   │           ├── CargoRepository.cs
│   │           ├── ClienteCargoRepository.cs
│   │           └── UnitOfWork.cs
│   │
│   ├── ExemploGRPC.GRPC/
│   │   ├── Protos/
│   │   │   ├── cliente.proto
│   │   │   ├── cargo.proto
│   │   │   └── clientecargo.proto
│   │   ├── Services/
│   │   │   ├── GrpcClienteService.cs
│   │   │   ├── GrpcCargoService.cs
│   │   │   └── GrpcClienteCargoService.cs
│   │   ├── Program.cs
│   │   └── appsettings.json
│   │
│   └── ExemploGRPC.Tests/
│       └── (Testes Unitários e de Integração)
│
├── Dockerfile
├── docker-compose.yml
└── README.md
```

## Padrões de Design Utilizados

### Repository Pattern
Abstração do acesso a dados, permitindo trocar a implementação sem afetar o domínio.

### Unit of Work Pattern
Gerenciamento de transações e coordenação de múltiplos repositórios.

### Dependency Injection
Injeção de dependências via container IoC do ASP.NET Core.

### DTO Pattern
Transferência de dados entre camadas sem expor entidades de domínio.

### Aggregate Pattern
Agrupamento de entidades relacionadas sob uma raiz.

## Tecnologias

- **.NET 8.0**: Framework base
- **gRPC**: Protocolo de comunicação
- **Entity Framework Core 8.0**: ORM
- **MySQL 8.0**: Banco de dados
- **AutoMapper 12.0**: Mapeamento objeto-objeto
- **Docker**: Containerização

## Considerações de Performance

1. **Lazy Loading Desabilitado**: Uso de Include explícito para evitar N+1 queries
2. **Paginação**: Suportada nos endpoints de listagem
3. **Soft Delete**: Filtros globais para melhor performance
4. **Connection Pooling**: Gerenciado pelo EF Core
5. **Docker Multi-Stage**: Build otimizado com camadas separadas

## Segurança

1. **Validação de Entrada**: Em todas as entidades
2. **Soft Delete**: Dados nunca são removidos fisicamente
3. **Sanitização**: CPF e Email validados
4. **Error Handling**: Exceções tratadas e convertidas para status gRPC apropriados

## Extensibilidade

O projeto foi desenvolvido para ser facilmente extensível:

1. **Novos Agregados**: Adicione novas entidades em Domain
2. **Novos Serviços**: Crie interfaces e implementações em Application
3. **Novos Endpoints**: Adicione novos .proto e services em GRPC
4. **Novos Repositórios**: Implemente interfaces em Infrastructure

## Próximos Passos

1. Implementar testes unitários completos
2. Adicionar testes de integração para gRPC
3. Implementar autenticação e autorização
4. Adicionar observabilidade (logs, métricas, traces)
5. Implementar cache (Redis)
6. Adicionar validações com FluentValidation
7. Implementar CQRS se necessário
