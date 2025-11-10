using ExemploGRPC.Domain.DTOs;

namespace ExemploGRPC.Application.Services.Interfaces;

/// <summary>
/// Service interface for Cargo operations
/// </summary>
public interface ICargoService
{
    Task<CargoDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<CargoDto?> GetByNomeAsync(string nome, CancellationToken cancellationToken = default);
    Task<IEnumerable<CargoDto>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<CargoDto>> GetPagedAsync(int pageNumber, int pageSize, CancellationToken cancellationToken = default);
    Task<CargoDto> CreateAsync(CreateCargoDto dto, CancellationToken cancellationToken = default);
    Task<CargoDto> UpdateAsync(int id, UpdateCargoDto dto, CancellationToken cancellationToken = default);
    Task DeleteAsync(int id, CancellationToken cancellationToken = default);
}
