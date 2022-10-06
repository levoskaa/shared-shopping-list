﻿using MediatR;
using SharedShoppingList.API.Application.Common;
using SharedShoppingList.API.Application.Entities;

namespace SharedShoppingList.API.Application.Commands
{
    public class GetShoppingListEntriesCommand :
        PaginationCommand, IRequest<PaginatedList<ShoppingListEntry>>
    {
        public int GroupId { get; set; }
    }
}