using ExemploGRPC.Application.DTOs;

namespace ExemploGRPC.Application.Services.Interfaces;

/// <summary>
/// Service interface for Cliente operations
/// </summary>
public interface IClienteService
{
    Task<ClienteDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<ClienteDto?> GetByCpfAsync(string cpf, CancellationToken cancellationToken = default);
    Task<IEnumerable<ClienteDto>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<ClienteDto>> GetPagedAsync(int pageNumber, int pageSize, CancellationToken cancellationToken = default);
    Task<ClienteDto> CreateAsync(CreateClienteDto dto, CancellationToken cancellationToken = default);
    Task<ClienteDto> UpdateAsync(Guid id, UpdateClienteDto dto, CancellationToken cancellationToken = default);
    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
    Task AddCargoAsync(Guid clienteId, Guid cargoId, CancellationToken cancellationToken = default);
    Task RemoveCargoAsync(Guid clienteId, Guid cargoId, CancellationToken cancellationToken = default);
}
