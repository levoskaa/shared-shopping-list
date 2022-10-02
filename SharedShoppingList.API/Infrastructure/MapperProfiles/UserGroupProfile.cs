using AutoMapper;
using SharedShoppingList.API.Application.Dtos;
using SharedShoppingList.API.Application.Entities;

namespace SharedShoppingList.API.Infrastructure.MapperProfiles
{
    public class UserGroupProfile : Profile
    {
        public UserGroupProfile()
        {
            CreateMap<UserGroup, UserGroupDto>()
                .ForMember(
                    dto => dto.MemberCount,
                    options => options.MapFrom(group => group.Members.Count));
        }
    }
}