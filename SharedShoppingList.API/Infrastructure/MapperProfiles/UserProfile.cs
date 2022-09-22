using AutoMapper;
using SharedShoppingList.API.Application.Commands;
using SharedShoppingList.API.Application.Dtos;

namespace SharedShoppingList.API.Infrastructure.MapperProfiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<SignInDto, SignInCommand>();
        }
    }
}
