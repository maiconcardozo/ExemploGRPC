using ExemploGRPC.Domain.Entities.Implementation;
using ExemploGRPC.Domain.Interfaces;
using ExemploGRPC.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ExemploGRPC.Infrastructure.Data.Repositories;

/// <summary>
/// Repository implementation for ClienteCargo entity
/// </summary>
public class ClienteCargoRepository : IClienteCargoRepository
{
    private readonly ApplicationDbContext _context;

    public ClienteCargoRepository(ApplicationDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task<ClienteCargo?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _context.ClienteCargos
            .Include(cc => cc.Cliente)
            .Include(cc => cc.Cargo)
            .FirstOrDefaultAsync(cc => cc.Id == id, cancellationToken);
    }

    public async Task<IEnumerable<ClienteCargo>> GetByClienteIdAsync(int clienteId, CancellationToken cancellationToken = default)
    {
        return await _context.ClienteCargos
            .Include(cc => cc.Cliente)
            .Include(cc => cc.Cargo)
            .Where(cc => cc.ClienteId == clienteId)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<ClienteCargo>> GetByCargoIdAsync(int cargoId, CancellationToken cancellationToken = default)
    {
        return await _context.ClienteCargos
            .Include(cc => cc.Cliente)
            .Include(cc => cc.Cargo)
            .Where(cc => cc.CargoId == cargoId)
            .ToListAsync(cancellationToken);
    }

    public async Task<ClienteCargo?> GetByClienteAndCargoAsync(int clienteId, int cargoId, CancellationToken cancellationToken = default)
    {
        return await _context.ClienteCargos
            .Include(cc => cc.Cliente)
            .Include(cc => cc.Cargo)
            .FirstOrDefaultAsync(cc => cc.ClienteId == clienteId && cc.CargoId == cargoId, cancellationToken);
    }

    public async Task<IEnumerable<ClienteCargo>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.ClienteCargos
            .Include(cc => cc.Cliente)
            .Include(cc => cc.Cargo)
            .ToListAsync(cancellationToken);
    }

    public async Task AddAsync(ClienteCargo clienteCargo, CancellationToken cancellationToken = default)
    {
        await _context.ClienteCargos.AddAsync(clienteCargo, cancellationToken);
    }

    public Task UpdateAsync(ClienteCargo clienteCargo, CancellationToken cancellationToken = default)
    {
        _context.ClienteCargos.Update(clienteCargo);
        return Task.CompletedTask;
    }

    public Task DeleteAsync(ClienteCargo clienteCargo, CancellationToken cancellationToken = default)
    {
        clienteCargo.SoftDelete("System");
        _context.ClienteCargos.Update(clienteCargo);
        return Task.CompletedTask;
    }

    public async Task<bool> ExistsAsync(int clienteId, int cargoId, CancellationToken cancellationToken = default)
    {
        return await _context.ClienteCargos.AnyAsync(cc => cc.ClienteId == clienteId && cc.CargoId == cargoId, cancellationToken);
    }
}
