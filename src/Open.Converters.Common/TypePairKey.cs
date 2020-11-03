using System;
using System.Reflection;

namespace Open.Converters
{
    public class TypePairKey
    {
        public TypePairKey(
            Type source,
            Type target)
        {
            Source = source;
            Target = target;
        }

        public Type Source { get; }

        public Type Target { get; }

        public static bool operator !=(
            TypePairKey a,
            TypePairKey b)
        {
            return !(a == b);
        }

        public static bool operator ==(
            TypePairKey a,
            TypePairKey b)
        {
            if (ReferenceEquals(a, null))
            {
                return ReferenceEquals(b, null);
            }

            return a.Equals(b);
        }

        public override bool Equals(
            object obj)
        {
            return
                ReferenceEquals(this, obj) ||
                obj is TypePairKey other &&
                GetHashCode().Equals(other.GetHashCode());
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Source, Target);
        }

        public bool Implements(
            object obj)
        {
            return
                ReferenceEquals(this, obj) ||
                obj is TypePairKey other &&
                other.Source.Implements(Source) &&
                other.Target.Implements(Target);
        }
    }
}