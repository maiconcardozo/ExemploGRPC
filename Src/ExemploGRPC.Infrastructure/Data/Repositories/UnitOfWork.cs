using ExemploGRPC.Domain.Interfaces;
using ExemploGRPC.Infrastructure.Data;
using Microsoft.EntityFrameworkCore.Storage;

namespace ExemploGRPC.Infrastructure.Data.Repositories;

/// <summary>
/// Unit of Work implementation
/// Implements transaction management and repository access
/// </summary>
public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _context;
    private IDbContextTransaction? _transaction;
    private IClienteRepository? _clientes;
    private ICargoRepository? _cargos;
    private IClienteCargoRepository? _clienteCargos;

    public UnitOfWork(ApplicationDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public IClienteRepository Clientes
    {
        get
        {
            _clientes ??= new ClienteRepository(_context);
            return _clientes;
        }
    }

    public ICargoRepository Cargos
    {
        get
        {
            _cargos ??= new CargoRepository(_context);
            return _cargos;
        }
    }

    public IClienteCargoRepository ClienteCargos
    {
        get
        {
            _clienteCargos ??= new ClienteCargoRepository(_context);
            return _clienteCargos;
        }
    }

    public async Task<int> CommitAsync(CancellationToken cancellationToken = default)
    {
        return await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task BeginTransactionAsync(CancellationToken cancellationToken = default)
    {
        _transaction = await _context.Database.BeginTransactionAsync(cancellationToken);
    }

    public async Task CommitTransactionAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            await _context.SaveChangesAsync(cancellationToken);
            if (_transaction != null)
            {
                await _transaction.CommitAsync(cancellationToken);
            }
        }
        catch
        {
            await RollbackTransactionAsync(cancellationToken);
            throw;
        }
        finally
        {
            if (_transaction != null)
            {
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }
    }

    public async Task RollbackTransactionAsync(CancellationToken cancellationToken = default)
    {
        if (_transaction != null)
        {
            await _transaction.RollbackAsync(cancellationToken);
            await _transaction.DisposeAsync();
            _transaction = null;
        }
    }

    public void Dispose()
    {
        _transaction?.Dispose();
        _context.Dispose();
    }
}
