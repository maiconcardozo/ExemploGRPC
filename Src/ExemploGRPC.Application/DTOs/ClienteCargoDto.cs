namespace ExemploGRPC.Application.DTOs;

/// <summary>
/// Data Transfer Object for ClienteCargo
/// </summary>
public class ClienteCargoDto
{
    public Guid Id { get; set; }
    public Guid ClienteId { get; set; }
    public Guid CargoId { get; set; }
    public DateTime DataAtribuicao { get; set; }
    public DateTime? DataFim { get; set; }
    public bool IsPrincipal { get; set; }
    public ClienteDto? Cliente { get; set; }
    public CargoDto? Cargo { get; set; }
}

/// <summary>
/// DTO for creating a new ClienteCargo association
/// </summary>
public class CreateClienteCargoDto
{
    public Guid ClienteId { get; set; }
    public Guid CargoId { get; set; }
    public bool IsPrincipal { get; set; }
}

/// <summary>
/// DTO for updating a ClienteCargo association
/// </summary>
public class UpdateClienteCargoDto
{
    public DateTime? DataFim { get; set; }
    public bool IsPrincipal { get; set; }
}
