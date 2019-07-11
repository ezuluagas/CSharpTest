using RA.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace RA
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
        private string _url;
        private HttpActionType _httpAction;
        private bool _isLoadTest = false;
        private int _threads = 1;
        private int _seconds = 60;
        public HttpActionContext(SetupContext setupContext)
        {
            _setupContext = setupContext;
        }

        public ExecutionContext Get(string url = null)
        {
            return SetHttpAction(url, HttpActionType.GET);
        }

        private ExecutionContext SetHttpAction(string url, HttpActionType actionType)
        {
            SetUrl(url);
            _httpAction = actionType;
            return new ExecutionContext(_setupContext, this);
        }

        public void SetUrl(string url)
        {
            if (string.IsNullOrEmpty(url) && string.IsNullOrEmpty(_setupContext.Host()))
                throw new ArgumentException("url must be provided");

            var uri = string.IsNullOrEmpty(url)
                ? new Uri(url.FixProtocol(_setupContext.UsesHttps()))
                : new Uri(new Uri(_setupContext.Host().FixProtocol(_setupContext.UsesHttps())), _setupContext.Uri());

            _url = uri.OriginalString;
        }

       
        public bool IsLoadTest()
        {
            return _isLoadTest;
        }

        public int Threads()
        {
            return _threads;
        }

        public HttpActionType HttpAction()
        {
            return _httpAction;
        }

        public string Url()
        {
            return _url;
        }

        public int Seconds()
        {
            return _seconds;
        }
    }
}
