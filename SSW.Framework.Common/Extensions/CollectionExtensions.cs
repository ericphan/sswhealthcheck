using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSW.Framework.Common
{
    public static class CollectionExtensions
    {

        public static List<int> GetItemsNotIn(this List<int> master, List<int> other)
        {
            var list = master.FindAll(c => other.Contains(c) == false);
            return list;
        }
        public static List<int> GetItemsIn(this List<int> master, List<int> other)
        {
            var list = master.FindAll(other.Contains);
            return list;
        }
    }
}
