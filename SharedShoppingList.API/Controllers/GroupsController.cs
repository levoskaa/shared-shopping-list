using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SharedShoppingList.API.Application.Commands.InviteCodeCommands;
using SharedShoppingList.API.Application.Commands.ShoppingListEntryCommands;
using SharedShoppingList.API.Application.Dtos;
using SharedShoppingList.API.Application.ViewModels;
using System.Net;

namespace SharedShoppingList.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GroupsController : ControllerBase
    {
        private readonly IMediator mediator;
        private readonly IMapper mapper;

        public GroupsController(
            IMediator mediator,
            IMapper mapper)
        {
            this.mediator = mediator;
            this.mapper = mapper;
        }

        [HttpPost("{groupId}/shopping-list-entries")]
        [ProducesResponseType(typeof(ShoppingListEntryViewModel), (int)HttpStatusCode.Created)]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType((int)HttpStatusCode.Forbidden)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ShoppingListEntryViewModel> CreateShoppingListEntry(
            [FromRoute] int groupId,
            [FromBody] CreateShoppingListEntryDto dto)
        {
            var createShoppingListEntryCommand = mapper.Map<CreateShoppingListEntryCommand>(dto);
            createShoppingListEntryCommand.GroupId = groupId;
            var createdShoppingListEntry = await mediator.Send(createShoppingListEntryCommand);
            HttpContext.Response.StatusCode = (int)HttpStatusCode.Created;
            return mapper.Map<ShoppingListEntryViewModel>(createdShoppingListEntry);
        }

        [HttpGet("{groupId}/shopping-list-entries")]
        [ProducesResponseType(typeof(PaginatedListViewModel<ShoppingListEntryViewModel>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType((int)HttpStatusCode.Forbidden)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<PaginatedListViewModel<ShoppingListEntryViewModel>> GetShoppingListEntries(
            [FromRoute] int groupId,
            [FromQuery] int? pageSize,
            [FromQuery] int? offset)
        {
            var getShoppingListEntriesCommand = new GetShoppingListEntriesCommand
            {
                GroupId = groupId,
                PageSize = pageSize,
                Offset = offset,
            };
            var shoppingListEntries = await mediator.Send(getShoppingListEntriesCommand);
            return mapper.Map<PaginatedListViewModel<ShoppingListEntryViewModel>>(shoppingListEntries);
        }

        [HttpGet("{groupId}/shopping-list-entries/{shoppingListEntryId}")]
        [ProducesResponseType(typeof(ShoppingListEntryViewModel), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType((int)HttpStatusCode.Forbidden)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ShoppingListEntryViewModel> GetShoppingListEntry(
            [FromRoute] int groupId,
            [FromRoute] int shoppingListEntryId)
        {
            var getShoppingListEntryCommand = new GetShoppingListEntryCommand
            {
                GroupId = groupId,
                ShoppingListEntryId = shoppingListEntryId,
            };
            var shoppingListEntry = await mediator.Send(getShoppingListEntryCommand);
            return mapper.Map<ShoppingListEntryViewModel>(shoppingListEntry);
        }

        [HttpPut("{groupId}/shopping-list-entries/{shoppingListEntryId}")]
        [ProducesResponseType(typeof(ShoppingListEntryViewModel), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType((int)HttpStatusCode.Forbidden)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ShoppingListEntryViewModel> UpdateShoppingListEntry(
            [FromRoute] int groupId,
            [FromRoute] int shoppingListEntryId,
            [FromBody] UpdateShoppingListEntryDto dto)
        {
            var updateShoppingListEntryCommand = mapper.Map<UpdateShoppingListEntryCommand>(dto);
            updateShoppingListEntryCommand.ShoppingListEntryId = shoppingListEntryId;
            updateShoppingListEntryCommand.GroupId = groupId;
            var updatedShoppingListEntry = await mediator.Send(updateShoppingListEntryCommand);
            return mapper.Map<ShoppingListEntryViewModel>(updatedShoppingListEntry);
        }

        [HttpDelete("{groupId}/shopping-list-entries/{shoppingListEntryId}")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType((int)HttpStatusCode.Forbidden)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task DeleteShoppingListEntry(
            [FromRoute] int groupId,
            [FromRoute] int shoppingListEntryId)
        {
            var deleteShoppingListEntryCommand = new DeleteShoppingListEntryCommand
            {
                ShoppingListEntryId = shoppingListEntryId,
                GroupId = groupId,
            };
            await mediator.Send(deleteShoppingListEntryCommand);
            HttpContext.Response.StatusCode = (int)HttpStatusCode.NoContent;
        }

        [HttpPost("{groupId}/invite-codes")]
        [ProducesResponseType(typeof(InviteCodeViewModel), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType((int)HttpStatusCode.Forbidden)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<InviteCodeViewModel> GenerateInviteCode([FromRoute] int groupId)
        {
            var generateInviteCodeCommand = new GenerateInviteCodeCommand
            {
                GroupId = groupId,
            };
            var inviteCode = await mediator.Send(generateInviteCodeCommand);
            return new InviteCodeViewModel
            {
                Value = inviteCode,
            };
        }

        [HttpGet("{groupId}/invite-codes")]
        [ProducesResponseType(typeof(InviteCodeViewModel), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType((int)HttpStatusCode.Forbidden)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<InviteCodeViewModel> GetInviteCodes([FromRoute] int groupId)
        {
            var getInviteCodeCommand = new GetInviteCodeCommand
            {
                GroupId = groupId,
            };
            var inviteCode = await mediator.Send(getInviteCodeCommand);
            return new InviteCodeViewModel
            {
                Value = inviteCode,
            };
        }

        [HttpPost("join/{inviteCode}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        public async Task JoinGroup([FromRoute] string inviteCode)
        {
            var joinUserGroupCommand = new JoinUserGroupCommand
            {
                InviteCode = inviteCode,
            };
            await mediator.Send(joinUserGroupCommand);
        }
    }
}