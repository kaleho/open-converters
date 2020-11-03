using System;

namespace Open.Converters
{
    public class ConverterMismatchException
        : Exception
    {
        public const string DefaultMessage =
            "The object provided cannot be converted using the requested converter.";

        public ConverterMismatchException()
            : this(DefaultMessage) { }

        public ConverterMismatchException(
            string message)
            : base(message) { }

        public ConverterMismatchException(
            string message,
            Exception innerException)
            : base(message, innerException) { }
    }
}