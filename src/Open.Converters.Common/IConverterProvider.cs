using System;

namespace Open.Converters
{
    public interface IConverterProvider
    {
        bool Exists<TSource, TTarget>();

        bool Exists(
            Type source,
            Type target);

        bool TryGet<TSource, TTarget>(
            out IConverter converter);

        bool TryGet(
            Type source,
            Type target,
            out IConverter converter);
    }
}