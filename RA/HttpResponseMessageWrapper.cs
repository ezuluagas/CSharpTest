using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace RA
{
    public class HttpResponseMessageWrapper
    {
        public HttpResponseMessage Response { get; set; }
        public TimeSpan ElaspedExecution { get; set; }
    }
}
