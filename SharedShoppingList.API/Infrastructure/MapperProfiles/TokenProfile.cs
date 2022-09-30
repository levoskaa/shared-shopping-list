using AutoMapper;
using SharedShoppingList.API.Application.Commands;
using SharedShoppingList.API.Application.Dtos;
using SharedShoppingList.API.Application.Entities;
using SharedShoppingList.API.Application.ViewModels;

namespace SharedShoppingList.API.Infrastructure.MapperProfiles
{
    public class TokenProfile : Profile
    {
        public TokenProfile()
        {
            CreateMap<AuthenticationResult, TokenViewModel>();
            CreateMap<RefreshTokenDto, RefreshAuthCommand>();
        }
    }
}