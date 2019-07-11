using Newtonsoft.Json.Linq;
using RA.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Xml.Linq;

namespace RA
{
    public class ResponseContext
    {
        private readonly HttpStatusCode _statusCode;
        private readonly string _content;
        private dynamic _parsedContent;
        private readonly Dictionary<string, IEnumerable<string>> _headers = new Dictionary<string, IEnumerable<string>>();
        private TimeSpan _elapsedExecutionTime;
        private readonly Dictionary<string, double> _loadValues = new Dictionary<string, double>();
        private readonly Dictionary<string, bool> _assertions = new Dictionary<string, bool>();
        private readonly List<LoadResponse> _loadResponses;
        private bool _isSchemaValid = true;
        private List<string> _schemaErrors = new List<string>();
        public ResponseContext(HttpStatusCode statusCode, string content, Dictionary<string, IEnumerable<string>> headers, TimeSpan elaspedExecutionTime, List<LoadResponse> loadResponses)
        {
            _statusCode = statusCode;
            _content = content;
            _headers = headers;
            _elapsedExecutionTime = elaspedExecutionTime;
            _loadResponses = loadResponses ?? new List<LoadResponse>();

            Initialize();
        }

        private void Initialize()
        {
            Parse();
            ParseLoad();
        }

        private void Parse()
        {
            var contentType = ContentType();

            if (contentType.Contains("json"))
            {
                if (!string.IsNullOrEmpty(_content))
                {
                    try
                    {
                        _parsedContent = JObject.Parse(_content);
                        return;
                    }
                    catch
                    {
                    }

                    try
                    {
                        _parsedContent = JArray.Parse(_content);
                        return;
                    }
                    catch
                    {
                    }
                }
                else
                {
                    return;
                }
            }
            else if (contentType.Contains("xml"))
            {
                if (!string.IsNullOrEmpty(_content))
                {
                    try
                    {
                        _parsedContent = XDocument.Parse(_content);
                        return;
                    }
                    catch
                    {
                    }
                }
            }

            if (!string.IsNullOrEmpty(_content))
                throw new Exception(string.Format("({0}) not supported", contentType));
        }

        private string ContentType()
        {
            return HeaderValue("Content-Type");
        }

        private string HeaderValue(string key)
        {
            return _headers.Where(x => x.Key.Equals(key, StringComparison.InvariantCultureIgnoreCase))
                    .Select(x => string.Join(", ", x.Value))
                    .DefaultIfEmpty(string.Empty)
                    .FirstOrDefault();
        }

        private void ParseLoad()
        {
            if (_loadResponses.Any())
            {
                _loadValues.Add(LoadValueTypes.TotalCall.Value, _loadResponses.Count);
                _loadValues.Add(LoadValueTypes.TotalSucceeded.Value, _loadResponses.Count(x => x.StatusCode == (int)HttpStatusCode.OK));
                _loadValues.Add(LoadValueTypes.TotalLost.Value, _loadResponses.Count(x => x.StatusCode == -1));
                _loadValues.Add(LoadValueTypes.AverageTTLMs.Value, new TimeSpan((long)_loadResponses.Where(x => x.StatusCode == (int)HttpStatusCode.OK).Average(x => x.Ticks)).TotalMilliseconds);
                _loadValues.Add(LoadValueTypes.MaximumTTLMs.Value, new TimeSpan(_loadResponses.Where(x => x.StatusCode == (int)HttpStatusCode.OK).Max(x => x.Ticks)).TotalMilliseconds);
                _loadValues.Add(LoadValueTypes.MinimumTTLMs.Value, new TimeSpan(_loadResponses.Where(x => x.StatusCode == (int)HttpStatusCode.OK).Min(x => x.Ticks)).TotalMilliseconds);
            }
        }

        public ResponseContext TestBody(string ruleName, Func<dynamic, bool> func)
        {
            return TestWrapper(ruleName, () => func.Invoke(_parsedContent));
        }

        private ResponseContext TestWrapper(string ruleName, Func<bool> func)
        {
            if (_assertions.ContainsKey(ruleName))
                throw new ArgumentException(string.Format("Rule for ({0}) already exist", ruleName));
            var result = false;

            try
            {
                result = func.Invoke();
            }
            catch
            { }

            _assertions.Add(ruleName, result);

            return this;
        }

        public ResponseContext Assert(string ruleName)
        {
            if (!_assertions.ContainsKey(ruleName)) return this;

            if (!_assertions[ruleName])
                throw new AssertException($"({ruleName}) Test Failed");
            // in order to allow multiple asserts
            return this;
        }

        public ResponseContext Debug()
        {
            "status code".WriteHeader();
            ((int)_statusCode).ToString().WriteLine();

            "response headers".WriteHeader();
            foreach (var header in _headers)
            {
                "{0} : {1}".WriteLine(header.Key, string.Join(", ", header.Value));
            }

            "content".WriteHeader();
            "{0}\n".Write(_content);

            "assertions".WriteHeader();
            foreach (var assertion in _assertions)
            {
                "{0} : {1}".WriteLine(assertion.Key, assertion.Value);
            }

            "schema errors".WriteHeader();
            foreach (var schemaError in _schemaErrors)
            {
                schemaError.WriteLine();
            }

            if (_loadResponses.Any())
            {
                "load test result".WriteHeader();
                LoadValueTypes.GetAll().ForEach(x => "{0} {1}".WriteLine(_loadValues[x.Value], x.DisplayName.ToLower()));
            }

            return this;
        }
    }
}
