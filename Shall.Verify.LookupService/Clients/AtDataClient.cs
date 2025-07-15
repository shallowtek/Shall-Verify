using Shall.Verify.Common.Helpers;
using Shall.Verify.LookupService.Configurations;
using Shall.Verify.LookupService.Entities.Email;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Http.Headers;
using System.Text.Json;

namespace Shall.Verify.LookupService.Clients;

public class AtDataClient
{
    private HttpClient _client { get; }
    private readonly JsonSerializerOptionsWrapper _jsonSerializerOptionsWrapper;
    private readonly ILogger<AtDataClient> _logger;
    private readonly AtDataServiceOptions _options;

    public AtDataClient(HttpClient client,
        JsonSerializerOptionsWrapper jsonSerializerOptionsWrapper,
        ILogger<AtDataClient> logger,
        IOptions<AtDataServiceOptions> options)
    {
        _options = options.Value;
        _client = client;
        _client.BaseAddress = new Uri(_options.BaseUrl);
        _client.Timeout = new TimeSpan(0, 0, _options.Timeout);
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(_options.Token);
        _jsonSerializerOptionsWrapper = jsonSerializerOptionsWrapper;
        _logger = logger;
    }

    public async Task<AtDataFraudPreventionResponse> GetAtDataFraudPreventionResponseAsync(
        string emailAddress,
        CancellationToken cancellationToken)
    {
        var request = new HttpRequestMessage(
           HttpMethod.Get,
           $"fr?email={emailAddress}");

        request.Headers.Accept.Add(
            new MediaTypeWithQualityHeaderValue("application/json"));
        request.Headers.AcceptEncoding.Add(
            new StringWithQualityHeaderValue("gzip"));

        try
        {
            using (var response = await _client.SendAsync(
                request,
                HttpCompletionOption.ResponseHeadersRead,
                cancellationToken))
            {
                if (!response.IsSuccessStatusCode)
                {

                    if (response.StatusCode == HttpStatusCode.NotFound)
                    {
                        _logger.LogError($"Not Found calling AtData fraud prevention.");
                        return null;
                    }
                    else if (response.StatusCode == HttpStatusCode.Unauthorized)
                    {
                        _logger.LogInformation($"Unauthorized calling AtData fraud prevention. Email: {emailAddress}");
                        return null;
                    }
                    else if (response.StatusCode == HttpStatusCode.BadRequest)
                    {
                        _logger.LogInformation($"BadRequest calling AtData fraud prevention. Email: {emailAddress}");
                        return null;
                    }

                    response.EnsureSuccessStatusCode();
                }

                var stream = await response.Content.ReadAsStreamAsync();
                var atDataFraudPreventionResult = await JsonSerializer.DeserializeAsync
                <AtDataFraudPreventionResponse>(stream, _jsonSerializerOptionsWrapper.Options);

                return atDataFraudPreventionResult;
            }
        }
        catch (OperationCanceledException ocException)
        {
            _logger.LogError($"An operation was cancelled with message {ocException.Message}");
        }

        return null;
    }
}