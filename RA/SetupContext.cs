using RA.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;

namespace RA
{
    public class SetupContext
    {
        private string _name;
        private HttpClient _httpClient;
        private string _host;
        private int _port;
        private string _uri;
        private bool _useHttps;
        private string _body;

        private readonly Dictionary<string, string> _cookies = new Dictionary<string, string>();
        private readonly Dictionary<string, string> _queryStrings = new Dictionary<string, string>();
        private readonly Dictionary<string, string> _headers = new Dictionary<string, string>();
        private readonly List<FileContent> _files = new List<FileContent>();
        private readonly Dictionary<string, string> _parameters = new Dictionary<string, string>();

        private TimeSpan? _timeout = null;
        public SetupContext Name(string name)
        {
            _name = name;
            return this;
        }
        public string Name()
        {
            return _name;
        }

        public SetupContext Cookie(string name, string value)
        {
            if (!_cookies.ContainsKey(name))
                _cookies.Add(name, value);
            return this;
        }

        public SetupContext Cookies(Dictionary<string, string> cookies)
        {
            foreach (var cookie in cookies)
            {
                if (!_cookies.ContainsKey(cookie.Key))
                    _cookies.Add(cookie.Key, cookie.Value);
            }
            return this;
        }

        public HttpActionContext When()
        {
            return new HttpActionContext(this);
        }

        public HttpClient HttpClient()
        {
            if (_httpClient != null) return _httpClient;

            var handler = new HttpClientHandler
            {
                AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate,
                //we will add cookies as a header latter in the request
                UseCookies = false,
            };
            _httpClient = new HttpClient(handler, true);
            return _httpClient;
        }

        public string Host()
        {
            return PortSpecified() ? $"{_host}:{_port}" : _host;
        }

        private bool PortSpecified()
        {
            return _port > 0 && _port != 88;
        }

        public bool UsesHttps()
        {
            return _useHttps;
        }

        public string Uri()
        {
            return _uri;
        }

        public Dictionary<string, string> Queries()
        {
            return _queryStrings.Select(x => new KeyValuePair<string, string>(x.Key, x.Value)).ToDictionary(x => x.Key, x => x.Value);
        }

        public List<string> HeaderAccept()
        {
            return GetHeaderFor("Accept", _headers);
        }

        public List<string> HeaderAcceptEncoding()
        {
            return GetHeaderFor("Accept-Encoding", _headers);
        }

        public List<string> HeaderAcceptCharset()
        {
            return GetHeaderFor("Accept-Charset", _headers);
        }


        private Func<string, IDictionary<string, string>, List<string>> GetHeaderFor = (filter, headers) =>
            {
                var value = headers.Where(x => x.Key.Equals(filter, StringComparison.InvariantCultureIgnoreCase))
                .Select(x => x.Value)
                .DefaultIfEmpty(string.Empty)
                .First();

                return !string.IsNullOrEmpty(value) ? value.Split(new[] { ',' }).Select(x => x.Trim()).ToList() : new List<string>();
            };

        public Dictionary<string, string> Cookies()
        {
            return _cookies.Select(x => new KeyValuePair<string, string>(x.Key, x.Value)).ToDictionary(x => x.Key, x => x.Value);
        }

        public SetupContext Timeout(int milliseconds)
        {
            _timeout = new TimeSpan(0, 0, 0, 0, milliseconds);
            return this;
        }
        public TimeSpan? Timeout()
        {
            return _timeout;
        }

        public List<FileContent> Files()
        {
            return _files.Select(x => new FileContent(x.FileName, x.ContentDispositionName, x.ContentType, x.Content)).ToList();
        }
        public Dictionary<string, string> Params()
        {
            return _parameters.Select(x => new KeyValuePair<string, string>(x.Key, x.Value)).ToDictionary(x => x.Key, x => x.Value);
        }

        public string Body()
        {
            return _body;
        }

        public List<string> HeaderContentType()
        {
            return GetHeaderFor("Content-Type", _headers);
        }
    }
}
