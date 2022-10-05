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

        [HttpPost("{username}/groups")]
        [Authorize(Policy = "MatchingUsername")]
        [ProducesResponseType(typeof(UserGroupViewModel), (int)HttpStatusCode.Created)]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType((int)HttpStatusCode.Forbidden)]
        public async Task<UserGroupViewModel> CreateUserGroup([FromBody] CreateUserGroupDto dto)
        {
            var createUserGroupCommand = mapper.Map<CreateUserGroupCommand>(dto);
            createUserGroupCommand.UserId = identityHelper.GetAuthenticatedUserId();
            var createdUserGroup = await mediator.Send(createUserGroupCommand);
            HttpContext.Response.StatusCode = (int)HttpStatusCode.Created;
            return mapper.Map<UserGroupViewModel>(createdUserGroup);
        }

        [HttpGet("{username}/groups")]
        [Authorize(Policy = "MatchingUsername")]
        [ProducesResponseType(typeof(PaginatedListViewModel<UserGroupViewModel>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType((int)HttpStatusCode.Forbidden)]
        public async Task<PaginatedListViewModel<UserGroupViewModel>> GetUserGroups(
            [FromQuery] int pageSize = 10,
            [FromQuery] int pageIndex = 1)
        {
            var getUserGroupsCommand = new GetUserGroupsCommand
            {
                PageSize = pageSize,
                PageIndex = pageIndex,
                UserId = identityHelper.GetAuthenticatedUserId(),
            };
            var userGroups = await mediator.Send(getUserGroupsCommand);
            return mapper.Map<PaginatedListViewModel<UserGroupViewModel>>(userGroups);
        }

        [HttpDelete("{username}/groups/{userGroupId}")]
        [Authorize(Policy = "MatchingUsername")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType((int)HttpStatusCode.Forbidden)]
        public async Task DeleteUserGroup([FromRoute] int userGroupId)
        {
            var deleteUserGroupCommand = new DeleteUserGroupCommand
            {
                UserId = identityHelper.GetAuthenticatedUserId(),
                UserGroupId = userGroupId,
            };
            await mediator.Send(deleteUserGroupCommand);
            HttpContext.Response.StatusCode = (int)HttpStatusCode.NoContent;
        }
    }
}