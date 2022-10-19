using AutoMapper;
using SharedShoppingList.API.Application.Commands;
using SharedShoppingList.API.Application.Commands.UserCommands;
using SharedShoppingList.API.Application.Dtos;
using SharedShoppingList.API.Application.Entities;
using SharedShoppingList.API.Application.ViewModels;

namespace SharedShoppingList.API.Infrastructure.MapperProfiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<SignInDto, SignInCommand>();
            CreateMap<RegisterDto, CreateUserCommand>();
            CreateMap<SignOutDto, SignOutCommand>();
            CreateMap<User, UserViewModel>()
                .ForMember(
                    vm => vm.GroupMembershipCount,
                    opt => opt.MapFrom(user => user.Groups.Count));
        }
    }
}