namespace ExemploGRPC.Application.DTOs;

/// <summary>
/// Data Transfer Object for Cliente
/// </summary>
public class ClienteDto
{
    public Guid Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Cpf { get; set; } = string.Empty;
    public string? Telefone { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public List<CargoDto> Cargos { get; set; } = new();
}

/// <summary>
/// DTO for creating a new Cliente
/// </summary>
public class CreateClienteDto
{
    public string Nome { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Cpf { get; set; } = string.Empty;
    public string? Telefone { get; set; }
}

/// <summary>
/// DTO for updating a Cliente
/// </summary>
public class UpdateClienteDto
{
    public string Nome { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? Telefone { get; set; }
}
