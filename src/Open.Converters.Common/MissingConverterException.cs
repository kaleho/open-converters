using System;

namespace Open.Converters
{
    public class MissingConverterException
        : Exception
    {
        public const string DefaultMessage =
            "Missing converter.";

        public MissingConverterException()
            : this(DefaultMessage) { }

        public MissingConverterException(
            string message)
            : base(message) { }

        public MissingConverterException(
            string message,
            Exception innerException)
            : base(message, innerException) { }
    }
}