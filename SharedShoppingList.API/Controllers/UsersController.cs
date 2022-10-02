using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SharedShoppingList.API.Application.Commands;
using SharedShoppingList.API.Application.Common;
using SharedShoppingList.API.Application.Dtos;
using SharedShoppingList.API.Application.ViewModels;
using SharedShoppingList.API.Services;
using System.Net;

namespace SharedShoppingList.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IMediator mediator;
        private readonly IIdentityHelper identityHelper;
        private readonly IMapper mapper;

        public UsersController(
            IMediator mediator,
            IIdentityHelper identityHelper,
            IMapper maper)
        {
            this.mediator = mediator;
            this.identityHelper = identityHelper;
            this.mapper = maper;
        }

        [HttpGet("{username}/groups")]
        [Authorize]
        [ProducesResponseType(typeof(PaginatedListDto<UserGroupDto>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorViewModel), (int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType(typeof(ErrorViewModel), (int)HttpStatusCode.Forbidden)]
        public async Task<PaginatedListDto<UserGroupDto>> GetUserGroups(
            [FromRoute] string username,
            [FromQuery] int pageSize = 10,
            [FromQuery] int pageIndex = 1)
        {
            var getUserGroupsCommand = new GetUserGroupsCommand
            {
                PageSize = pageSize,
                PageIndex = pageIndex,
                Username = username,
                UserId = identityHelper.GetAuthenticatedUserId(),
            };
            var userGroups = await mediator.Send(getUserGroupsCommand);
            return mapper.Map<PaginatedListDto<UserGroupDto>>(userGroups);
        }
    }
}