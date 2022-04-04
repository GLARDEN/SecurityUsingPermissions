

using Blazored.LocalStorage;

using Microsoft.AspNetCore.Components;

using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace BlazorClient.Services;

public class HttpService :IHttpService
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly NavigationManager _navigationManager;
    private readonly ILocalStorageService _localStorage;
    private readonly HttpClient _httpClient;
    private readonly string _apiBaseUrl;

    public HttpService(IHttpClientFactory httpClientFactory, NavigationManager navigationManager, ILocalStorageService localStorage)
    {
        _httpClient = httpClientFactory.CreateClient("WebAPI");
        _navigationManager = navigationManager;
        _localStorage = localStorage;
        _apiBaseUrl = _httpClient?.BaseAddress?.ToString() ?? "";
    }

    public async Task<T> HttpGetAsync<T>(string uri) where T : class
    {
        var test = $"{_apiBaseUrl}{uri}";

        var result = await _httpClient.GetAsync(test);

        if (!result.IsSuccessStatusCode)
        {
            return null;
        }
        return await FromHttpResponseMessageAsync<T>(result);
    }

    public async Task<string> HttpGetAsync(string uri)
    {
        var result = await _httpClient.GetAsync($"{_apiBaseUrl}{uri}");
        if (!result.IsSuccessStatusCode)
        {
            return null;
        }

        return await result.Content.ReadAsStringAsync();
    }

    public Task<T> HttpDeleteAsync<T>(string uri, object id) where T : class
    {
        return HttpDeleteAsync<T>($"{uri}/{id}");
    }

    public async Task<T> HttpDeleteAsync<T>(string uri)
        where T : class
    {
        var result = await _httpClient.DeleteAsync($"{_apiBaseUrl}{uri}");
        if (!result.IsSuccessStatusCode)
        {
            return null;
        }

        return await FromHttpResponseMessageAsync<T>(result);
    }

    public async Task<T> HttpPostAsync<T>(string uri, object dataToSend) where T : class
    {
        var content = ToJson(dataToSend);
        var apiURL = $"{_apiBaseUrl}{uri}";

        var result = await _httpClient.PostAsync(apiURL, content);
        if (!result.IsSuccessStatusCode)
        {
            return null;
        }

        return await FromHttpResponseMessageAsync<T>(result);
    }

    public async Task<T> HttpPutAsync<T>(string uri, object dataToSend)
        where T : class
    {
        var content = ToJson(dataToSend);
        var apiURL = $"{_apiBaseUrl}{uri}";

        var result = await _httpClient.PutAsync(apiURL, content);
        if (!result.IsSuccessStatusCode)
        {
            return null;
        }

        return await FromHttpResponseMessageAsync<T>(result);
    }

    public async void LogOut()
    {
        await _localStorage.RemoveItemAsync("authenticationToken");
        _httpClient.DefaultRequestHeaders.Authorization = null;
    }

    private StringContent ToJson(object obj)
    {
        try
        {
            var result = JsonSerializer.Serialize(obj);
            return new StringContent(result, Encoding.UTF8, "application/json");
        }
        catch (Exception ex)
        {
            var test = ex.Message;
        }
        return null;
    }

    private async Task<T> FromHttpResponseMessageAsync<T>(HttpResponseMessage result)
    {
        try
        {
            return JsonSerializer.Deserialize<T>(await result.Content.ReadAsStringAsync(), new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true

            });
        } 
        catch (Exception ex)
        {
            var test = ex;
        }

        return JsonSerializer.Deserialize<T>("", new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,

        });
    }
}

