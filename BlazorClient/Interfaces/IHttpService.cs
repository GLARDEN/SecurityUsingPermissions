
using Security.Core.Models.Administration.RoleManagement;
using Security.Core.Models;

namespace BlazorClient.Interfaces;

public interface IHttpService
{

    Task<ApiResponse<T>> HttpDeleteAsync<T>(string uri) where T : class;
    Task<ApiResponse<T>> HttpDeleteAsync<T>(string uri, object id) where T : class;
   // Task<string> HttpGetAsync(string uri);
    Task<ApiResponse<T>> HttpGetAsync<T>(string uri) where T : class;
    Task<ApiResponse<T>> HttpPostAsync<T>(string uri, object dataToSend) where T : class;
    Task<ApiResponse<T>> HttpPutAsync<T>(string uri, object dataToSend) where T : class;
   
}