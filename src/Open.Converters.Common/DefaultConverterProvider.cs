using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Microsoft.Extensions.Logging;

namespace Open.Converters
{
    public class DefaultConverterProvider
        : IConverterProvider
    {
        private readonly ConcurrentDictionary<TypePairKey, Lazy<IConverter>> _cache;

        private readonly List<IConverter> _converterList;

        private readonly ILogger<DefaultConverterProvider> _log;

        public DefaultConverterProvider(
            ILogger<DefaultConverterProvider> log,
            IEnumerable<IConverter> converters)
        {
            _log = log;

            _cache = new ConcurrentDictionary<TypePairKey, Lazy<IConverter>>();

            _converterList = new List<IConverter>(converters);
        }

        public bool Exists<TSource, TTarget>()
        {
            return Exists(typeof(TSource), typeof(TTarget));
        }

        public bool Exists(
            Type source, 
            Type target)
        {
            return TryGet(source, target, out var converter);
        }

        public bool TryGet<TSource, TTarget>(
            out IConverter converter)
        {
            return TryGet(
                typeof(TSource),
                typeof(TTarget),
                out converter);
        }

        public ImmutableDictionary<TypePairKey, Lazy<IConverter>> Cache =>
            ImmutableDictionary.CreateRange(
                _cache);

        public bool TryGet(
            Type source,
            Type target,
            out IConverter converter)
        {
            converter = default;

            var typePairKey = new TypePairKey(source, target);

            if (_cache.ContainsKey(typePairKey))
            {
                converter = _cache[typePairKey].Value;
            }
            else
            {
                var locatedConverter =
                    _converterList.FirstOrDefault(
                        x => x.CanConvert(source, target));

                if (locatedConverter != null)
                {
                    converter =
                        _cache
                            .AddOrUpdate(
                                typePairKey,
                                key => new Lazy<IConverter>(() => locatedConverter),
                                (key, existing) => new Lazy<IConverter>(() => locatedConverter))
                            .Value;
                }
            }

            return converter != default;
        }
    }
}