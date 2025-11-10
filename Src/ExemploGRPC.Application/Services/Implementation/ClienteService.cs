using AutoMapper;
using ExemploGRPC.Application.DTOs;
using ExemploGRPC.Application.Services.Interfaces;
using ExemploGRPC.Domain.Entities.Implementation;
using ExemploGRPC.Domain.Interfaces;

namespace ExemploGRPC.Application.Services.Implementation;

/// <summary>
/// Service implementation for Cliente operations
/// Implements business logic following SOLID principles
/// </summary>
public class ClienteService : IClienteService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public ClienteService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public async Task<ClienteDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var cliente = await _unitOfWork.Clientes.GetByIdAsync(id, cancellationToken);
        return cliente == null ? null : _mapper.Map<ClienteDto>(cliente);
    }

    public async Task<ClienteDto?> GetByCpfAsync(string cpf, CancellationToken cancellationToken = default)
    {
        var cliente = await _unitOfWork.Clientes.GetByCpfAsync(cpf, cancellationToken);
        return cliente == null ? null : _mapper.Map<ClienteDto>(cliente);
    }

    public async Task<IEnumerable<ClienteDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var clientes = await _unitOfWork.Clientes.GetAllAsync(cancellationToken);
        return _mapper.Map<IEnumerable<ClienteDto>>(clientes);
    }

    public async Task<IEnumerable<ClienteDto>> GetPagedAsync(int pageNumber, int pageSize, CancellationToken cancellationToken = default)
    {
        var clientes = await _unitOfWork.Clientes.GetPagedAsync(pageNumber, pageSize, cancellationToken);
        return _mapper.Map<IEnumerable<ClienteDto>>(clientes);
    }

    public async Task<ClienteDto> CreateAsync(CreateClienteDto dto, CancellationToken cancellationToken = default)
    {
        // Check if CPF already exists
        if (await _unitOfWork.Clientes.ExistsByCpfAsync(dto.Cpf, cancellationToken))
        {
            throw new InvalidOperationException($"Cliente com CPF {dto.Cpf} já existe");
        }

        var cliente = _mapper.Map<Cliente>(dto);
        await _unitOfWork.Clientes.AddAsync(cliente, cancellationToken);
        await _unitOfWork.CommitAsync(cancellationToken);

        return _mapper.Map<ClienteDto>(cliente);
    }

    public async Task<ClienteDto> UpdateAsync(int id, UpdateClienteDto dto, CancellationToken cancellationToken = default)
    {
        var cliente = await _unitOfWork.Clientes.GetByIdAsync(id, cancellationToken)
            ?? throw new KeyNotFoundException($"Cliente com ID {id} não encontrado");

        cliente.Update(dto.Nome, dto.Email, dto.Telefone);
        await _unitOfWork.Clientes.UpdateAsync(cliente, cancellationToken);
        await _unitOfWork.CommitAsync(cancellationToken);

        return _mapper.Map<ClienteDto>(cliente);
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var cliente = await _unitOfWork.Clientes.GetByIdAsync(id, cancellationToken)
            ?? throw new KeyNotFoundException($"Cliente com ID {id} não encontrado");

        await _unitOfWork.Clientes.DeleteAsync(cliente, cancellationToken);
        await _unitOfWork.CommitAsync(cancellationToken);
    }

    public async Task AddCargoAsync(int clienteId, int cargoId, CancellationToken cancellationToken = default)
    {
        var cliente = await _unitOfWork.Clientes.GetByIdAsync(clienteId, cancellationToken)
            ?? throw new KeyNotFoundException($"Cliente com ID {clienteId} não encontrado");

        var cargo = await _unitOfWork.Cargos.GetByIdAsync(cargoId, cancellationToken)
            ?? throw new KeyNotFoundException($"Cargo com ID {cargoId} não encontrado");

        cliente.AddCargo(cargo);
        await _unitOfWork.Clientes.UpdateAsync(cliente, cancellationToken);
        await _unitOfWork.CommitAsync(cancellationToken);
    }

    public async Task RemoveCargoAsync(int clienteId, int cargoId, CancellationToken cancellationToken = default)
    {
        var cliente = await _unitOfWork.Clientes.GetByIdAsync(clienteId, cancellationToken)
            ?? throw new KeyNotFoundException($"Cliente com ID {clienteId} não encontrado");

        cliente.RemoveCargo(cargoId);
        await _unitOfWork.Clientes.UpdateAsync(cliente, cancellationToken);
        await _unitOfWork.CommitAsync(cancellationToken);
    }
}
