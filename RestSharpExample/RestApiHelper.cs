using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using RestSharp;

namespace RestSharpExample
{
    public class RestApiHelper<T>
    {
        public RestClient _restClient;
        public RestRequest _restRequest;
        public string _baseUrl = "https://reqres.in/";

        public RestClient SetUrl(string resourceUrl)
        {
            var url = Path.Combine(_baseUrl, resourceUrl);
            var _restClient = new RestClient(url);
            return _restClient;
        }

        public RestRequest CreatePostRequest(string jsonString)
        {
            _restRequest = new RestRequest(Method.POST);
            // agrega encambezados
            _restRequest.AddHeader("Accept", "application/json");
            _restRequest.AddParameter("application/json",jsonString,ParameterType.RequestBody);
            return _restRequest;
        }

        public RestRequest CreateGetRequest()
        {
            _restRequest = new RestRequest(Method.GET);
            // agrega encambezados
            _restRequest.AddHeader("Accept", "application/json");
            return _restRequest;
        }
        public RestRequest CreatePutRequest(string jsonString)
        {
            _restRequest = new RestRequest(Method.PUT);
            _restRequest.AddHeader("Accept", "application/json");
            _restRequest.AddParameter("application/json", jsonString, ParameterType.RequestBody);
            return _restRequest;
        }

        public RestRequest CreateDeleteRequest()
        {
            _restRequest = new RestRequest(Method.DELETE);
            _restRequest.AddHeader("Accept", "application/json");
            return _restRequest;
        }

        public IRestResponse GetResponse(RestClient restClient, RestRequest restRequest)
        {
            return restClient.Execute(restRequest);
        }

        public DTO GetContent<DTO>(IRestResponse response)
        {
            var content = response.Content;
            DTO deseiralizeObject = JsonConvert.DeserializeObject<DTO>(content);
            return deseiralizeObject;
        }
    }
}
