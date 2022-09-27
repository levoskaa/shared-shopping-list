using AutoMapper;
using SharedShoppingList.API.Application.ViewModels;
using SharedShoppingList.API.Infrastructure.Exceptions;

namespace SharedShoppingList.API.Infrastructure.MapperProfiles
{
    public class ExceptionProfile : Profile
    {
        public ExceptionProfile()
        {
            CreateMap<SharedShoppingListException, ErrorViewModel>()
                .ForMember(
                    viewModel => viewModel.Errors,
                    options => options.MapFrom(exception => exception.ErrorCodes))
                .IncludeAllDerived();
        }
    }
}