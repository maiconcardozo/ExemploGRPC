using ExemploGRPC.Application.DTOs;
using ExemploGRPC.Application.Services.Interfaces;
using Grpc.Core;

namespace ExemploGRPC.GRPC.Services;

/// <summary>
/// GRPC Service implementation for ClienteCargo
/// </summary>
public class GrpcClienteCargoService : ClienteCargoService.ClienteCargoServiceBase
{
    private readonly IClienteCargoService _clienteCargoService;
    private readonly ILogger<GrpcClienteCargoService> _logger;

    public GrpcClienteCargoService(IClienteCargoService clienteCargoService, ILogger<GrpcClienteCargoService> logger)
    {
        _clienteCargoService = clienteCargoService ?? throw new ArgumentNullException(nameof(clienteCargoService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public override async Task<ClienteCargoResponse> GetClienteCargo(GetClienteCargoRequest request, ServerCallContext context)
    {
        try
        {
            var id = Guid.Parse(request.Id);
            var clienteCargo = await _clienteCargoService.GetByIdAsync(id, context.CancellationToken);

            if (clienteCargo == null)
                throw new RpcException(new Status(StatusCode.NotFound, $"ClienteCargo with ID {request.Id} not found"));

            return MapToClienteCargoResponse(clienteCargo);
        }
        catch (FormatException)
        {
            throw new RpcException(new Status(StatusCode.InvalidArgument, "Invalid ID format"));
        }
        catch (RpcException) { throw; }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting clienteCargo by ID: {Id}", request.Id);
            throw new RpcException(new Status(StatusCode.Internal, "Internal server error"));
        }
    }

    public override async Task<GetClienteCargosResponse> GetClienteCargosByClienteId(GetClienteCargosByClienteIdRequest request, ServerCallContext context)
    {
        try
        {
            var clienteId = Guid.Parse(request.ClienteId);
            var clienteCargos = await _clienteCargoService.GetByClienteIdAsync(clienteId, context.CancellationToken);

            var response = new GetClienteCargosResponse();
            response.ClienteCargos.AddRange(clienteCargos.Select(MapToClienteCargoResponse));
            return response;
        }
        catch (FormatException)
        {
            throw new RpcException(new Status(StatusCode.InvalidArgument, "Invalid ID format"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting clienteCargos by cliente ID: {ClienteId}", request.ClienteId);
            throw new RpcException(new Status(StatusCode.Internal, "Internal server error"));
        }
    }

    public override async Task<GetClienteCargosResponse> GetClienteCargosByCargoId(GetClienteCargosByCargoIdRequest request, ServerCallContext context)
    {
        try
        {
            var cargoId = Guid.Parse(request.CargoId);
            var clienteCargos = await _clienteCargoService.GetByCargoIdAsync(cargoId, context.CancellationToken);

            var response = new GetClienteCargosResponse();
            response.ClienteCargos.AddRange(clienteCargos.Select(MapToClienteCargoResponse));
            return response;
        }
        catch (FormatException)
        {
            throw new RpcException(new Status(StatusCode.InvalidArgument, "Invalid ID format"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting clienteCargos by cargo ID: {CargoId}", request.CargoId);
            throw new RpcException(new Status(StatusCode.Internal, "Internal server error"));
        }
    }

    public override async Task<GetClienteCargosResponse> GetAllClienteCargos(GetAllClienteCargosRequest request, ServerCallContext context)
    {
        try
        {
            var clienteCargos = await _clienteCargoService.GetAllAsync(context.CancellationToken);

            var response = new GetClienteCargosResponse();
            response.ClienteCargos.AddRange(clienteCargos.Select(MapToClienteCargoResponse));
            return response;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting all clienteCargos");
            throw new RpcException(new Status(StatusCode.Internal, "Internal server error"));
        }
    }

    public override async Task<ClienteCargoResponse> CreateClienteCargo(CreateClienteCargoRequest request, ServerCallContext context)
    {
        try
        {
            var createDto = new CreateClienteCargoDto
            {
                ClienteId = Guid.Parse(request.ClienteId),
                CargoId = Guid.Parse(request.CargoId),
                IsPrincipal = request.IsPrincipal
            };

            var clienteCargo = await _clienteCargoService.CreateAsync(createDto, context.CancellationToken);
            return MapToClienteCargoResponse(clienteCargo);
        }
        catch (KeyNotFoundException ex)
        {
            throw new RpcException(new Status(StatusCode.NotFound, ex.Message));
        }
        catch (InvalidOperationException ex)
        {
            throw new RpcException(new Status(StatusCode.AlreadyExists, ex.Message));
        }
        catch (FormatException)
        {
            throw new RpcException(new Status(StatusCode.InvalidArgument, "Invalid ID format"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating clienteCargo");
            throw new RpcException(new Status(StatusCode.Internal, "Internal server error"));
        }
    }

    public override async Task<ClienteCargoResponse> UpdateClienteCargo(UpdateClienteCargoRequest request, ServerCallContext context)
    {
        try
        {
            var id = Guid.Parse(request.Id);
            var updateDto = new UpdateClienteCargoDto
            {
                DataFim = string.IsNullOrWhiteSpace(request.DataFim) ? null : DateTime.Parse(request.DataFim),
                IsPrincipal = request.IsPrincipal
            };

            var clienteCargo = await _clienteCargoService.UpdateAsync(id, updateDto, context.CancellationToken);
            return MapToClienteCargoResponse(clienteCargo);
        }
        catch (KeyNotFoundException ex)
        {
            throw new RpcException(new Status(StatusCode.NotFound, ex.Message));
        }
        catch (FormatException)
        {
            throw new RpcException(new Status(StatusCode.InvalidArgument, "Invalid ID or date format"));
        }
        catch (ArgumentException ex)
        {
            throw new RpcException(new Status(StatusCode.InvalidArgument, ex.Message));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating clienteCargo");
            throw new RpcException(new Status(StatusCode.Internal, "Internal server error"));
        }
    }

    public override async Task<DeleteClienteCargoResponse> DeleteClienteCargo(DeleteClienteCargoRequest request, ServerCallContext context)
    {
        try
        {
            var id = Guid.Parse(request.Id);
            await _clienteCargoService.DeleteAsync(id, context.CancellationToken);

            return new DeleteClienteCargoResponse
            {
                Success = true,
                Message = "ClienteCargo deleted successfully"
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
            _logger.LogError(ex, "Error deleting clienteCargo");
            throw new RpcException(new Status(StatusCode.Internal, "Internal server error"));
        }
    }

    private static ClienteCargoResponse MapToClienteCargoResponse(ClienteCargoDto clienteCargo)
    {
        return new ClienteCargoResponse
        {
            Id = clienteCargo.Id.ToString(),
            ClienteId = clienteCargo.ClienteId.ToString(),
            CargoId = clienteCargo.CargoId.ToString(),
            DataAtribuicao = clienteCargo.DataAtribuicao.ToString("O"),
            DataFim = clienteCargo.DataFim?.ToString("O") ?? string.Empty,
            IsPrincipal = clienteCargo.IsPrincipal
        };
    }
}
