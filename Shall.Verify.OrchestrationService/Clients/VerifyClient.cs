using Shall.Verify.Common.Helpers;
using Shall.Verify.OrchestrationService.Configurations;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.IO.Compression;
using System.Net;
using System.Net.Http.Headers;
using System.Text.Json;
using Shall.Verify.Common.Dtos.Verify;

namespace Shall.Verify.OrchestrationService.Clients;

public class VerifyClient
{
    private HttpClient _client { get; }
    private readonly JsonSerializerOptionsWrapper _jsonSerializerOptionsWrapper;
    private readonly ILogger<VerifyClient> _logger;
    private readonly VerifyServiceOptions _options;

    public VerifyClient(HttpClient client,
        JsonSerializerOptionsWrapper jsonSerializerOptionsWrapper,
        ILogger<VerifyClient> logger,
        IOptions<VerifyServiceOptions> options)
    {
        _options = options.Value;
        _client = client;
        _client.BaseAddress = new Uri(_options.BaseUrl);
        _client.Timeout = new TimeSpan(0, 0, _options.Timeout);
        _jsonSerializerOptionsWrapper = jsonSerializerOptionsWrapper;
        _logger = logger;
    }

    public async Task<VerifyResponse> VerifyAsync(VerifyRequest verifyRequest, CancellationToken cancellationToken)
    {
        using (var memoryContentStream = new MemoryStream())
        {
            await JsonSerializer.SerializeAsync(
                memoryContentStream,
                verifyRequest);

            memoryContentStream.Seek(0, SeekOrigin.Begin);

            using (var request = new HttpRequestMessage(
                HttpMethod.Post,
                "/verify"))
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

                                    if (response.StatusCode == HttpStatusCode.NotFound ||
                                       response.StatusCode == HttpStatusCode.BadRequest ||
                                       response.StatusCode == HttpStatusCode.Unauthorized)
                                    {
                                        _logger.LogError($"Error calling VerifyService. SiteId: {verifyRequest.SiteId} HttpStatus: {response.StatusCode}");
                                        return null;
                                    }

                                    response.EnsureSuccessStatusCode();
                                }

                                var stream = await response.Content.ReadAsStreamAsync();

                                var verifyResponse = await JsonSerializer.DeserializeAsync
                            <VerifyResponse>(stream, _jsonSerializerOptionsWrapper.Options);


                                return verifyResponse;
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