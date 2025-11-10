using ExemploGRPC.Domain.Entities.Base;

namespace ExemploGRPC.Domain.Entities.Implementation;

/// <summary>
/// Cliente entity - Aggregate Root
/// Represents a client in the system
/// </summary>
public class Cliente : EntityBase
{
    /// <summary>
    /// Client's full name
    /// </summary>
    public string Nome { get; private set; }

    /// <summary>
    /// Client's email address
    /// </summary>
    public string Email { get; private set; }

    /// <summary>
    /// Client's phone number
    /// </summary>
    public string? Telefone { get; private set; }

    /// <summary>
    /// Client's CPF (Brazilian tax ID)
    /// </summary>
    public string Cpf { get; private set; }

    /// <summary>
    /// Collection of positions (cargos) associated with this client
    /// Navigation property for N-to-N relationship
    /// </summary>
    public virtual ICollection<ClienteCargo> ClienteCargos { get; private set; }

    // EF Core requires a parameterless constructor
    private Cliente() : base()
    {
        Nome = string.Empty;
        Email = string.Empty;
        Cpf = string.Empty;
        ClienteCargos = new List<ClienteCargo>();
    }

    public Cliente(string nome, string email, string cpf, string? telefone = null) : this()
    {
        Nome = nome;
        Email = email;
        Cpf = cpf;
        Telefone = telefone;
        Validate();
    }

    /// <summary>
    /// Updates client information
    /// </summary>
    public void Update(string nome, string email, string? telefone)
    {
        Nome = nome;
        Email = email;
        Telefone = telefone;
        MarkAsUpdated();
        Validate();
    }

    /// <summary>
    /// Associates a cargo (position) with this client
    /// </summary>
    public void AddCargo(Cargo cargo)
    {
        if (cargo == null)
            throw new ArgumentNullException(nameof(cargo));

        var exists = ClienteCargos.Any(cc => cc.CargoId == cargo.Id);
        if (exists)
            throw new InvalidOperationException($"Cargo {cargo.Nome} já está associado ao cliente {Nome}");

        var clienteCargo = new ClienteCargo(this.Id, cargo.Id);
        ClienteCargos.Add(clienteCargo);
        MarkAsUpdated();
    }

    /// <summary>
    /// Removes a cargo (position) from this client
    /// </summary>
    public void RemoveCargo(Guid cargoId)
    {
        var clienteCargo = ClienteCargos.FirstOrDefault(cc => cc.CargoId == cargoId);
        if (clienteCargo == null)
            throw new InvalidOperationException("Cargo não encontrado para este cliente");

        ClienteCargos.Remove(clienteCargo);
        MarkAsUpdated();
    }

    public override void Validate()
    {
        if (string.IsNullOrWhiteSpace(Nome))
            throw new ArgumentException("Nome é obrigatório", nameof(Nome));

        if (string.IsNullOrWhiteSpace(Email))
            throw new ArgumentException("Email é obrigatório", nameof(Email));

        if (string.IsNullOrWhiteSpace(Cpf))
            throw new ArgumentException("CPF é obrigatório", nameof(Cpf));

        if (!IsValidEmail(Email))
            throw new ArgumentException("Email inválido", nameof(Email));

        if (!IsValidCpf(Cpf))
            throw new ArgumentException("CPF inválido", nameof(Cpf));
    }

    private bool IsValidEmail(string email)
    {
        try
        {
            var addr = new System.Net.Mail.MailAddress(email);
            return addr.Address == email;
        }
        catch
        {
            return false;
        }
    }

    private bool IsValidCpf(string cpf)
    {
        // Remove non-numeric characters
        cpf = new string(cpf.Where(char.IsDigit).ToArray());

        if (cpf.Length != 11)
            return false;

        // Check if all digits are the same
        if (cpf.Distinct().Count() == 1)
            return false;

        // Validate CPF algorithm
        var sum = 0;
        for (int i = 0; i < 9; i++)
            sum += int.Parse(cpf[i].ToString()) * (10 - i);

        var remainder = sum % 11;
        var digit1 = remainder < 2 ? 0 : 11 - remainder;

        if (int.Parse(cpf[9].ToString()) != digit1)
            return false;

        sum = 0;
        for (int i = 0; i < 10; i++)
            sum += int.Parse(cpf[i].ToString()) * (11 - i);

        remainder = sum % 11;
        var digit2 = remainder < 2 ? 0 : 11 - remainder;

        return int.Parse(cpf[10].ToString()) == digit2;
    }
}
