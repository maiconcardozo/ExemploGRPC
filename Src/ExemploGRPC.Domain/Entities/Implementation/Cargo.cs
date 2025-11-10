using ExemploGRPC.Domain.Entities.Base;

namespace ExemploGRPC.Domain.Entities.Implementation;

/// <summary>
/// Cargo entity - Aggregate Root
/// Represents a position/role in the system
/// </summary>
public class Cargo : EntityBase
{
    /// <summary>
    /// Position name
    /// </summary>
    public string Nome { get; private set; }

    /// <summary>
    /// Position description
    /// </summary>
    public string? Descricao { get; private set; }

    /// <summary>
    /// Salary level for this position
    /// </summary>
    public decimal? NivelSalarial { get; private set; }

    /// <summary>
    /// Collection of clients associated with this position
    /// Navigation property for N-to-N relationship
    /// </summary>
    public virtual ICollection<ClienteCargo> ClienteCargos { get; private set; }

    // EF Core requires a parameterless constructor
    private Cargo() : base()
    {
        Nome = string.Empty;
        ClienteCargos = new List<ClienteCargo>();
    }

    public Cargo(string nome, string? descricao = null, decimal? nivelSalarial = null) : this()
    {
        Nome = nome;
        Descricao = descricao;
        NivelSalarial = nivelSalarial;
        Validate();
    }

    /// <summary>
    /// Updates position information
    /// </summary>
    public void Update(string nome, string? descricao, decimal? nivelSalarial)
    {
        Nome = nome;
        Descricao = descricao;
        NivelSalarial = nivelSalarial;
        MarkAsUpdated();
        Validate();
    }

    /// <summary>
    /// Associates a client with this position
    /// </summary>
    public void AddCliente(Cliente cliente)
    {
        if (cliente == null)
            throw new ArgumentNullException(nameof(cliente));

        var exists = ClienteCargos.Any(cc => cc.ClienteId == cliente.Id);
        if (exists)
            throw new InvalidOperationException($"Cliente {cliente.Nome} já está associado ao cargo {Nome}");

        var clienteCargo = new ClienteCargo(cliente.Id, this.Id);
        ClienteCargos.Add(clienteCargo);
        MarkAsUpdated();
    }

    /// <summary>
    /// Removes a client from this position
    /// </summary>
    public void RemoveCliente(Guid clienteId)
    {
        var clienteCargo = ClienteCargos.FirstOrDefault(cc => cc.ClienteId == clienteId);
        if (clienteCargo == null)
            throw new InvalidOperationException("Cliente não encontrado para este cargo");

        ClienteCargos.Remove(clienteCargo);
        MarkAsUpdated();
    }

    public override void Validate()
    {
        if (string.IsNullOrWhiteSpace(Nome))
            throw new ArgumentException("Nome é obrigatório", nameof(Nome));

        if (NivelSalarial.HasValue && NivelSalarial.Value < 0)
            throw new ArgumentException("Nível salarial não pode ser negativo", nameof(NivelSalarial));
    }
}
