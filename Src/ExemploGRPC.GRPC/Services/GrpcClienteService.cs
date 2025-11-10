using ExemploGRPC.Domain.DTOs;
using ExemploGRPC.Application.Services.Interfaces;
using Grpc.Core;

namespace ExemploGRPC.GRPC.Services;

/// <summary>
/// GRPC Service implementation for Cliente
/// </summary>
public class GrpcClienteService : ClienteService.ClienteServiceBase
{
    private readonly IClienteService _clienteService;
    private readonly ILogger<GrpcClienteService> _logger;

    public GrpcClienteService(IClienteService clienteService, ILogger<GrpcClienteService> logger)
    {
        _clienteService = clienteService ?? throw new ArgumentNullException(nameof(clienteService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public override async Task<ClienteResponse> GetCliente(GetClienteRequest request, ServerCallContext context)
    {
        try
        {
            var id = request.Id;
            var cliente = await _clienteService.GetByIdAsync(id, context.CancellationToken);

            if (cliente == null)
            {
                throw new RpcException(new Status(StatusCode.NotFound, $"Cliente with ID {request.Id} not found"));
            }

            return MapToClienteResponse(cliente);
        }
        catch (FormatException)
        {
            throw new RpcException(new Status(StatusCode.InvalidArgument, "Invalid ID format"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting cliente by ID: {Id}", request.Id);
            throw new RpcException(new Status(StatusCode.Internal, "Internal server error"));
        }
    }

    public override async Task<ClienteResponse> GetClienteByCpf(GetClienteByCpfRequest request, ServerCallContext context)
    {
        try
        {
            var cliente = await _clienteService.GetByCpfAsync(request.Cpf, context.CancellationToken);

            if (cliente == null)
            {
                throw new RpcException(new Status(StatusCode.NotFound, $"Cliente with CPF {request.Cpf} not found"));
            }

            return MapToClienteResponse(cliente);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting cliente by CPF: {Cpf}", request.Cpf);
            throw new RpcException(new Status(StatusCode.Internal, "Internal server error"));
        }
    }

    public override async Task<GetAllClientesResponse> GetAllClientes(GetAllClientesRequest request, ServerCallContext context)
    {
        try
        {
            IEnumerable<ClienteDto> clientes;

            if (request.PageNumber > 0 && request.PageSize > 0)
            {
                clientes = await _clienteService.GetPagedAsync(request.PageNumber, request.PageSize, context.CancellationToken);
            }
            else
            {
                clientes = await _clienteService.GetAllAsync(context.CancellationToken);
            }

            var response = new GetAllClientesResponse();
            response.Clientes.AddRange(clientes.Select(MapToClienteResponse));

            return response;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting all clientes");
            throw new RpcException(new Status(StatusCode.Internal, "Internal server error"));
        }
    }

    public override async Task<ClienteResponse> CreateCliente(CreateClienteRequest request, ServerCallContext context)
    {
        try
        {
            var createDto = new CreateClienteDto
            {
                Nome = request.Nome,
                Email = request.Email,
                Cpf = request.Cpf,
                Telefone = string.IsNullOrWhiteSpace(request.Telefone) ? null : request.Telefone
            };

            var cliente = await _clienteService.CreateAsync(createDto, context.CancellationToken);
            return MapToClienteResponse(cliente);
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
            _logger.LogError(ex, "Error creating cliente");
            throw new RpcException(new Status(StatusCode.Internal, "Internal server error"));
        }
    }

    public override async Task<ClienteResponse> UpdateCliente(UpdateClienteRequest request, ServerCallContext context)
    {
        try
        {
            var id = request.Id;
            var updateDto = new UpdateClienteDto
            {
                Nome = request.Nome,
                Email = request.Email,
                Telefone = string.IsNullOrWhiteSpace(request.Telefone) ? null : request.Telefone
            };

            var cliente = await _clienteService.UpdateAsync(id, updateDto, context.CancellationToken);
            return MapToClienteResponse(cliente);
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
            _logger.LogError(ex, "Error updating cliente");
            throw new RpcException(new Status(StatusCode.Internal, "Internal server error"));
        }
    }

    public override async Task<DeleteClienteResponse> DeleteCliente(DeleteClienteRequest request, ServerCallContext context)
    {
        try
        {
            var id = request.Id;
            await _clienteService.DeleteAsync(id, context.CancellationToken);

            return new DeleteClienteResponse
            {
                Success = true,
                Message = "Cliente deleted successfully"
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
            _logger.LogError(ex, "Error deleting cliente");
            throw new RpcException(new Status(StatusCode.Internal, "Internal server error"));
        }
    }

    public override async Task<AddCargoToClienteResponse> AddCargoToCliente(AddCargoToClienteRequest request, ServerCallContext context)
    {
        try
        {
            var clienteId = request.ClienteId;
            var cargoId = request.CargoId;

            await _clienteService.AddCargoAsync(clienteId, cargoId, context.CancellationToken);

            return new AddCargoToClienteResponse
            {
                Success = true,
                Message = "Cargo added to cliente successfully"
            };
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
            _logger.LogError(ex, "Error adding cargo to cliente");
            throw new RpcException(new Status(StatusCode.Internal, "Internal server error"));
        }
    }

    public override async Task<RemoveCargoFromClienteResponse> RemoveCargoFromCliente(RemoveCargoFromClienteRequest request, ServerCallContext context)
    {
        try
        {
            var clienteId = request.ClienteId;
            var cargoId = request.CargoId;

            await _clienteService.RemoveCargoAsync(clienteId, cargoId, context.CancellationToken);

            return new RemoveCargoFromClienteResponse
            {
                Success = true,
                Message = "Cargo removed from cliente successfully"
            };
        }
        catch (KeyNotFoundException ex)
        {
            throw new RpcException(new Status(StatusCode.NotFound, ex.Message));
        }
        catch (InvalidOperationException ex)
        {
            throw new RpcException(new Status(StatusCode.NotFound, ex.Message));
        }
        catch (FormatException)
        {
            throw new RpcException(new Status(StatusCode.InvalidArgument, "Invalid ID format"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error removing cargo from cliente");
            throw new RpcException(new Status(StatusCode.Internal, "Internal server error"));
        }
    }

    private static ClienteResponse MapToClienteResponse(ClienteDto cliente)
    {
        var response = new ClienteResponse
        {
            Id = cliente.Id,
            Nome = cliente.Nome,
            Email = cliente.Email,
            Cpf = cliente.Cpf,
            Telefone = cliente.Telefone ?? string.Empty,
            CreatedAt = cliente.CreatedAt.ToString("O"),
            UpdatedAt = cliente.UpdatedAt?.ToString("O") ?? string.Empty
        };

        if (cliente.Cargos != null && cliente.Cargos.Any())
        {
            response.Cargos.AddRange(cliente.Cargos.Select(c => new CargoMessage
            {
                Id = c.Id,
                Nome = c.Nome,
                Descricao = c.Descricao ?? string.Empty,
                NivelSalarial = (double)(c.NivelSalarial ?? 0)
            }));
        }

        return response;
    }
}
