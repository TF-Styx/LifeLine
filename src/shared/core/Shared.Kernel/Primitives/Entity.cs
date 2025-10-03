﻿namespace Shared.Kernel.Primitives
{
    public abstract class Entity<TId> : IEquatable<Entity<TId>> where TId : notnull
    {
        public TId Id { get; protected init; } = default!;

        // Нужен для EFC
        protected Entity() { }
        protected Entity(TId id) => Id = id;

        public override bool Equals(object? obj)
        {
            if (obj is not Entity<TId> other)
                return false;

            if (ReferenceEquals(this, other))
                return true;

            if (Id.Equals(default) || other.Id.Equals(default))
                return false;

            return Id.Equals(other.Id);
        }

        public bool Equals(Entity<TId>? other) => Equals((object?)other);

        public override int GetHashCode() => HashCode.Combine(Id);

        public static bool operator == (Entity<TId>? left, Entity<TId>? right) => left?.Equals(right) ?? right is null;

        public static bool operator != (Entity<TId>? left, Entity<TId>? right) => !(left == right);
    }
}
