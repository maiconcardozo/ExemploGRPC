using AutoMapper;
using ExemploGRPC.Domain.DTOs;
using ExemploGRPC.Domain.Entities.Implementation;

namespace ExemploGRPC.Application.Mappings;

/// <summary>
/// AutoMapper profile for mapping between entities and DTOs
/// </summary>
public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // Cliente mappings
        CreateMap<Cliente, ClienteDto>()
            .ForMember(dest => dest.Cargos, opt => opt.MapFrom(src =>
                src.ClienteCargos.Select(cc => cc.Cargo).ToList()));

        CreateMap<CreateClienteDto, Cliente>()
            .ConstructUsing(src => new Cliente(src.Nome, src.Email, src.Cpf, src.Telefone));

        CreateMap<UpdateClienteDto, Cliente>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.Cpf, opt => opt.Ignore())
            .ForMember(dest => dest.DtCreated, opt => opt.Ignore())
            .ForMember(dest => dest.DtUpdated, opt => opt.Ignore())
            .ForMember(dest => dest.DtDeleted, opt => opt.Ignore())
            .ForMember(dest => dest.IsActive, opt => opt.Ignore())
            .ForMember(dest => dest.ClienteCargos, opt => opt.Ignore());

        // Cargo mappings
        CreateMap<Cargo, CargoDto>()
            .ForMember(dest => dest.Clientes, opt => opt.MapFrom(src =>
                src.ClienteCargos.Select(cc => cc.Cliente).ToList()));

        CreateMap<CreateCargoDto, Cargo>()
            .ConstructUsing(src => new Cargo(src.Nome, src.Descricao, src.NivelSalarial));

        CreateMap<UpdateCargoDto, Cargo>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.DtCreated, opt => opt.Ignore())
            .ForMember(dest => dest.DtUpdated, opt => opt.Ignore())
            .ForMember(dest => dest.DtDeleted, opt => opt.Ignore())
            .ForMember(dest => dest.IsActive, opt => opt.Ignore())
            .ForMember(dest => dest.ClienteCargos, opt => opt.Ignore());

        // ClienteCargo mappings
        CreateMap<ClienteCargo, ClienteCargoDto>();

        CreateMap<CreateClienteCargoDto, ClienteCargo>()
            .ConstructUsing(src => new ClienteCargo(src.ClienteId, src.CargoId, src.IsPrincipal));
    }
}
