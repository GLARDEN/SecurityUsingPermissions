
using System.Net;

namespace Security.Core.Models;

public class ApiResponse<T> where T : class
{
    public T? Data { get; set; }
    public HttpStatusCode StatusCode { get; set; }
    public List<string>? ResponseMessages { get; set; }

    public ApiResponse(HttpStatusCode statusCode, List<string>? responseMessages = null, T? data = null)
    {
        Data = data;
        StatusCode = statusCode;
        ResponseMessages = responseMessages ?? new List<string>();
    }
}


//if (!result.IsSuccessStatusCode)
//   {
//       var test = result.Content.ReadAsStringAsync().GetAwaiter().GetResult();

//   }