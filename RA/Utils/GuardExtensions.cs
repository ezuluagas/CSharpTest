using System;
using System.Collections.Generic;
using System.Text;

namespace RA.Utils
{
    public static class GuardExtensions
    {
        public static string FixProtocol(this string source, bool useHttps)
        {
            var defaultPortocol = useHttps ? "https" : "http";
            if (!source.StartsWith("http://") && !source.StartsWith("https://"))
                return $"{defaultPortocol}://" + source;

            return source;
        }
    }
}
