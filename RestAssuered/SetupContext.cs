using System;
using System.Collections.Generic;
using System.Text;

namespace RestAssuered
{
    public class SetupContext
    {
        private string _name;

        private readonly Dictionary<string, string> _cookies = new Dictionary<string, string>();

        public SetupContext Name(string name)
        {
            _name = name;
            return this;
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


    }
}
