using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSW.HealthCheck.Mvc4
{
    using System.IO;
    using System.Text.RegularExpressions;
    using System.Web.Mvc;

    public static class UrlHelperExtensions
    {
        public static string ContentWithWildcard(this UrlHelper helper, string path, string filePattern, string defaultValue)
        {
            var physicalPath = helper.RequestContext.HttpContext.Server.MapPath(path);
            if (string.IsNullOrEmpty(physicalPath) || !Directory.Exists(physicalPath))
            {
                return helper.Content(defaultValue);
            }

            foreach (var filePath in Directory.GetFiles(physicalPath))
            {
                var fileName = Path.GetFileName(filePath);
                if (!string.IsNullOrEmpty(fileName) && Regex.IsMatch(fileName, filePattern))
                {
                    return helper.Content(path) + "/" + fileName;
                }
            }

            return helper.Content(defaultValue);
        }
    }
}
