using MediatR;
using SharedShoppingList.API.Application.Entities;
using SharedShoppingList.API.Data;
using SharedShoppingList.API.Data.Repositories;
using SharedShoppingList.API.Infrastructure.Exceptions;

namespace SharedShoppingList.API.Application.Commands
{
    public class SignOutCommandHandler : IRequestHandler<SignOutCommand>
    {
        private readonly IRepository<User> userRepository;
        private readonly IUnitOfWork unitOfWork;

        public SignOutCommandHandler(
            IRepository<User> userRepository,
            IUnitOfWork unitOfWork)
        {
            this.userRepository = userRepository;
            this.unitOfWork = unitOfWork;
        }

        public async Task<Unit> Handle(SignOutCommand command, CancellationToken cancellationToken)
        {
            var user = await userRepository.GetByIdAsync(
                command.UserId,
                cancellationToken,
                nameof(User.RefreshTokens));
            if (user == null)
            {
                throw new EntityNotFoundException("User not found");
            }
            var refreshTokenToRemove = user.RefreshTokens
                .SingleOrDefault(token => token.Value == command.RefreshToken);
            if (refreshTokenToRemove != null)
            {
                user.RemoveRefreshToken(refreshTokenToRemove);
                userRepository.Update(user);
                await unitOfWork.SaveChangesAsync(cancellationToken);
            }
            return Unit.Value;
        }
    }
}