using System;
using System.Runtime.Serialization;

namespace XafDTW.Helpers
{
    public class MyException : Exception
    {
        public MyException()
        {
        }
        public MyException(string message) : base(message)
        {
            Logs.WriteLog("Exception", "", message);
        }

        public MyException(string message, Exception innerException) : base(message, innerException)
        {
            Logs.WriteLog("Exception", "", message);
        }

        protected MyException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
