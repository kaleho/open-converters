using System;
using System.Collections.Immutable;
using System.Reflection;

namespace Open.Converters
{
    public interface IConverter
    {
        ImmutableDictionary<TypePairKey, MethodInfo> Capabilities { get; }

        bool CanConvert(
            Type source,
            Type target);

        bool TryConvert<TFrom, TTo>(
            TFrom source,
            out TTo target)
            where TFrom : class
            where TTo : class;

        bool TryConvert<TTo>(
            object source,
            out TTo target)
            where TTo : class;

        bool TryConvert(
            object source,
            Type targetType,
            out object target);
    }
}