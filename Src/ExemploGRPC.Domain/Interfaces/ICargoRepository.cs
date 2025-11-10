using ExemploGRPC.Domain.Entities.Implementation;

namespace ExemploGRPC.Domain.Interfaces;

/// <summary>
/// Repository interface for Cargo entity
/// Follows Repository pattern from DDD
/// </summary>
public interface ICargoRepository
{
    /// <summary>
    /// Gets a cargo by ID
    /// </summary>
    Task<Cargo?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets a cargo by name
    /// </summary>
    Task<Cargo?> GetByNomeAsync(string nome, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets all cargos
    /// </summary>
    Task<IEnumerable<Cargo>> GetAllAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets cargos with pagination
    /// </summary>
    Task<IEnumerable<Cargo>> GetPagedAsync(int pageNumber, int pageSize, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets cargos by cliente ID
    /// </summary>
    Task<IEnumerable<Cargo>> GetByClienteIdAsync(Guid clienteId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Adds a new cargo
    /// </summary>
    Task AddAsync(Cargo cargo, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates an existing cargo
    /// </summary>
    Task UpdateAsync(Cargo cargo, CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes a cargo
    /// </summary>
    Task DeleteAsync(Cargo cargo, CancellationToken cancellationToken = default);

    /// <summary>
    /// Checks if a cargo with the given name exists
    /// </summary>
    Task<bool> ExistsByNomeAsync(string nome, CancellationToken cancellationToken = default);
}
