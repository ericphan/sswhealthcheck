using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSW.Framework.Common
{
    public static class ExceptionHelpers
    {
        public static Exception InnerMostException(this Exception exception)
        {

            Exception current = exception;

            while (current.InnerException != null)
            {
                current = current.InnerException;
            }

            return current;
        }
    }
}
