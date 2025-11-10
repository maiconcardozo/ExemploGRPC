using ExemploGRPC.Application.DTOs;

namespace ExemploGRPC.Application.Services.Interfaces;

/// <summary>
/// Service interface for ClienteCargo operations
/// </summary>
public interface IClienteCargoService
{
    Task<ClienteCargoDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IEnumerable<ClienteCargoDto>> GetByClienteIdAsync(Guid clienteId, CancellationToken cancellationToken = default);
    Task<IEnumerable<ClienteCargoDto>> GetByCargoIdAsync(Guid cargoId, CancellationToken cancellationToken = default);
    Task<IEnumerable<ClienteCargoDto>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<ClienteCargoDto> CreateAsync(CreateClienteCargoDto dto, CancellationToken cancellationToken = default);
    Task<ClienteCargoDto> UpdateAsync(Guid id, UpdateClienteCargoDto dto, CancellationToken cancellationToken = default);
    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}
