using ExemploGRPC.Application.DTOs;

namespace ExemploGRPC.Application.Services.Interfaces;

/// <summary>
/// Service interface for Cargo operations
/// </summary>
public interface ICargoService
{
    Task<CargoDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<CargoDto?> GetByNomeAsync(string nome, CancellationToken cancellationToken = default);
    Task<IEnumerable<CargoDto>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<CargoDto>> GetPagedAsync(int pageNumber, int pageSize, CancellationToken cancellationToken = default);
    Task<CargoDto> CreateAsync(CreateCargoDto dto, CancellationToken cancellationToken = default);
    Task<CargoDto> UpdateAsync(Guid id, UpdateCargoDto dto, CancellationToken cancellationToken = default);
    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}
