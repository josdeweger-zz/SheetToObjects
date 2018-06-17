using System;
using System.Runtime.Serialization;

namespace SheetToObjects.Lib.Exceptions
{
    public class MappingConfigurationException : Exception
    {
        public MappingConfigurationException()
        {
        }

        public MappingConfigurationException(string message) : base(message)
        {
        }

        public MappingConfigurationException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected MappingConfigurationException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}