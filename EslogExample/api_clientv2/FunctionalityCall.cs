
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BirokratNext.api_clientv2
{
    public class FunctionalityCall
    {
        public virtual HttpClient HttpClient { get; private set; }
        public FunctionalityCall(HttpClient client) {
            HttpClient = client;
        }

        public async Task<object> HttpDelete(string path) {
            try {
                HttpResponseMessage response = null;
                response = await HttpClient.DeleteAsync($"{path}");
                string cont = await response.Content.ReadAsStringAsync();
                cont = response.StatusCode == HttpStatusCode.OK ? "" : cont;
                Console.WriteLine(response.StatusCode + $" {path} {cont}");
                return await response.Content.ReadAsStringAsync();
            } catch (Exception ex) {
                Console.WriteLine(ex.ToString());
                throw ex;
            }
        }

        public async Task<object> HttpPost(string path, string content = "") {

            try {
                HttpResponseMessage response = null;
                do {
                    var some = new StringContent(content, Encoding.UTF8, "application/json");
                    response = await HttpClient.PostAsync($"{path}", some);
                    string cont = await response.Content.ReadAsStringAsync();
                    cont = response.StatusCode == HttpStatusCode.OK ? "" : cont;
                    Console.WriteLine(response.StatusCode + $" {path} {cont}");
                    if (response.StatusCode == HttpStatusCode.OK) break;
                    Thread.Sleep(3000);
                } while (response.StatusCode == HttpStatusCode.Accepted);
                if (response.StatusCode == HttpStatusCode.InternalServerError) {
                    Console.WriteLine();
                }
                return await response.Content.ReadAsStringAsync();
            } catch (Exception ex) {
                Console.WriteLine(ex.ToString());
                throw ex;
            }
        }

        public async Task<object> HttpGet(string path) {
            try {
                HttpResponseMessage response = null;
                do {
                    response = await HttpClient.GetAsync($"{path}");
                    string cont = await response.Content.ReadAsStringAsync();
                    cont = response.StatusCode == HttpStatusCode.OK ? "" : cont;
                    Console.WriteLine(response.StatusCode + $" {path} {cont}");
                    if (response.StatusCode == HttpStatusCode.OK) break;
                    Thread.Sleep(3000);
                } while (response.StatusCode == HttpStatusCode.Accepted);
                if (response.StatusCode == HttpStatusCode.InternalServerError) {
                    Console.WriteLine();
                }
                return await response.Content.ReadAsStringAsync();
            } catch (Exception ex) {
                Console.WriteLine(ex.ToString());
                throw ex;
            }
        }
    }
}
