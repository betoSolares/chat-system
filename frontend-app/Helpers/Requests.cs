using frontend_app.Models;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace frontend_app.Helpers
{
    public class Requests<T>
    {
        /// <summary>Make a get request to the API</summary>
        /// <param name="token">The authorization token</param>
        /// <param name="uri">The uri for the request</param>
        /// <returns>The result of the request and the response read</returns>
        public (HttpResponseMessage result, Task<string>) Get(string token, string uri)
        {
            HttpClient client = new HttpClient
            {
                BaseAddress = new Client().URI
            };
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            Task<HttpResponseMessage> response = client.GetAsync(uri);
            response.Wait();
            client.Dispose();

            HttpResponseMessage result = response.Result;
            Task<string> readTask = result.Content.ReadAsStringAsync();
            readTask.Wait();
            return (result, readTask);
        }

        /// <summary>Make a post request to the API</summary>
        /// <param name="token">The authorization token</param>
        /// <param name="uri">The uri for the request</param>
        /// <param name="param">The data to send to the API</param>
        /// <returns>The result of the request and the response read</returns>
        public (HttpResponseMessage result, Task<string>) Post(string token, string uri, T param)
        {
            HttpClient client = new HttpClient
            {
                BaseAddress = new Client().URI
            };
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            Task<HttpResponseMessage> response = client.PostAsJsonAsync(uri, param);
            response.Wait();
            client.Dispose();

            HttpResponseMessage result = response.Result;
            Task<string> readTask = result.Content.ReadAsStringAsync();
            readTask.Wait();
            return (result, readTask);
        }

        /// <summary>Make a put request to the API</summary>
        /// <param name="token">The authorization token</param>
        /// <param name="uri">The uri for the request</param>
        /// <param name="param">The data to send to the API</param>
        /// <returns>The result of the request and the response read</returns>
        public (HttpResponseMessage result, Task<string>) Put(string token, string uri, T param)
        {
            HttpClient client = new HttpClient
            {
                BaseAddress = new Client().URI
            };
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            Task<HttpResponseMessage> response = client.PutAsJsonAsync(uri, param);
            response.Wait();
            client.Dispose();

            HttpResponseMessage result = response.Result;
            Task<string> readTask = result.Content.ReadAsStringAsync();
            readTask.Wait();
            return (result, readTask);
        }
    }
}