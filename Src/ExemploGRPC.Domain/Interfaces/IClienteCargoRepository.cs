using ExemploGRPC.Domain.Entities.Implementation;

namespace ExemploGRPC.Domain.Interfaces;

/// <summary>
/// Repository interface for ClienteCargo entity
/// Follows Repository pattern from DDD
/// </summary>
public interface IClienteCargoRepository
{
    /// <summary>
    /// Gets a cliente-cargo association by ID
    /// </summary>
    Task<ClienteCargo?> GetByIdAsync(int id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets all cliente-cargo associations for a specific cliente
    /// </summary>
    Task<IEnumerable<ClienteCargo>> GetByClienteIdAsync(int clienteId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets all cliente-cargo associations for a specific cargo
    /// </summary>
    Task<IEnumerable<ClienteCargo>> GetByCargoIdAsync(int cargoId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets a specific association between a cliente and cargo
    /// </summary>
    Task<ClienteCargo?> GetByClienteAndCargoAsync(int clienteId, int cargoId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets all cliente-cargo associations
    /// </summary>
    Task<IEnumerable<ClienteCargo>> GetAllAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Adds a new cliente-cargo association
    /// </summary>
    Task AddAsync(ClienteCargo clienteCargo, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates an existing cliente-cargo association
    /// </summary>
    Task UpdateAsync(ClienteCargo clienteCargo, CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes a cliente-cargo association
    /// </summary>
    Task DeleteAsync(ClienteCargo clienteCargo, CancellationToken cancellationToken = default);

    /// <summary>
    /// Checks if an association between cliente and cargo exists
    /// </summary>
    Task<bool> ExistsAsync(int clienteId, int cargoId, CancellationToken cancellationToken = default);
}
