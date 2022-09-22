using AutoMapper;
using SharedShoppingList.API.Application.ViewModels;
using SharedShoppingList.API.Infrastructure.Exceptions;

namespace SharedShoppingList.API.Infrastructure.MapperProfiles
{
    public class ExceptionProfile : Profile
    {
        public ExceptionProfile()
        {
            CreateMap<DomainException, ErrorViewModel>();
            CreateMap<EntityNotFoundException, ErrorViewModel>();
            CreateMap<ForbiddenException, ErrorViewModel>();
            CreateMap<Exception, ErrorViewModel>();
        }
    }
}