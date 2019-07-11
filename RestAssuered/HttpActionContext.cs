using System;
using System.Collections.Generic;
using System.Text;

namespace RestAssuered
{
    public enum HttpActionType
    {
        GET,
        POST,
        PUT,
        PATCH,
        DELETE
    }
    public class HttpActionContext
    {
        private readonly SetupContext _setupContext;


        public HttpActionContext(SetupContext setupContext)
        {
            _setupContext = setupContext;
        }
    }
}
