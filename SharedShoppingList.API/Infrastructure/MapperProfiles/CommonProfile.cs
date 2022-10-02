using AutoMapper;
using SharedShoppingList.API.Application.Common;
using SharedShoppingList.API.Application.Dtos;

namespace SharedShoppingList.API.Infrastructure.MapperProfiles
{
    public class CommonProfile : Profile
    {
        public CommonProfile()
        {
            CreateMap(typeof(PaginatedList<>), typeof(PaginatedListDto<>));
        }
    }
}