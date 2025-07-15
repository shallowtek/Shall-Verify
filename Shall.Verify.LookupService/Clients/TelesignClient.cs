using Shall.Verify.Common.Helpers;
using Shall.Verify.LookupService.Configurations;
using Shall.Verify.LookupService.Entities.Phone;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Http.Headers;
using System.Text.Json;

namespace Shall.Verify.LookupService.Clients;

public class TelesignClient
{
    private HttpClient _client { get; }
    private readonly JsonSerializerOptionsWrapper _jsonSerializerOptionsWrapper;
    private readonly ILogger<TelesignClient> _logger;
    private readonly TelesignServiceOptions _options;

    public TelesignClient(HttpClient client,
        JsonSerializerOptionsWrapper jsonSerializerOptionsWrapper,
        ILogger<TelesignClient> logger,
        IOptions<TelesignServiceOptions> options)
    {
        _options = options.Value;
        _client = client;
        _client.BaseAddress = new Uri(_options.BaseUrl);
        _client.Timeout = new TimeSpan(0, 0, _options.Timeout);
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(_options.Token);
        _jsonSerializerOptionsWrapper = jsonSerializerOptionsWrapper;
        _logger = logger;
    }

    public async Task<TelesignDetectApiResponse> GetTelesignDetectApiResponseAsync(string phone, CancellationToken cancellationToken)
    {
        var request = new HttpRequestMessage(
           HttpMethod.Get,
           $"v1/score/{phone}");

        request.Headers.Accept.Add(
            new MediaTypeWithQualityHeaderValue("application/json"));
        request.Headers.AcceptEncoding.Add(
            new StringWithQualityHeaderValue("gzip"));

        try
        {
            using (var response = await _client.SendAsync(request,
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
                        _logger.LogInformation($"Unauthorized calling AtData fraud prevention. Email: {phone}");
                        return null;
                    }
                    else if (response.StatusCode == HttpStatusCode.BadRequest)
                    {
                        _logger.LogInformation($"BadRequest calling AtData fraud prevention. Email: {phone}");
                        return null;
                    }

                    response.EnsureSuccessStatusCode();
                }

                var stream = await response.Content.ReadAsStreamAsync();
                var telesignDetectApiResponse = await JsonSerializer.DeserializeAsync
                <TelesignDetectApiResponse>(stream, _jsonSerializerOptionsWrapper.Options);

                return telesignDetectApiResponse;
            }
        }
        catch (OperationCanceledException ocException)
        {
            _logger.LogError($"An operation was cancelled with message {ocException.Message}");
        }

        return null;
    }
}
