namespace SharedShoppingList.API.Infrastructure.Exceptions
{
    public class DomainException : SharedShoppingListException
    {
        public DomainException()
        {
        }

        public DomainException(string message)
            : base(message)
        {
        }

        public DomainException(string message, params string[] errorCodes)
            : base(message, errorCodes)
        {
        }
    }
}
