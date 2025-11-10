using ExemploGRPC.Application.DTOs;

namespace ExemploGRPC.Application.Services.Interfaces;

/// <summary>
/// Service interface for ClienteCargo operations
/// </summary>
public interface IClienteCargoService
{
    Task<ClienteCargoDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<IEnumerable<ClienteCargoDto>> GetByClienteIdAsync(int clienteId, CancellationToken cancellationToken = default);
    Task<IEnumerable<ClienteCargoDto>> GetByCargoIdAsync(int cargoId, CancellationToken cancellationToken = default);
    Task<IEnumerable<ClienteCargoDto>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<ClienteCargoDto> CreateAsync(CreateClienteCargoDto dto, CancellationToken cancellationToken = default);
    Task<ClienteCargoDto> UpdateAsync(int id, UpdateClienteCargoDto dto, CancellationToken cancellationToken = default);
    Task DeleteAsync(int id, CancellationToken cancellationToken = default);
}
