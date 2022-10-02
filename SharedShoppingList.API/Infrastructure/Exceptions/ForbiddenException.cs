namespace SharedShoppingList.API.Infrastructure.Exceptions
{
    public class ForbiddenException : SharedShoppingListException
    {
        public ForbiddenException()
            : base("Forbidden")
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