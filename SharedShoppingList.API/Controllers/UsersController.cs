using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SharedShoppingList.API.Application.Commands.UserCommands;
using SharedShoppingList.API.Application.Commands.UserGroupCommands;
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

        [HttpPost]
        [AllowAnonymous]
        [ProducesResponseType(typeof(TokenViewModel), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorViewModel), (int)HttpStatusCode.BadRequest)]
        public async Task<TokenViewModel> CreateUser([FromBody] SignUpDto dto)
        {
            var createUserCommand = mapper.Map<CreateUserCommand>(dto);
            var token = await mediator.Send(createUserCommand);
            return mapper.Map<TokenViewModel>(token);
        }

        [HttpGet]
        [Authorize(Roles = "admin")]
        [ProducesResponseType(typeof(PaginatedListViewModel<UserViewModel>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType((int)HttpStatusCode.Forbidden)]
        public async Task<PaginatedListViewModel<UserViewModel>> GetUsers(
            [FromQuery] int? pageSize,
            [FromQuery] int? offset)
        {
            var getUsersCommand = new GetUsersCommand
            {
                PageSize = pageSize,
                Offset = offset,
            };
            var users = await mediator.Send(getUsersCommand);
            return mapper.Map<PaginatedListViewModel<UserViewModel>>(users);
        }

        [HttpPost("{username}/groups")]
        [Authorize(Policy = "MatchingUsername")]
        [ProducesResponseType(typeof(UserGroupViewModel), (int)HttpStatusCode.Created)]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType((int)HttpStatusCode.Forbidden)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
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
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<PaginatedListViewModel<UserGroupViewModel>> GetUserGroups(
            [FromQuery] int? pageSize,
            [FromQuery] int? offset)
        {
            var getUserGroupsCommand = new GetUserGroupsCommand
            {
                PageSize = pageSize,
                Offset = offset,
                UserId = identityHelper.GetAuthenticatedUserId(),
            };
            var userGroups = await mediator.Send(getUserGroupsCommand);
            return mapper.Map<PaginatedListViewModel<UserGroupViewModel>>(userGroups);
        }

        [HttpGet("{username}/groups/{groupId}")]
        [Authorize(Policy = "MatchingUsername")]
        [ProducesResponseType(typeof(UserGroupViewModel), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType((int)HttpStatusCode.Forbidden)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<UserGroupViewModel> GetUserGroup([FromRoute] int groupId)
        {
            var getUserGroupCommand = new GetUserGroupCommand
            {
                UserId = identityHelper.GetAuthenticatedUserId(),
                GroupId = groupId,
            };
            var userGroup = await mediator.Send(getUserGroupCommand);
            return mapper.Map<UserGroupViewModel>(userGroup);
        }

        [HttpPut("{username}/groups/{groupId}")]
        [Authorize(Policy = "MatchingUsername")]
        [ProducesResponseType(typeof(UserGroupViewModel), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType((int)HttpStatusCode.Forbidden)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<UserGroupViewModel> UpdateUserGroup(
            [FromRoute] int groupId,
            [FromBody] UpdateUserGroupDto dto)
        {
            var updateUserGroupCommand = mapper.Map<UpdateUserGroupCommand>(dto);
            updateUserGroupCommand.UserId = identityHelper.GetAuthenticatedUserId();
            updateUserGroupCommand.GroupId = groupId;
            var userGroup = await mediator.Send(updateUserGroupCommand);
            return mapper.Map<UserGroupViewModel>(userGroup);
        }

        [HttpDelete("{username}/groups/{userGroupId}")]
        [Authorize(Policy = "MatchingUsername")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType((int)HttpStatusCode.Forbidden)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
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