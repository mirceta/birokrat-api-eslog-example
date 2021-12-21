using System.Net.Http;
using System.Threading.Tasks;

namespace BirokratNext
{
    public interface IApiClientV2
    {
        string ApiKey { get; set; }
        HttpClient HttpClient { get; }

        void Dispose();
        Task<object> Logout();
        Task Start();
        Task<object> Test();
    }
}