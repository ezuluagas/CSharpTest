using Microsoft.VisualStudio.TestTools.UnitTesting;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestSharpExample
{
    [TestClass]
    public class TestExample
    {
        [TestMethod]
        public void GetWeatherInfo()
        {
            //RestClient restClient = new RestClient("http://restapi.demoqa.com/utilities/weather/city/");
            //(Or)
            const string baseURL = "http://restapi.demoqa.com/utilities/weather/city/";
            RestClient restClient = new RestClient();
            restClient.BaseUrl = new Uri(baseURL);

           RestRequest restRequest = new RestRequest("Guntur", Method.GET);

            IRestResponse restResponse = restClient.Execute(restRequest);
            string response = restClient.Execute(restRequest).Content;

            if (!response.Contains("Guntur")){
                Assert.Fail("Whether information is not displayed");
            }
        }

        
        [TestMethod]
        public void CreateUsers()
        {
            string jsonString = @"{
                                    ""name"": ""morpheus"",
                                    ""job"": ""leader""
                                }";

            RestApiHelper<CreateUser> restApi = new RestApiHelper<CreateUser>();
            var restUrl = restApi.SetUrl("api/users");
            var restRequest = restApi.CreatePostRequest(jsonString);
            var response = restApi.GetResponse(restUrl, restRequest);

            CreateUser content = restApi.GetContent<CreateUser>(response);
            Assert.AreEqual(response.StatusCode.ToString(), "Created");
            Assert.AreEqual(content.name, "morpheus");
            Assert.AreEqual(content.job, "leader");

        }

        [TestMethod]
        public void GetResponseUsers()
        {
            Demo demo = new Demo();
            var response = demo.GetUsers();
            Assert.AreEqual(2, response.page);

        }

        [TestMethod]
        public void AddNumbers()
        {
           
        }

    }
    
}
