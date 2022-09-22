namespace SharedShoppingList.API.Infrastructure.Exceptions
{
    public class ForbiddenException : SharedShoppingListException
    {
        public ForbiddenException()
        {
        }

        public ForbiddenException(string message)
            : base(message)
        {
        }

        public ForbiddenException(string message, params string[] errorCodes)
            : base(message, errorCodes)
        {
        }
    }
}