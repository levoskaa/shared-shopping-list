namespace SharedShoppingList.API.Application.Entities
{
    public abstract class Entity
    {
        private int id;
        public virtual int Id
        {
            get
            {
                return id;
            }
            protected set
            {
                id = value;
            }
        }
    }
}