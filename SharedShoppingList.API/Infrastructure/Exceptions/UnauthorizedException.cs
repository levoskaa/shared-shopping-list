namespace SharedShoppingList.API.Infrastructure.Exceptions
{
    public class UnauthorizedException : SharedShoppingListException
    {
        public UnauthorizedException()
        {
        }

        public UnauthorizedException(string message)
            : base(message)
        {
        }

        public UnauthorizedException(string message, params string[] errorCodes)
            : base(message, errorCodes)
        {
        }
    }
}