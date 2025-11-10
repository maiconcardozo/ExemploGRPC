namespace ExemploGRPC.Domain.Interfaces;

/// <summary>
/// Unit of Work interface
/// Implements Unit of Work pattern for transaction management
/// </summary>
public interface IUnitOfWork : IDisposable
{
    /// <summary>
    /// Cliente repository
    /// </summary>
    IClienteRepository Clientes { get; }

    /// <summary>
    /// Cargo repository
    /// </summary>
    ICargoRepository Cargos { get; }

    /// <summary>
    /// ClienteCargo repository
    /// </summary>
    IClienteCargoRepository ClienteCargos { get; }

    /// <summary>
    /// Commits all changes to the database
    /// </summary>
    Task<int> CommitAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Begins a new database transaction
    /// </summary>
    Task BeginTransactionAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Commits the current transaction
    /// </summary>
    Task CommitTransactionAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Rolls back the current transaction
    /// </summary>
    Task RollbackTransactionAsync(CancellationToken cancellationToken = default);
}
