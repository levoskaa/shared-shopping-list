using AutoMapper;
using SharedShoppingList.API.Application.Commands.UserGroupCommands;
using SharedShoppingList.API.Application.Dtos;
using SharedShoppingList.API.Application.Entities;
using SharedShoppingList.API.Application.ViewModels;

namespace SharedShoppingList.API.Infrastructure.MapperProfiles
{
    public class UserGroupProfile : Profile
    {
        public UserGroupProfile()
        {
            CreateMap<UserGroup, UserGroupViewModel>()
                .ForMember(
                    dto => dto.MemberCount,
                    options => options.MapFrom(group => group.Members.Count));
            CreateMap<CreateUserGroupDto, CreateUserGroupCommand>();
            CreateMap<UpdateUserGroupDto, UpdateUserGroupCommand>();
            CreateMap<UserGroup, UserGroupDetailsViewModel>();
        }
    }
}