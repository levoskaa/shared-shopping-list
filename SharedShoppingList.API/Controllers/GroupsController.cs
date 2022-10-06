using AutoMapper;
using MediatR;
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
    public class GroupsController : ControllerBase
    {
        private readonly IMediator mediator;
        private readonly IMapper mapper;
        private readonly IIdentityHelper identityHelper;

        public GroupsController(
            IMediator mediator,
            IMapper mapper,
            IIdentityHelper identityHelper)
        {
            this.mediator = mediator;
            this.mapper = mapper;
            this.identityHelper = identityHelper;
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

        [HttpGet("{groupId}/shooping-list-entries")]
        [ProducesResponseType(typeof(PaginatedListViewModel<ShoppingListEntryViewModel>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType((int)HttpStatusCode.Forbidden)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<PaginatedListViewModel<ShoppingListEntryViewModel>> GetShoppingListEntries(
            [FromRoute] int groupId,
            [FromQuery] int pageSize = 10,
            [FromQuery] int pageIndex = 1)
        {
            var getShoppingListEntriesCommand = new GetShoppingListEntriesCommand
            {
                GroupId = groupId,
                PageSize = pageSize,
                PageIndex = pageIndex,
            };
            var shoppingListEntries = await mediator.Send(getShoppingListEntriesCommand);
            return mapper.Map<PaginatedListViewModel<ShoppingListEntryViewModel>>(shoppingListEntries);
        }
    }
}