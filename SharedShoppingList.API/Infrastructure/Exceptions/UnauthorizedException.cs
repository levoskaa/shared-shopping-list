namespace SharedShoppingList.API.Infrastructure.Exceptions
{
    public class UnauthorizedException : SharedShoppingListException
    {
        public UnauthorizedException()
            : base("Unauthorized")
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