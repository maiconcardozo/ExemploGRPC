using ExemploGRPC.Domain.Entities.Implementation;
using ExemploGRPC.Domain.Interfaces;
using ExemploGRPC.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ExemploGRPC.Infrastructure.Data.Repositories;

/// <summary>
/// Repository implementation for Cargo entity
/// </summary>
public class CargoRepository : ICargoRepository
{
    private readonly ApplicationDbContext _context;

    public CargoRepository(ApplicationDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task<Cargo?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Cargos
            .Include(c => c.ClienteCargos)
            .ThenInclude(cc => cc.Cliente)
            .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
    }

    public async Task<Cargo?> GetByNomeAsync(string nome, CancellationToken cancellationToken = default)
    {
        return await _context.Cargos
            .Include(c => c.ClienteCargos)
            .ThenInclude(cc => cc.Cliente)
            .FirstOrDefaultAsync(c => c.Nome == nome, cancellationToken);
    }

    public async Task<IEnumerable<Cargo>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Cargos
            .Include(c => c.ClienteCargos)
            .ThenInclude(cc => cc.Cliente)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Cargo>> GetPagedAsync(int pageNumber, int pageSize, CancellationToken cancellationToken = default)
    {
        return await _context.Cargos
            .Include(c => c.ClienteCargos)
            .ThenInclude(cc => cc.Cliente)
            .OrderBy(c => c.Nome)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Cargo>> GetByClienteIdAsync(Guid clienteId, CancellationToken cancellationToken = default)
    {
        return await _context.Cargos
            .Include(c => c.ClienteCargos)
            .ThenInclude(cc => cc.Cliente)
            .Where(c => c.ClienteCargos.Any(cc => cc.ClienteId == clienteId))
            .ToListAsync(cancellationToken);
    }

    public async Task AddAsync(Cargo cargo, CancellationToken cancellationToken = default)
    {
        await _context.Cargos.AddAsync(cargo, cancellationToken);
    }

    public Task UpdateAsync(Cargo cargo, CancellationToken cancellationToken = default)
    {
        _context.Cargos.Update(cargo);
        return Task.CompletedTask;
    }

    public Task DeleteAsync(Cargo cargo, CancellationToken cancellationToken = default)
    {
        cargo.MarkAsDeleted();
        _context.Cargos.Update(cargo);
        return Task.CompletedTask;
    }

    public async Task<bool> ExistsByNomeAsync(string nome, CancellationToken cancellationToken = default)
    {
        return await _context.Cargos.AnyAsync(c => c.Nome == nome, cancellationToken);
    }
}
