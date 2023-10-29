namespace MYTICKET.BASE.ENTITIES.Base
{
    public class Entity<TKey> : IEntity<TKey>
    {
        public TKey Id { get; set; }
    }

    public interface IEntity<TKey>
    {
        TKey Id { get; set; }
    }

    public class Entity : Entity<int>
    {
    }
}
