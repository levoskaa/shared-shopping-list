using AutoMapper;
using SharedShoppingList.API.Application.Commands;
using SharedShoppingList.API.Application.Dtos;
using SharedShoppingList.API.Application.Entities;
using SharedShoppingList.API.Application.ViewModels;

namespace SharedShoppingList.API.Infrastructure.MapperProfiles
{
    public class ShoppingListEntryProfile : Profile
    {
        public ShoppingListEntryProfile()
        {
            CreateMap<CreateShoppingListEntryDto, CreateShoppingListEntryCommand>();
            CreateMap<ShoppingListEntry, ShoppingListEntryViewModel>();
        }
    }
}