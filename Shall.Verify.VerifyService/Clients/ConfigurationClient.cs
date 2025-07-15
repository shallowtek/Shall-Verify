using Shall.Verify.Common.Entities.Configuration;
using Shall.Verify.Common.Helpers;
using Shall.Verify.VerifyService.Configurations;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Http.Headers;
using System.Text.Json;

namespace Shall.Verify.VerifyService.Clients;

public class ConfigurationClient
{
    private HttpClient _client { get; }
    private readonly JsonSerializerOptionsWrapper _jsonSerializerOptionsWrapper;
    private readonly ILogger<ConfigurationClient> _logger;
    private readonly ConfigurationServiceOptions _options;

    public ConfigurationClient(HttpClient client,
        JsonSerializerOptionsWrapper jsonSerializerOptionsWrapper,
        ILogger<ConfigurationClient> logger,
        IOptions<ConfigurationServiceOptions> options)
    {
        _options = options.Value;
        _client = client;
        _client.BaseAddress = new Uri(_options.BaseUrl);
        _client.Timeout = new TimeSpan(0, 0, _options.Timeout);
        _jsonSerializerOptionsWrapper = jsonSerializerOptionsWrapper;
        _logger = logger;
    }

    public async Task<RuleConfiguration> GetRuleConfigurationAsync(Guid siteId, CancellationToken cancellationToken)
    {
        var request = new HttpRequestMessage(
        HttpMethod.Get,
           $"/ruleconfiguration/{siteId}");

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
                        return null;
                    }
                    else if (response.StatusCode == HttpStatusCode.Unauthorized)
                    {
                        return null;
                    }

                    response.EnsureSuccessStatusCode();
                }

                var stream = await response.Content.ReadAsStreamAsync();
                var ruleConfiguration = await JsonSerializer.DeserializeAsync
                <RuleConfiguration>(stream, _jsonSerializerOptionsWrapper.Options);

                return ruleConfiguration;
            }
        }
        catch (OperationCanceledException ocException)
        {
            _logger.LogError($"An operation was cancelled with message {ocException.Message}");
        }

        return null;
    }

    public async Task<FeatureConfiguration> GetFeatureConfigurationAsync(Guid clientId, CancellationToken cancellationToken)
    {
        var request = new HttpRequestMessage(
        HttpMethod.Get,
           $"/featureconfiguration/{clientId}");

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
                        return null;
                    }
                    else if (response.StatusCode == HttpStatusCode.Unauthorized)
                    {
                        return null;
                    }

                    response.EnsureSuccessStatusCode();
                }

                var stream = await response.Content.ReadAsStreamAsync();
                var featureConfiguration = await JsonSerializer.DeserializeAsync
                <FeatureConfiguration>(stream, _jsonSerializerOptionsWrapper.Options);

                return featureConfiguration;
            }
        }
        catch (OperationCanceledException ocException)
        {
            _logger.LogError($"An operation was cancelled with message {ocException.Message}");
        }

        return null;
    }
}
