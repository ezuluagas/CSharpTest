using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RestSharp;

namespace RestSharpExample
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            //Creating Client connection	
            RestClient restClient = new RestClient("http://restapi.demoqa.com/utilities/weather/city/");

            //Creating request to get data from server
            RestRequest restRequest = new RestRequest("Guntur", Method.GET);

            // Executing request to server and checking server response to the it
            IRestResponse restResponse = restClient.Execute(restRequest);

            // Extracting output data from received response
            string response = restResponse.Content;

            // Verifiying reponse
            if (!response.Contains("Guntur"))
                   Assert.Fail("Whether information is not displayed");
        }
    
    }
}
