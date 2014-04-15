using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSW.Framework.Common
{
    public class ErrorMessages
    {
        public const string Unexpected_Exception = "An unexpected error occured.";
    }

    public class UpdateResponse
    {
        public bool success { get; set; }

        public IEnumerable<string> errors { get; set; }
    }
}
