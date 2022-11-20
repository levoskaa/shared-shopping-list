using System.Text;

namespace SharedShoppingList.API.Services
{
    public class InviteCodeService : IInviteCodeService
    {
        public string GenerateInviteCode()
        {
            const string validChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var sb = new StringBuilder();
            var random = new Random();

            for (int i = 0; i < 6; i++)
            {
                sb.Append(validChars[random.Next(validChars.Length)]);
            }

            return sb.ToString();
        }
    }
}
