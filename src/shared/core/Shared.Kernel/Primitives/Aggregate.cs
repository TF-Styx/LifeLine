namespace Shared.Kernel.Primitives
{
    public abstract class Aggregate<TId> : Entity<TId>, IAggregate where TId : notnull
    {
        protected Aggregate() { }
        protected Aggregate(TId id) : base(id) { }

        private readonly List<IDomainEvent> _domainEvents = [];

        public IReadOnlyCollection<IDomainEvent> GetEvents() => _domainEvents;
        public void AddEvents(IDomainEvent domainEvent) => _domainEvents.Add(domainEvent);
        public void ClearEvents() => _domainEvents.Clear();
    }
}
