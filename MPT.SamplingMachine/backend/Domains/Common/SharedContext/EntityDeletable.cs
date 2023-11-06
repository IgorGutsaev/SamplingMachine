namespace MPT.Vending.Domains.SharedContext
{
    public abstract class EntityDeletable<T> : Entity<T>
    {
        public bool Deleted { get; set; }
    }
}
