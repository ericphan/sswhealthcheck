using System;

namespace SSW.Framework.Common.Exceptions
{
    public class DataOperationException : System.Exception
    {
        public DataOperationException(string message):base (message)
        {
            
        }
        public DataOperationException(string message, Exception ex)
            : base(message, ex)
        {

        }
    }

}