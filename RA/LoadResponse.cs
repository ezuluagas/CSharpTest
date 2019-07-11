using System;
using System.Collections.Generic;
using System.Text;

namespace RA
{
    public class LoadResponse
    {
        public long Ticks { get; set; }
        public int StatusCode { get; set; }
        public LoadResponse(int statusCode,long ticks)
        {
            Ticks = ticks;
            StatusCode = statusCode;
        }

    }
}
