using Shall.Verify.Common.Dtos.Lookup;
using Shall.Verify.Common.Helpers;
using Shall.Verify.VerifyService.Configurations;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.IO.Compression;
using System.Net;
using System.Net.Http.Headers;
using System.Text.Json;

namespace Shall.Verify.VerifyService.Clients;

public class LookupClient
{
    private HttpClient _client { get; }
    private readonly JsonSerializerOptionsWrapper _jsonSerializerOptionsWrapper;
    private readonly ILogger<LookupClient> _logger;
    private readonly LookupServiceOptions _options;

    public LookupClient(HttpClient client,
        JsonSerializerOptionsWrapper jsonSerializerOptionsWrapper,
        ILogger<LookupClient> logger,
        IOptions<LookupServiceOptions> options)
    {
        _options = options.Value;
        _client = client;
        _client.BaseAddress = new Uri(_options.BaseUrl);
        _client.Timeout = new TimeSpan(0, 0, _options.Timeout);
        _jsonSerializerOptionsWrapper = jsonSerializerOptionsWrapper;
        _logger = logger;
    }

    public async Task<LookupResponse> GetLookupResponseAsync(LookupRequest lookupRequest, CancellationToken cancellationToken)
    {
        using (var memoryContentStream = new MemoryStream())
        {
            await JsonSerializer.SerializeAsync(
                memoryContentStream,
                lookupRequest);

            memoryContentStream.Seek(0, SeekOrigin.Begin);

            using (var request = new HttpRequestMessage(
                HttpMethod.Post,
                "/lookup"))
            {
                request.Headers.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
                request.Headers.AcceptEncoding.Add(
                new StringWithQualityHeaderValue("gzip"));

                using (var compressedMemoryContentStream = new MemoryStream())
                {
                    using (var gzipStream = new GZipStream(
                      compressedMemoryContentStream,
                      CompressionMode.Compress))
                    {
                        memoryContentStream.CopyTo(gzipStream);
                        gzipStream.Flush();
                        compressedMemoryContentStream.Position = 0;

                        using (var streamContent = new StreamContent(compressedMemoryContentStream))

                        {
                            streamContent.Headers.ContentType =
                               new MediaTypeHeaderValue("application/json");
                            streamContent.Headers.ContentEncoding.Add("gzip");

                            request.Content = streamContent;

                            try
                            {
                                var response = await _client.SendAsync(request,
                                    HttpCompletionOption.ResponseHeadersRead);

                                if (!response.IsSuccessStatusCode)
                                {
                                    if (response.StatusCode == HttpStatusCode.NotFound)
                                    {
                                        _logger.LogInformation($"Not Found calling LookupService.");
                                        return null;
                                    }
                                    else if (response.StatusCode == HttpStatusCode.BadRequest)
                                    {
                                        _logger.LogInformation($"Bad Request calling LookupService.");
                                        return null;
                                    }
                                    else if (response.StatusCode == HttpStatusCode.Unauthorized)
                                    {
                                        _logger.LogInformation($"Unauthorized calling LookupService.");
                                        return null;
                                    }

                                    response.EnsureSuccessStatusCode();
                                }

                                var stream = await response.Content.ReadAsStreamAsync();

                                var lookupResponse = await JsonSerializer.DeserializeAsync
                            <LookupResponse>(stream, _jsonSerializerOptionsWrapper.Options);


                                return lookupResponse;
                            }

                            catch (OperationCanceledException ocException)
                            {
                                _logger.LogError($"An operation was cancelled with message {ocException.Message}");
                            }
                            finally
                            {
                                gzipStream.Dispose();
                            }

                            return null;
                        }
                    }
                }
            }
        }
    }
}