using System;
using System.Collections.Generic;
using System.Text;

namespace RA.Utils
{
    public class AssertException:Exception
    {
        public AssertException(string message = null) : base(message)
        { }
    }
}
