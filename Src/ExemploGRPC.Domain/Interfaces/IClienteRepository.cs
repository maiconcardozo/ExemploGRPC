using ExemploGRPC.Domain.Entities.Implementation;

namespace ExemploGRPC.Domain.Interfaces;

/// <summary>
/// Repository interface for Cliente entity
/// Follows Repository pattern from DDD
/// </summary>
public interface IClienteRepository
{
    /// <summary>
    /// Gets a cliente by ID
    /// </summary>
    Task<Cliente?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets a cliente by CPF
    /// </summary>
    Task<Cliente?> GetByCpfAsync(string cpf, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets all clientes
    /// </summary>
    Task<IEnumerable<Cliente>> GetAllAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets clientes with pagination
    /// </summary>
    Task<IEnumerable<Cliente>> GetPagedAsync(int pageNumber, int pageSize, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets clientes by cargo ID
    /// </summary>
    Task<IEnumerable<Cliente>> GetByCargoIdAsync(Guid cargoId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Adds a new cliente
    /// </summary>
    Task AddAsync(Cliente cliente, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates an existing cliente
    /// </summary>
    Task UpdateAsync(Cliente cliente, CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes a cliente
    /// </summary>
    Task DeleteAsync(Cliente cliente, CancellationToken cancellationToken = default);

    /// <summary>
    /// Checks if a cliente with the given CPF exists
    /// </summary>
    Task<bool> ExistsByCpfAsync(string cpf, CancellationToken cancellationToken = default);
}
