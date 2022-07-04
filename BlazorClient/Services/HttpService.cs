
using Blazored.LocalStorage;
using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;

using BlazorClient.Interfaces;
using Security.Core.Models;
using Microsoft.AspNetCore.Http;

namespace BlazorClient.Services;



public class HttpService : IHttpService
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ILocalStorageService _localStorage;
    private readonly HttpClient _httpClient;
    private readonly string _apiBaseUrl;

    public HttpService(IHttpClientFactory httpClientFactory, ILocalStorageService localStorage) 
    {
        _httpClientFactory = httpClientFactory;
        _localStorage = localStorage;
    }

    public async Task<ApiResponse<T>> HttpGetAsync<T>(string uri) where T : class
    {
        HttpClient httpClient = _httpClientFactory?.CreateClient("api");
        var requestUrl = $"{httpClient?.BaseAddress?.ToString()}{uri}";

        var result = await httpClient.GetAsync(requestUrl);

        if (!result.IsSuccessStatusCode)
        {
            return null;
        }
        return await FromHttpResponseMessageAsync<T>(result);
    }

    public Task<ApiResponse<T>> HttpDeleteAsync<T>(string uri, object id) where T : class
    {
        return HttpDeleteAsync<T>($"{uri}/{id}");
    }

    public async Task<ApiResponse<T>> HttpDeleteAsync<T>(string uri) where T : class
    {
        HttpClient httpClient = _httpClientFactory?.CreateClient("api");
        var requestUrl = $"{httpClient?.BaseAddress?.ToString()}{uri}";

        var result = await httpClient.DeleteAsync(requestUrl);
        if (!result.IsSuccessStatusCode)
        {
            return null;
        }

        return await FromHttpResponseMessageAsync<T>(result);
    }

    public async Task<ApiResponse<T>> HttpPostAsync<T>(string uri, object dataToSend) where T : class
    {
        var content = ToJson(dataToSend);
        HttpClient httpClient = _httpClientFactory?.CreateClient("api");
        var requestUrl = $"{httpClient?.BaseAddress?.ToString()}{uri}";

        var result = await httpClient.PostAsync(requestUrl, content);

        if (result == null)
        {
            return null;
        }

        return await FromHttpResponseMessageAsync<T>(result);
    }

    public async Task<ApiResponse<T>> HttpPutAsync<T>(string uri, object dataToSend) where T : class
    {
        var content = ToJson(dataToSend);
        var apiURL = $"{_apiBaseUrl}{uri}";

        HttpClient httpClient = _httpClientFactory?.CreateClient("api");
        var requestUrl = $"{httpClient?.BaseAddress?.ToString()}{uri}";

        var result = await httpClient.PutAsync(apiURL, content);

        return await FromHttpResponseMessageAsync<T>(result);
    }

    private StringContent ToJson(object obj)
    {
        var result = JsonSerializer.Serialize(obj);
        return new StringContent(result, Encoding.UTF8, "application/json");
    }

    private async Task<ApiResponse<T>> FromHttpResponseMessageAsync<T>(HttpResponseMessage result) where T : class
    {
        ApiResponse<T> apiResponse;
        try
        {
            apiResponse = new ApiResponse<T>(result.StatusCode);

            if (result.StatusCode == HttpStatusCode.OK)
            {
                var jsonData = await result.Content.ReadFromJsonAsync<T>();
                apiResponse.Data = jsonData;
            }
            else if (result.StatusCode == HttpStatusCode.BadRequest)
            {
                //Convert validation messages or exceptions to response object for UI to display
                //Sample Structure Validation Result: { "example":["Sample Validation Message 1","Sample Validation Message 2"]}
                var jsonString = await result.Content.ReadAsStringAsync();
                Dictionary<string, List<string>>? requestResult = JsonSerializer.Deserialize<Dictionary<string, List<string>>>(jsonString);

                List<string>? messages = requestResult?.Values.SelectMany(v => v).ToList();
                apiResponse.ResponseMessages?.AddRange(messages ?? new());
            }

            return apiResponse;
        }
        catch (JsonException ex)
        {
            return new ApiResponse<T>(HttpStatusCode.InternalServerError, new List<string>() { ex.Message });
        }
    }
}


