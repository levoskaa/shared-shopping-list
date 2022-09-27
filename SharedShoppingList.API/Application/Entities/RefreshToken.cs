namespace SharedShoppingList.API.Application.Entities
{
    public class RefreshToken
    {
        public string Token { get; set; }
        public DateTime ExpiryTime { get; set; }
        public string UserId { get; set; }
        public User? User { get; set; }
    }
}
