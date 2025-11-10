using AutoMapper;
using ExemploGRPC.Domain.DTOs;
using ExemploGRPC.Application.Services.Interfaces;
using ExemploGRPC.Domain.Entities.Implementation;
using ExemploGRPC.Domain.Interfaces;

namespace ExemploGRPC.Application.Services.Implementation;

/// <summary>
/// Service implementation for Cargo operations
/// Implements business logic following SOLID principles
/// </summary>
public class CargoService : ICargoService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CargoService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public async Task<CargoDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var cargo = await _unitOfWork.Cargos.GetByIdAsync(id, cancellationToken);
        return cargo == null ? null : _mapper.Map<CargoDto>(cargo);
    }

    public async Task<CargoDto?> GetByNomeAsync(string nome, CancellationToken cancellationToken = default)
    {
        var cargo = await _unitOfWork.Cargos.GetByNomeAsync(nome, cancellationToken);
        return cargo == null ? null : _mapper.Map<CargoDto>(cargo);
    }

    public async Task<IEnumerable<CargoDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var cargos = await _unitOfWork.Cargos.GetAllAsync(cancellationToken);
        return _mapper.Map<IEnumerable<CargoDto>>(cargos);
    }

    public async Task<IEnumerable<CargoDto>> GetPagedAsync(int pageNumber, int pageSize, CancellationToken cancellationToken = default)
    {
        var cargos = await _unitOfWork.Cargos.GetPagedAsync(pageNumber, pageSize, cancellationToken);
        return _mapper.Map<IEnumerable<CargoDto>>(cargos);
    }

    public async Task<CargoDto> CreateAsync(CreateCargoDto dto, CancellationToken cancellationToken = default)
    {
        // Check if cargo with same name already exists
        if (await _unitOfWork.Cargos.ExistsByNomeAsync(dto.Nome, cancellationToken))
        {
            throw new InvalidOperationException($"Cargo com nome '{dto.Nome}' já existe");
        }

        var cargo = _mapper.Map<Cargo>(dto);
        await _unitOfWork.Cargos.AddAsync(cargo, cancellationToken);
        await _unitOfWork.CommitAsync(cancellationToken);

        return _mapper.Map<CargoDto>(cargo);
    }

    public async Task<CargoDto> UpdateAsync(int id, UpdateCargoDto dto, CancellationToken cancellationToken = default)
    {
        var cargo = await _unitOfWork.Cargos.GetByIdAsync(id, cancellationToken)
            ?? throw new KeyNotFoundException($"Cargo com ID {id} não encontrado");

        cargo.Update(dto.Nome, dto.Descricao, dto.NivelSalarial);
        await _unitOfWork.Cargos.UpdateAsync(cargo, cancellationToken);
        await _unitOfWork.CommitAsync(cancellationToken);

        return _mapper.Map<CargoDto>(cargo);
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var cargo = await _unitOfWork.Cargos.GetByIdAsync(id, cancellationToken)
            ?? throw new KeyNotFoundException($"Cargo com ID {id} não encontrado");

        await _unitOfWork.Cargos.DeleteAsync(cargo, cancellationToken);
        await _unitOfWork.CommitAsync(cancellationToken);
    }
}
