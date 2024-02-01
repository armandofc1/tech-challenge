namespace WebApi.Helpers;

using AutoMapper;
using WebApi.Entities;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        // CreateRequest -> User
        CreateMap<WebApi.Models.Users.CreateRequest, User>();

        // UpdateRequest -> User
        CreateMap<WebApi.Models.Users.UpdateRequest, User>()
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

        // CreateRequest -> Produto
        CreateMap<WebApi.Models.Produto.CreateRequest, Produto>();

        // UpdateRequest -> Produto
        CreateMap<WebApi.Models.Produto.UpdateRequest, Produto>()
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

        // CreateRequest -> Pedido
        CreateMap<WebApi.Models.Pedido.CreateRequest, Pedido>();

        // UpdateRequest -> Pedido
        CreateMap<WebApi.Models.Pedido.UpdateRequest, Pedido>()
            .ForAllMembers(x => x.Condition(
                (src, dest, prop) =>
                {
                    // ignore both null & empty string properties
                    if (prop == null) return false;
                    if (prop.GetType() == typeof(string) && string.IsNullOrEmpty((string)prop)) return false;

                    // ignore null role
                    if (x.DestinationMember.Name == "Status" && src.Status == null) return false;

                    return true;
                }
            ));
    }
}