using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SharedShoppingList.API.Application.Commands;
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
    }
}