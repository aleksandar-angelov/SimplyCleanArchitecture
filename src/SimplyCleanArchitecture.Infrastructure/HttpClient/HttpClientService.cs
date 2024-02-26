using Newtonsoft.Json;
using SimplyCleanArchitecture.Application.Common.Abstractions;
using SimplyCleanArchitecture.Application.Common.Models.HttpClient;
using System.Net.Http;
using System.Text;

namespace SimplyCleanArchitecture.Infrastructure.HttpClient;

public class HttpClientService : IHttpClient
{
    private readonly IHttpClientFactory _clientFactory;
    public HttpClientService(IHttpClientFactory clientFactory)
    {
        _clientFactory = clientFactory;
    }
    public async Task<TOut> SendAsync<TOut, TIn>(string client, string route, string method, List<Header> headers = null, TIn payload = null)
        where TIn : class
    {
        var httpClient = _clientFactory.CreateClient(client);
        using var request = new HttpRequestMessage(new HttpMethod(method), new Uri($"{httpClient.BaseAddress}{route}"));

        if (payload is not null)
        {
            var payloadJson = JsonConvert.SerializeObject(
                payload,
                new JsonSerializerSettings
                {
                    DefaultValueHandling = DefaultValueHandling.Ignore
                });

            request.Content = new StringContent(
                payloadJson,
                new UTF8Encoding(),
                "application/json");
        }

        if (headers is not null)
        {
            foreach (var header in headers)
            {
                request.Headers.Add(header.Key, header.Value);
            }
        }

        using var httpResponseMessage = await httpClient.SendAsync(request);
        httpResponseMessage.EnsureSuccessStatusCode();

        var responseJson = await httpResponseMessage.Content
            .ReadAsStringAsync();

        return JsonConvert.DeserializeObject<TOut>(responseJson);
    }

    public async Task<TOut> SendAsync<TOut>(string client, string route, string method, List<Header> headers = null)

    {
        var httpClient = _clientFactory.CreateClient(client);
        using var request = new HttpRequestMessage(new HttpMethod(method), new Uri($"{httpClient.BaseAddress}{route}"));

        if (headers is not null)
        {
            foreach (var header in headers)
            {
                request.Headers.Add(header.Key, header.Value);
            }
        }

        using var httpResponseMessage = await httpClient.SendAsync(request);
        httpResponseMessage.EnsureSuccessStatusCode();

        var responseJson = await httpResponseMessage.Content
            .ReadAsStringAsync();

        return JsonConvert.DeserializeObject<TOut>(responseJson);
    }

    public async Task SendAsync<TIn>(string client, string route, string method, List<Header> headers = null, TIn payload = null)
        where TIn : class
    {
        var httpClient = _clientFactory.CreateClient(client);
        using var request = new HttpRequestMessage(new HttpMethod(method), new Uri($"{httpClient.BaseAddress}{route}"));

        if (payload is not null)
        {
            var payloadJson = JsonConvert.SerializeObject(
                payload);
            request.Content = new StringContent(
                payloadJson,
                new UTF8Encoding(),
                "application/json");
        }

        if (headers is not null)
        {
            foreach (var header in headers)
            {
                request.Headers.Add(header.Key, header.Value);
            }
        }

        using var httpResponseMessage = await httpClient.SendAsync(request);
        httpResponseMessage.EnsureSuccessStatusCode();

        var responseJson = await httpResponseMessage.Content.ReadAsStringAsync();
    }
}
