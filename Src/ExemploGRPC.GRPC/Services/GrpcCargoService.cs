using ExemploGRPC.Application.DTOs;
using ExemploGRPC.Application.Services.Interfaces;
using Grpc.Core;

namespace ExemploGRPC.GRPC.Services;

/// <summary>
/// GRPC Service implementation for Cargo
/// </summary>
public class GrpcCargoService : CargoService.CargoServiceBase
{
    private readonly ICargoService _cargoService;
    private readonly ILogger<GrpcCargoService> _logger;

    public GrpcCargoService(ICargoService cargoService, ILogger<GrpcCargoService> logger)
    {
        _cargoService = cargoService ?? throw new ArgumentNullException(nameof(cargoService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public override async Task<CargoResponse> GetCargo(GetCargoRequest request, ServerCallContext context)
    {
        try
        {
            var id = Guid.Parse(request.Id);
            var cargo = await _cargoService.GetByIdAsync(id, context.CancellationToken);

            if (cargo == null)
                throw new RpcException(new Status(StatusCode.NotFound, $"Cargo with ID {request.Id} not found"));

            return MapToCargoResponse(cargo);
        }
        catch (FormatException)
        {
            throw new RpcException(new Status(StatusCode.InvalidArgument, "Invalid ID format"));
        }
        catch (RpcException) { throw; }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting cargo by ID: {Id}", request.Id);
            throw new RpcException(new Status(StatusCode.Internal, "Internal server error"));
        }
    }

    public override async Task<CargoResponse> GetCargoByNome(GetCargoByNomeRequest request, ServerCallContext context)
    {
        try
        {
            var cargo = await _cargoService.GetByNomeAsync(request.Nome, context.CancellationToken);

            if (cargo == null)
                throw new RpcException(new Status(StatusCode.NotFound, $"Cargo with name {request.Nome} not found"));

            return MapToCargoResponse(cargo);
        }
        catch (RpcException) { throw; }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting cargo by name: {Nome}", request.Nome);
            throw new RpcException(new Status(StatusCode.Internal, "Internal server error"));
        }
    }

    public override async Task<GetAllCargosResponse> GetAllCargos(GetAllCargosRequest request, ServerCallContext context)
    {
        try
        {
            IEnumerable<CargoDto> cargos;

            if (request.PageNumber > 0 && request.PageSize > 0)
                cargos = await _cargoService.GetPagedAsync(request.PageNumber, request.PageSize, context.CancellationToken);
            else
                cargos = await _cargoService.GetAllAsync(context.CancellationToken);

            var response = new GetAllCargosResponse();
            response.Cargos.AddRange(cargos.Select(MapToCargoResponse));
            return response;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting all cargos");
            throw new RpcException(new Status(StatusCode.Internal, "Internal server error"));
        }
    }

    public override async Task<CargoResponse> CreateCargo(CreateCargoRequest request, ServerCallContext context)
    {
        try
        {
            var createDto = new CreateCargoDto
            {
                Nome = request.Nome,
                Descricao = string.IsNullOrWhiteSpace(request.Descricao) ? null : request.Descricao,
                NivelSalarial = request.NivelSalarial > 0 ? (decimal)request.NivelSalarial : null
            };

            var cargo = await _cargoService.CreateAsync(createDto, context.CancellationToken);
            return MapToCargoResponse(cargo);
        }
        catch (InvalidOperationException ex)
        {
            throw new RpcException(new Status(StatusCode.AlreadyExists, ex.Message));
        }
        catch (ArgumentException ex)
        {
            throw new RpcException(new Status(StatusCode.InvalidArgument, ex.Message));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating cargo");
            throw new RpcException(new Status(StatusCode.Internal, "Internal server error"));
        }
    }

    public override async Task<CargoResponse> UpdateCargo(UpdateCargoRequest request, ServerCallContext context)
    {
        try
        {
            var id = Guid.Parse(request.Id);
            var updateDto = new UpdateCargoDto
            {
                Nome = request.Nome,
                Descricao = string.IsNullOrWhiteSpace(request.Descricao) ? null : request.Descricao,
                NivelSalarial = request.NivelSalarial > 0 ? (decimal)request.NivelSalarial : null
            };

            var cargo = await _cargoService.UpdateAsync(id, updateDto, context.CancellationToken);
            return MapToCargoResponse(cargo);
        }
        catch (KeyNotFoundException ex)
        {
            throw new RpcException(new Status(StatusCode.NotFound, ex.Message));
        }
        catch (FormatException)
        {
            throw new RpcException(new Status(StatusCode.InvalidArgument, "Invalid ID format"));
        }
        catch (ArgumentException ex)
        {
            throw new RpcException(new Status(StatusCode.InvalidArgument, ex.Message));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating cargo");
            throw new RpcException(new Status(StatusCode.Internal, "Internal server error"));
        }
    }

    public override async Task<DeleteCargoResponse> DeleteCargo(DeleteCargoRequest request, ServerCallContext context)
    {
        try
        {
            var id = Guid.Parse(request.Id);
            await _cargoService.DeleteAsync(id, context.CancellationToken);

            return new DeleteCargoResponse
            {
                Success = true,
                Message = "Cargo deleted successfully"
            };
        }
        catch (KeyNotFoundException ex)
        {
            throw new RpcException(new Status(StatusCode.NotFound, ex.Message));
        }
        catch (FormatException)
        {
            throw new RpcException(new Status(StatusCode.InvalidArgument, "Invalid ID format"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting cargo");
            throw new RpcException(new Status(StatusCode.Internal, "Internal server error"));
        }
    }

    private static CargoResponse MapToCargoResponse(CargoDto cargo)
    {
        return new CargoResponse
        {
            Id = cargo.Id.ToString(),
            Nome = cargo.Nome,
            Descricao = cargo.Descricao ?? string.Empty,
            NivelSalarial = (double)(cargo.NivelSalarial ?? 0),
            CreatedAt = cargo.CreatedAt.ToString("O"),
            UpdatedAt = cargo.UpdatedAt?.ToString("O") ?? string.Empty
        };
    }
}
