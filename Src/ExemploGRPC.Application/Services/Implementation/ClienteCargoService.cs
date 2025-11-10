using AutoMapper;
using ExemploGRPC.Application.DTOs;
using ExemploGRPC.Application.Services.Interfaces;
using ExemploGRPC.Domain.Entities.Implementation;
using ExemploGRPC.Domain.Interfaces;

namespace ExemploGRPC.Application.Services.Implementation;

/// <summary>
/// Service implementation for ClienteCargo operations
/// Implements business logic following SOLID principles
/// </summary>
public class ClienteCargoService : IClienteCargoService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public ClienteCargoService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public async Task<ClienteCargoDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var clienteCargo = await _unitOfWork.ClienteCargos.GetByIdAsync(id, cancellationToken);
        return clienteCargo == null ? null : _mapper.Map<ClienteCargoDto>(clienteCargo);
    }

    public async Task<IEnumerable<ClienteCargoDto>> GetByClienteIdAsync(int clienteId, CancellationToken cancellationToken = default)
    {
        var clienteCargos = await _unitOfWork.ClienteCargos.GetByClienteIdAsync(clienteId, cancellationToken);
        return _mapper.Map<IEnumerable<ClienteCargoDto>>(clienteCargos);
    }

    public async Task<IEnumerable<ClienteCargoDto>> GetByCargoIdAsync(int cargoId, CancellationToken cancellationToken = default)
    {
        var clienteCargos = await _unitOfWork.ClienteCargos.GetByCargoIdAsync(cargoId, cancellationToken);
        return _mapper.Map<IEnumerable<ClienteCargoDto>>(clienteCargos);
    }

    public async Task<IEnumerable<ClienteCargoDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var clienteCargos = await _unitOfWork.ClienteCargos.GetAllAsync(cancellationToken);
        return _mapper.Map<IEnumerable<ClienteCargoDto>>(clienteCargos);
    }

    public async Task<ClienteCargoDto> CreateAsync(CreateClienteCargoDto dto, CancellationToken cancellationToken = default)
    {
        // Validate that cliente exists
        var cliente = await _unitOfWork.Clientes.GetByIdAsync(dto.ClienteId, cancellationToken)
            ?? throw new KeyNotFoundException($"Cliente com ID {dto.ClienteId} não encontrado");

        // Validate that cargo exists
        var cargo = await _unitOfWork.Cargos.GetByIdAsync(dto.CargoId, cancellationToken)
            ?? throw new KeyNotFoundException($"Cargo com ID {dto.CargoId} não encontrado");

        // Check if association already exists
        if (await _unitOfWork.ClienteCargos.ExistsAsync(dto.ClienteId, dto.CargoId, cancellationToken))
        {
            throw new InvalidOperationException($"Associação entre Cliente e Cargo já existe");
        }

        var clienteCargo = _mapper.Map<ClienteCargo>(dto);
        await _unitOfWork.ClienteCargos.AddAsync(clienteCargo, cancellationToken);
        await _unitOfWork.CommitAsync(cancellationToken);

        return _mapper.Map<ClienteCargoDto>(clienteCargo);
    }

    public async Task<ClienteCargoDto> UpdateAsync(int id, UpdateClienteCargoDto dto, CancellationToken cancellationToken = default)
    {
        var clienteCargo = await _unitOfWork.ClienteCargos.GetByIdAsync(id, cancellationToken)
            ?? throw new KeyNotFoundException($"ClienteCargo com ID {id} não encontrado");

        if (dto.DataFim.HasValue)
        {
            clienteCargo.SetDataFim(dto.DataFim.Value);
        }

        if (dto.IsPrincipal)
        {
            clienteCargo.SetAsPrincipal();
        }
        else
        {
            clienteCargo.RemovePrincipal();
        }

        await _unitOfWork.ClienteCargos.UpdateAsync(clienteCargo, cancellationToken);
        await _unitOfWork.CommitAsync(cancellationToken);

        return _mapper.Map<ClienteCargoDto>(clienteCargo);
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var clienteCargo = await _unitOfWork.ClienteCargos.GetByIdAsync(id, cancellationToken)
            ?? throw new KeyNotFoundException($"ClienteCargo com ID {id} não encontrado");

        await _unitOfWork.ClienteCargos.DeleteAsync(clienteCargo, cancellationToken);
        await _unitOfWork.CommitAsync(cancellationToken);
    }
}
