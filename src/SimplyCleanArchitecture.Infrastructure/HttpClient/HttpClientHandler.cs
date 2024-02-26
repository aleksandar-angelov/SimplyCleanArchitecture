using Microsoft.Extensions.Logging;

namespace Aspekt.KICBIntegrations.Infrastructure.HttpClient;

public class SendAsyncHandler : DelegatingHandler
{
    private readonly ILogger<SendAsyncHandler> _logger;

    public SendAsyncHandler(ILogger<SendAsyncHandler> logger)
    {
        _logger = logger;
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"HTTP Request SendAsync {request.Method} {request.RequestUri} Headers: {request.Headers} Content: {((request.Content is null) ? "No Content" : await request.Content.ReadAsStringAsync(cancellationToken))}");
        var response = await base.SendAsync(request, cancellationToken);
        _logger.LogInformation($"HTTP Response SendAsync Content: {((request.Content is null) ? "No Content" : await response.Content.ReadAsStringAsync(cancellationToken))}");
        return response;
    }
}