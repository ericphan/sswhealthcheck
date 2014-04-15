using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SSW.Framework.Common
{
    public static class IntegerExtensions
    {
        public static bool HasValidId(this int? value)
        {
            return value.HasValue && value > 0;
        }

        public static bool HasValidId(this int value)
        {
            return value > 0;
        }


        public static string ToCommaDelimitedString(this IList<int> value)
        {
            var sb = new StringBuilder();
            foreach (int i in value)
            {
                sb.Append(i.ToString() + ",");
            }
            if (sb.Length > 0) sb.Remove(sb.Length - 1, 1); // remove trailing ,
            return sb.ToString();
        }


        public static IList<int> ParseCommaDelimitedIntString(this string input)
        {
            var result = new List<int>();
            foreach (var s in input.Split(new char[] {','}, StringSplitOptions.RemoveEmptyEntries))
            {
                try
                {
                    int i = Int32.Parse(s);
                    result.Add(i);
                }
                catch (Exception)
                {
                    // ignore parse exceptions and continue
                }
            }
            return result;

        }




    }
}
