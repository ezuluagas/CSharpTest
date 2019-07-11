using System;
using System.Collections.Generic;
using System.Text;

namespace RestAssuered
{
    public class RestAssuredImp
    {
        public SetupContext Given()
        {
            return new SetupContext();
        }
    }
}
