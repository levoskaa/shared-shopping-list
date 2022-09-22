namespace SharedShoppingList.API.Application.ViewModels
{
    public class TokenViewModel
    {
        public string AccessToken { get; set; }
        public DateTime ExpirationTime { get; set; }
    }
}
