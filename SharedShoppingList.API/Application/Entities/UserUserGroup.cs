namespace SharedShoppingList.API.Application.Entities
{
    public class UserUserGroup
    {
        public string UserId { get; set; }
        public virtual User User { get; set; }

        public int GroupId { get; set; }
        public virtual UserGroup Group { get; set; }
    }
}