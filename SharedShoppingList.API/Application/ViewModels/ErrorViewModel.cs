namespace SharedShoppingList.API.Application.ViewModels
{
    public class ErrorViewModel
    {
        public string Message { get; set; }
        public IEnumerable<string> Errors { get; set; } = Enumerable.Empty<string>();
        public string? StackTrace { get; set; }
    }
}