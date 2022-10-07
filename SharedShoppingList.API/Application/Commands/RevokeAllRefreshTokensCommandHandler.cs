using MediatR;
using SharedShoppingList.API.Application.Entities;
using SharedShoppingList.API.Data;
using SharedShoppingList.API.Data.Repositories;
using SharedShoppingList.API.Infrastructure.Exceptions;

namespace SharedShoppingList.API.Application.Commands
{
    public class RevokeAllRefreshTokensCommandHandler : IRequestHandler<RevokeAllRefreshTokensCommand>
    {
        private readonly IRepository<User> userRepository;
        private readonly IUnitOfWork unitOfWork;

        public RevokeAllRefreshTokensCommandHandler(
            IRepository<User> userRepository,
            IUnitOfWork unitOfWork)
        {
            this.userRepository = userRepository;
            this.unitOfWork = unitOfWork;
        }

        public async Task<Unit> Handle(RevokeAllRefreshTokensCommand command, CancellationToken cancellationToken)
        {
            var user = await userRepository.GetByIdAsync(
                command.UserId,
                cancellationToken,
                nameof(User.RefreshTokens));
            if (user == null)
            {
                throw new EntityNotFoundException("User not found");
            }
            user.RemoveAllRefreshTokens();
            await unitOfWork.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }
    }
}