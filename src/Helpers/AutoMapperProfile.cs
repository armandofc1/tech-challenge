namespace WebApi.Helpers;

using AutoMapper;
using WebApi.Entities;
using WebApi.Models.Users;
using WebApi.Models.Clientes;
using WebApi.Models.Produtos;
using WebApi.Models.Pedidos;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        // UserCreateRequest -> User
        CreateMap<UserCreateRequest, User>();

        // UserUpdateRequest -> User
        CreateMap<UserUpdateRequest, User>()
            .ForAllMembers(x => x.Condition(
                (src, dest, prop) =>
                {
                    // ignore both null & empty string properties
                    if (prop == null) return false;
                    if (prop.GetType() == typeof(string) && string.IsNullOrEmpty((string)prop)) return false;

                    // ignore null role
                    if (x.DestinationMember.Name == "Role" && src.Role == null) return false;

                    return true;
                }
            ));

        // UserCreateRequest -> User
        CreateMap<ClienteCreateRequest, Cliente>();
        CreateMap<ClienteUpdateRequest, Cliente>();

        // ProdutoCreateRequest -> Produto
        CreateMap<ProdutoCreateRequest, Produto>();

        // ProdutoUpdateRequest -> Produto
        CreateMap<ProdutoUpdateRequest, Produto>()
            .ForAllMembers(x => x.Condition(
                (src, dest, prop) =>
                {
                    // ignore both null & empty string properties
                    if (prop == null) return false;
                    if (prop.GetType() == typeof(string) && string.IsNullOrEmpty((string)prop)) return false;

                    // ignore null role
                    if (x.DestinationMember.Name == "Categoria" && src.Categoria == null) return false;

                    return true;
                }
            ));

        // PedidoCreateRequest -> Pedido
        CreateMap<PedidoCreateRequest, Pedido>()
        .ForMember(t=>t.Items, opt=>opt.Ignore());

        // PedidoUpdateRequest -> Pedido
        CreateMap<PedidoUpdateRequest, Pedido>()
            .ForMember(t=>t.Items, opt=>opt.Ignore())
            .ForAllMembers(x => x.Condition(
                (src, dest, prop) =>
                {
                    // ignore both null & empty string properties
                    if (prop == null) return false;
                    if (prop.GetType() == typeof(string) && string.IsNullOrEmpty((string)prop)) return false;

                    // ignore null role
                    if (x.DestinationMember.Name == "Status" && src.Status == null) return false;

                    if (x.DestinationMember.Name == "Items" && src.Items == null) return false;

                    return true;
                }
            )
            );
    }
}