using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace EventLogger.Controllers
{
    public class DataProcessingController : ApiController
    {
        public string Post(string uri, string data, string contentType, string method = "POST")
        {
            byte[] dataBytes = Encoding.UTF8.GetBytes(data);
            string clientId = "f9eb1e2f851baa7d3ce785ce84d66ae0bc58e9d8ee8865a02499dbcd247ceb5f";
            string clientSecret = "7617edb630a5c52806b6f589760c48b509219f5a24d8971a23a026162dd18f75";
            HttpWebRequest request = (HttpWebRequest) WebRequest.Create(uri);
            request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
            request.ContentLength = dataBytes.Length;
            request.ContentType = contentType;
            request.Method = method;
            request.Headers.Add("Authorization", $"client_id: {clientId}, client_secret: {clientSecret}");

            using (Stream requestBody = request.GetRequestStream())
            {
                requestBody.Write(dataBytes, 0, dataBytes.Length);
            }

            using (HttpWebResponse response = (HttpWebResponse) request.GetResponse())
            using (Stream stream = response.GetResponseStream())
            using (StreamReader reader = new StreamReader(stream))
            {
                return reader.ReadToEnd();
            }
        }

        private JObject GetEvent()
        {
            const string oauthUri = "https://api.us.onelogin.com/auth/oauth2/token";
            const string payload = "{\n\"grant_type\":\"client_credentials\"\n}";
            byte[] dataBytes = Encoding.UTF8.GetBytes(payload);
            string clientId = "f9eb1e2f851baa7d3ce785ce84d66ae0bc58e9d8ee8865a02499dbcd247ceb5f";
            string clientSecret = "7617edb630a5c52806b6f589760c48b509219f5a24d8971a23a026162dd18f75";
            HttpWebRequest request = (HttpWebRequest) WebRequest.Create(oauthUri);
            request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
            request.ContentLength = dataBytes.Length;
            request.ContentType = "application/json";
            request.Method = "POST";
            request.Headers.Add("Authorization", $"client_id: {clientId}, client_secret: {clientSecret}");

            using (Stream requestBody = request.GetRequestStream())
            {
                requestBody.Write(dataBytes, 0, dataBytes.Length);
            }

            string postResponse;

            using (HttpWebResponse response = (HttpWebResponse) request.GetResponse())
            using (Stream stream = response.GetResponseStream())
            using (StreamReader reader = new StreamReader(stream))
            {
                postResponse = reader.ReadToEnd();
            }

            JObject oauthResponse = JObject.Parse(postResponse);
            string accessToken = (string)oauthResponse["data"][0]["access_token"];

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://api.us.onelogin.com");
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                HttpResponseMessage repsonse = client.GetAsync("/api/1/events?event_type_id=&client_id=&directory_id=&created_at=&id=&resolution=&since=&until=&user_id=").Result;
                string result = repsonse.Content.ReadAsStringAsync().Result;

                return JObject.Parse(result);
            }
        }

        [HttpGet]
        public async Task<IHttpActionResult> Run()
        {
            var data = GetEvent()["data"];

            return Ok(OneLogin.Client.GetEvents());
        }
    }
}