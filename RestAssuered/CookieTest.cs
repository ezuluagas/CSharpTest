using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;


namespace RestAssuered
{ 
    [TestFixture]
    public class CookieTest
    {
        private KeyValuePair<string, string> _singleCookie = new KeyValuePair<string, string>("cookie1", "value1");
        private Dictionary<string, string> _multipleCookies = new Dictionary<string, string>();

        [Test]
        public void ShouldSetCookies()
        {
            new RestAssuered().Given().
                Name("Should set cookies").
                Cookie(_singleCookie.Key,_singleCookie.Value)
                .Cookies(_multipleCookies)
                .When()
                .Get("https://httpbin.org/cookies")
                .Then()
                .TestBody("Should contain cookies", resp => TestCookieResponse(resp))
                .Assert("Should contain cookies")
                .Debug();

        }

    }
}
