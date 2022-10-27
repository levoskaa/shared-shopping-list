using MediatR;
using Microsoft.AspNetCore.Authorization;
using SharedShoppingList.API.Application.Entities;
using SharedShoppingList.API.Data;
using SharedShoppingList.API.Data.Repositories;
using SharedShoppingList.API.Infrastructure.Authorization;
using SharedShoppingList.API.Infrastructure.Exceptions;
using SharedShoppingList.API.Services;
using System.Text;

namespace SharedShoppingList.API.Application.Commands.InviteCodeCommands
{
    public class GenerateInviteCodeCommandHandler : IRequestHandler<GenerateInviteCodeCommand, string>
    {
        private readonly IRepository<UserGroup> userGroupRepository;
        private readonly IAuthorizationService authorizationService;
        private readonly IIdentityHelper identityHelper;
        private readonly IUnitOfWork unitOfWork;

        public GenerateInviteCodeCommandHandler(
            IRepository<UserGroup> userGroupRepository,
            IAuthorizationService authorizationService,
            IIdentityHelper identityHelper,
            IUnitOfWork unitOfWork)
        {
            this.userGroupRepository = userGroupRepository;
            this.authorizationService = authorizationService;
            this.identityHelper = identityHelper;
            this.unitOfWork = unitOfWork;
        }

        public async Task<string> Handle(GenerateInviteCodeCommand command, CancellationToken cancellationToken)
        {
            var userGroup = await userGroupRepository.GetByIdAsync(
                command.GroupId,
                cancellationToken);
            if (userGroup == null)
            {
                throw new EntityNotFoundException("UserGroup not found");
            }

            var authorizationResult = await authorizationService.AuthorizeAsync(
                identityHelper.ClaimsPrincipal,
                userGroup,
                new UserGroupOwnerRequirement());
            if (!authorizationResult.Succeeded)
            {
                throw new ForbiddenException();
            }

            userGroup.InviteCode = GenerateInviteCode();
            await unitOfWork.SaveChangesAsync(cancellationToken);
            return userGroup.InviteCode;
        }

        private string GenerateInviteCode()
        {
            const string validChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var sb = new StringBuilder();
            var random = new Random();

            for (int i = 0; i < 6; i++)
            {
                sb.Append(validChars[random.Next(validChars.Length)]);
            }

            return sb.ToString();
        }
    }
}