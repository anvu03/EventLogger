using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Web;
using Newtonsoft.Json.Linq;

namespace EventLogger.OneLogin
{
    public class Client
    {
        private const string ClientSecret = "7617edb630a5c52806b6f589760c48b509219f5a24d8971a23a026162dd18f75";
        private const string ClientId = "f9eb1e2f851baa7d3ce785ce84d66ae0bc58e9d8ee8865a02499dbcd247ceb5f";

        private const string BaseUri = "https://api.us.onelogin.com";

        private static string GetAccessToken()
        {
            const string oauthUri = "https://api.us.onelogin.com/auth/oauth2/token";
            const string payload = "{\n\"grant_type\":\"client_credentials\"\n}";
            byte[] dataBytes = Encoding.UTF8.GetBytes(payload);
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(BaseUri + "/auth/oauth2/token");
            request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
            request.ContentLength = dataBytes.Length;
            request.ContentType = "application/json";
            request.Method = "POST";
            request.Headers.Add("Authorization", $"client_id: {ClientId}, client_secret: {ClientSecret}");

            using (Stream requestBody = request.GetRequestStream())
            {
                requestBody.Write(dataBytes, 0, dataBytes.Length);
            }

            string postResponse;

            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            using (Stream stream = response.GetResponseStream())
            using (StreamReader reader = new StreamReader(stream))
            {
                postResponse = reader.ReadToEnd();
            }

            JObject oauthResponse = JObject.Parse(postResponse);
            string accessToken = (string)oauthResponse["data"][0]["access_token"];

            return accessToken;
        }

        public static JToken GetEventTypes()
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://api.us.onelogin.com");
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", GetAccessToken());
                HttpResponseMessage repsonse = client.GetAsync("/api/1/events/types").Result;
                string result = repsonse.Content.ReadAsStringAsync().Result;

                return JObject.Parse(result)["data"];
            }
        }



        public static JToken GetEvents()
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://api.us.onelogin.com");
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", GetAccessToken());
                HttpResponseMessage repsonse = client.GetAsync("/api/1/events?event_type_id=&client_id=&directory_id=&created_at=&id=&resolution=&since=&until=&user_id=").Result;
                string result = repsonse.Content.ReadAsStringAsync().Result;

                return JObject.Parse(result)["data"];
            }
        }
    }

    public struct EventType
    {
        public int Id{ get; set; }
        public string Description { get; set; }
        public string Name { get; set; }
    }

    public struct Event
    {
        
    }

    public struct App
    {

    }
}