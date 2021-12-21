using BirokratNext.api_clientv2;
using BirokratNext.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BirokratNext
{
    public class ApiClientV2 : IDisposable, IApiClientV2
    {

        public DocumentCalls document;
        public virtual HttpClient HttpClient { get; private set; }
        private string apiKey;
        public virtual string ApiKey {
            get {
                return apiKey;
            }
            set {
                apiKey = value;
                HttpClient.DefaultRequestHeaders.Add("X-API-KEY", apiKey);
            }
        }

        public ApiClientV2(string apiAddress, string apiKey) {
            HttpClient = new HttpClient { BaseAddress = new Uri(apiAddress) };
            HttpClient.Timeout = new TimeSpan(1, 0, 0);
            ApiKey = apiKey;
            document = new DocumentCalls(HttpClient);
        }

        public Task Start() {
            return Task.CompletedTask;
        }

        public async Task<object> Logout() {
            try {
                HttpResponseMessage response = await HttpClient.GetAsync("v2/restart");
                return "ok";
            } catch (Exception ex) {
                Console.WriteLine(ex.ToString());
                return "fail";
            }
        }

        public async Task<object> Test() {
            var tmp = new Dictionary<string, object>();
            tmp["someKey"] = "someValue";
            var content = Serializer.ToJson(tmp);

            var response = await HttpClient.PostAsync(@"v2\poslovanje\racuni\kumulativnipregled\parameters",
                new StringContent(content, Encoding.UTF8, "application/json"));
            return await response.Content.ReadAsStringAsync();
        }

        public void Dispose() {
            HttpClient.Dispose();
            HttpClient = null;
        }
    }
}
