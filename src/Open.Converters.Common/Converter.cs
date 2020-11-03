using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Reflection;

namespace Open.Converters
{
    public class Converter
        : IConverter
    {
        public Converter()
        {
            Capabilities = GetCapabilities(GetType());
        }

        public bool CanConvert(
            Type source,
            Type target)
        {
            return GetMethod(source, target) != null;
        }

        public ImmutableDictionary<TypePairKey, MethodInfo> Capabilities { get; protected set; }

        public bool TryConvert<TFrom, TTo>(
            TFrom source,
            out TTo target)
            where TFrom : class
            where TTo : class
        {
            return TryConvert<TTo>(source, out target);
        }

        public bool TryConvert<TTo>(
            object source,
            out TTo target)
            where TTo : class
        {
            var method = GetMethod(source.GetType(), typeof(TTo));

            target = (TTo) method?.Invoke(this, new[] { source });

            return target != null;
        }

        public bool TryConvert(
            object source,
            Type targetType,
            out object target)
        {
            var method = GetMethod(source.GetType(), targetType);

            target = method?.Invoke(this, new[] { source });

            return target != null;
        }

        protected static ImmutableDictionary<TypePairKey, MethodInfo> GetCapabilities(
            Type type)
        {
            var methodCache =
                type
                    .GetMethods(
                        BindingFlags.FlattenHierarchy |
                        BindingFlags.Instance |
                        BindingFlags.NonPublic)
                    .Where(
                        x => (x.Name == nameof(Convert) || x.Name.StartsWith(nameof(Convert))) &&
                             x.GetParameters().Length == 1)
                    .ToList();

            var capabilities =
                methodCache.Select(
                    x =>
                    {
                        var key =
                            new TypePairKey(
                                x.GetParameters()[0].ParameterType,
                                x.ReturnType);

                        return new KeyValuePair<TypePairKey, MethodInfo>(key, x);
                    });

            var returnValue = ImmutableDictionary<TypePairKey, MethodInfo>.Empty.AddRange(capabilities);

            return returnValue;
        }

        protected MethodInfo GetMethod(
            Type source,
            Type target)
        {
            var typeKey = new TypePairKey(source, target);

            var capability =
                Capabilities.FirstOrDefault(
                    x => x.Key.Equals(typeKey) ||
                         x.Key.Implements(typeKey));

            return capability.Value;
        }
    }
}