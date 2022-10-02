using AutoMapper;
using SharedShoppingList.API.Application.Common;
using SharedShoppingList.API.Application.ViewModels;

namespace SharedShoppingList.API.Infrastructure.MapperProfiles
{
    public class CommonProfile : Profile
    {
        public CommonProfile()
        {
            CreateMap(typeof(PaginatedList<>), typeof(PaginatedListViewModel<>));
        }
    }
}