using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SharedShoppingList.API.Application.Commands;
using SharedShoppingList.API.Application.Dtos;
using SharedShoppingList.API.Application.ViewModels;
using System.Net;

namespace SharedShoppingList.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IMediator mediator;
        private readonly IMapper mapper;

        public AuthController(IMediator mediator, IMapper mapper)
        {
            this.mediator = mediator;
            this.mapper = mapper;
        }

        [HttpPost("sign-in")]
        [ProducesResponseType(typeof(TokenViewModel), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorViewModel), (int)HttpStatusCode.BadRequest)]
        public async Task<TokenViewModel> SignIn([FromBody] SignInDto dto)
        {
            var signInCommand = mapper.Map<SignInCommand>(dto);
            var token = await mediator.Send(signInCommand);
            return mapper.Map<TokenViewModel>(token);
        }
    }
}