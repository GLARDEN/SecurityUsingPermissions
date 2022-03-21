
namespace BlazorClient.Services;

public interface IHttpService
{
    Task<T> HttpDeleteAsync<T>(string uri) where T : class;
    Task<T> HttpDeleteAsync<T>(string uri, object id) where T : class;
    Task<string> HttpGetAsync(string uri);
    Task<T> HttpGetAsync<T>(string uri) where T : class;
    Task<T> HttpPostAsync<T>(string uri, object dataToSend) where T : class;
    Task<T> HttpPutAsync<T>(string uri, object dataToSend) where T : class;
    void LogOut();
}