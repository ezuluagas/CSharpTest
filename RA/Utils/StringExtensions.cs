using System;
using System.Collections.Generic;
using System.Text;

namespace RA.Utils
{
    public static class StringExtensions
    {
        public static string Quote(this string source)
        {
            return "\"" + source + "\"";
        }
    }
}
