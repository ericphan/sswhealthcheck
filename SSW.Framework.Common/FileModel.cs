using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSW.Framework.Common
{
    public class FileModel
    {

        public System.IO.Stream InputStream { get; set; }

        public System.IO.Stream OutputStream { get; set; }

        public String Name { get; set; }

        public String Path { get; set; }

        public String ContentType { get; set; }

    }
}
