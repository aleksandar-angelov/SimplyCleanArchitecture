using SimplyCleanArchitecture.Application.Common.Models.HttpClient;

namespace SimplyCleanArchitecture.Application.Common.Abstractions;

public interface IHttpClient
{
    Task<TOut> SendAsync<TOut, TIn>(string client, string route, string method, List<Header> headers = null, TIn payload = null)
        where TIn : class;
    Task<TOut> SendAsync<TOut>(string client, string route, string method, List<Header> headers = null);
    Task SendAsync<TIn>(string client, string route, string method, List<Header> headers = null, TIn payload = null)
        where TIn : class;
}
