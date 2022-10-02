namespace SharedShoppingList.API.Application.Entities
{
    public class RefreshToken
    {
        public string Value { get; set; }
        public DateTime ExpiryTime { get; set; }
        public string UserId { get; set; }
        public virtual User? User { get; set; }
    }
}