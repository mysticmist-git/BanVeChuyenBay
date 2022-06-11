using System;
using System.Runtime.Serialization;

namespace FlightTicketSell.ViewModels
{
    [Serializable]
    internal class ExceptionEntity : Exception
    {
        public ExceptionEntity()
        {
        }

        public ExceptionEntity(string message) : base(message)
        {
        }

        public ExceptionEntity(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected ExceptionEntity(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}