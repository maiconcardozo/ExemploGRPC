using ExemploGRPC.Domain.Entities.Implementation;
using ExemploGRPC.Domain.Interfaces;
using ExemploGRPC.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ExemploGRPC.Infrastructure.Data.Repositories;

/// <summary>
/// Repository implementation for Cliente entity
/// </summary>
public class ClienteRepository : IClienteRepository
{
    private readonly ApplicationDbContext _context;

    public ClienteRepository(ApplicationDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task<Cliente?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _context.Clientes
            .Include(c => c.ClienteCargos)
            .ThenInclude(cc => cc.Cargo)
            .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
    }

    public async Task<Cliente?> GetByCpfAsync(string cpf, CancellationToken cancellationToken = default)
    {
        return await _context.Clientes
            .Include(c => c.ClienteCargos)
            .ThenInclude(cc => cc.Cargo)
            .FirstOrDefaultAsync(c => c.Cpf == cpf, cancellationToken);
    }

    public async Task<IEnumerable<Cliente>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Clientes
            .Include(c => c.ClienteCargos)
            .ThenInclude(cc => cc.Cargo)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Cliente>> GetPagedAsync(int pageNumber, int pageSize, CancellationToken cancellationToken = default)
    {
        return await _context.Clientes
            .Include(c => c.ClienteCargos)
            .ThenInclude(cc => cc.Cargo)
            .OrderBy(c => c.Nome)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Cliente>> GetByCargoIdAsync(int cargoId, CancellationToken cancellationToken = default)
    {
        return await _context.Clientes
            .Include(c => c.ClienteCargos)
            .ThenInclude(cc => cc.Cargo)
            .Where(c => c.ClienteCargos.Any(cc => cc.CargoId == cargoId))
            .ToListAsync(cancellationToken);
    }

    public async Task AddAsync(Cliente cliente, CancellationToken cancellationToken = default)
    {
        await _context.Clientes.AddAsync(cliente, cancellationToken);
    }

    public Task UpdateAsync(Cliente cliente, CancellationToken cancellationToken = default)
    {
        _context.Clientes.Update(cliente);
        return Task.CompletedTask;
    }

    public Task DeleteAsync(Cliente cliente, CancellationToken cancellationToken = default)
    {
        cliente.SoftDelete("System");
        _context.Clientes.Update(cliente);
        return Task.CompletedTask;
    }

    public async Task<bool> ExistsByCpfAsync(string cpf, CancellationToken cancellationToken = default)
    {
        return await _context.Clientes.AnyAsync(c => c.Cpf == cpf, cancellationToken);
    }
}
