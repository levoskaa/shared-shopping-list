using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SharedShoppingList.API.Application.Commands;
using SharedShoppingList.API.Application.Dtos;
using SharedShoppingList.API.Application.ViewModels;
using SharedShoppingList.API.Services;
using System.Net;

namespace SharedShoppingList.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IMediator mediator;
        private readonly IMapper mapper;
        private readonly IIdentityHelper identityHelper;

        public AuthController(
            IMediator mediator,
            IMapper mapper,
            IIdentityHelper identityHelper)
        {
            this.mediator = mediator;
            this.mapper = mapper;
            this.identityHelper = identityHelper;
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

        [HttpPost("register")]
        [ProducesResponseType(typeof(TokenViewModel), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorViewModel), (int)HttpStatusCode.BadRequest)]
        public async Task<TokenViewModel> Register([FromBody] RegisterDto dto)
        {
            var createUserCommand = mapper.Map<CreateUserCommand>(dto);
            var token = await mediator.Send(createUserCommand);
            return mapper.Map<TokenViewModel>(token);
        }

        [Authorize]
        [HttpPost("sign-out")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        public async Task SignOut([FromBody] SignOutDto dto)
        {
            var signOutCommand = mapper.Map<SignOutCommand>(dto);
            signOutCommand.UserId = identityHelper.GetAuthenticatedUserId();
            await mediator.Send(signOutCommand);
        }
    }
}