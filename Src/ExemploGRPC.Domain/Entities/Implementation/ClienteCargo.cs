using Foundation.Base.Domain.Implementation;

namespace ExemploGRPC.Domain.Entities.Implementation;

/// <summary>
/// ClienteCargo entity - Join table for N-to-N relationship
/// Represents the association between a Cliente and a Cargo
/// </summary>
public class ClienteCargo : Entity
{
    /// <summary>
    /// ID of the associated Cliente
    /// </summary>
    public int ClienteId { get; private set; }

    /// <summary>
    /// Navigation property to Cliente
    /// </summary>
    public virtual Cliente? Cliente { get; private set; }

    /// <summary>
    /// ID of the associated Cargo
    /// </summary>
    public int CargoId { get; private set; }

    /// <summary>
    /// Navigation property to Cargo
    /// </summary>
    public virtual Cargo? Cargo { get; private set; }

    /// <summary>
    /// Date when the client was assigned to this position
    /// </summary>
    public DateTime DataAtribuicao { get; private set; }

    /// <summary>
    /// Date when the client's assignment to this position ends (if applicable)
    /// </summary>
    public DateTime? DataFim { get; private set; }

    /// <summary>
    /// Indicates if this is the client's primary position
    /// </summary>
    public bool IsPrincipal { get; private set; }

    // EF Core requires a parameterless constructor
    private ClienteCargo() : base()
    {
    }

    public ClienteCargo(int clienteId, int cargoId, bool isPrincipal = false) : this()
    {
        ClienteId = clienteId;
        CargoId = cargoId;
        DataAtribuicao = DateTime.UtcNow;
        IsPrincipal = isPrincipal;
        Validate();
    }

    /// <summary>
    /// Sets the end date for this position assignment
    /// </summary>
    public void SetDataFim(DateTime dataFim)
    {
        if (dataFim < DataAtribuicao)
            throw new ArgumentException("Data fim não pode ser anterior à data de atribuição", nameof(dataFim));

        DataFim = dataFim;
        UpdateAuditInfo("System");
    }

    /// <summary>
    /// Marks this assignment as the principal position for the client
    /// </summary>
    public void SetAsPrincipal()
    {
        IsPrincipal = true;
        UpdateAuditInfo("System");
    }

    /// <summary>
    /// Removes the principal flag from this assignment
    /// </summary>
    public void RemovePrincipal()
    {
        IsPrincipal = false;
        UpdateAuditInfo("System");
    }

    public void Validate()
    {
        if (ClienteId == 0)
            throw new ArgumentException("ClienteId é obrigatório", nameof(ClienteId));

        if (CargoId == 0)
            throw new ArgumentException("CargoId é obrigatório", nameof(CargoId));

        if (DataFim.HasValue && DataFim.Value < DataAtribuicao)
            throw new ArgumentException("Data fim não pode ser anterior à data de atribuição", nameof(DataFim));
    }
}
