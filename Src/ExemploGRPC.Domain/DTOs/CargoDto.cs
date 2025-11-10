namespace ExemploGRPC.Domain.DTOs;

/// <summary>
/// Data Transfer Object for Cargo
/// </summary>
public class CargoDto
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string? Descricao { get; set; }
    public decimal? NivelSalarial { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public List<ClienteDto>? Clientes { get; set; }
}

/// <summary>
/// DTO for creating a new Cargo
/// </summary>
public class CreateCargoDto
{
    public string Nome { get; set; } = string.Empty;
    public string? Descricao { get; set; }
    public decimal? NivelSalarial { get; set; }
}

/// <summary>
/// DTO for updating a Cargo
/// </summary>
public class UpdateCargoDto
{
    public string Nome { get; set; } = string.Empty;
    public string? Descricao { get; set; }
    public decimal? NivelSalarial { get; set; }
}
