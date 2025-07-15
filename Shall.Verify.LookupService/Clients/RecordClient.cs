using Shall.Verify.Common.Helpers;
using Shall.Verify.LookupService.Configurations;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.IO.Compression;
using System.Net;
using System.Net.Http.Headers;
using System.Text.Json;
using Shall.Verify.Common.Dtos.Record;

namespace Shall.Verify.LookupService.Clients;

public class RecordClient
{
    private HttpClient _client { get; }
    private readonly JsonSerializerOptionsWrapper _jsonSerializerOptionsWrapper;
    private readonly ILogger<RecordClient> _logger;
    private readonly RecordServiceOptions _options;

    public RecordClient(HttpClient client,
        JsonSerializerOptionsWrapper jsonSerializerOptionsWrapper,
        ILogger<RecordClient> logger,
        IOptions<RecordServiceOptions> options)
    {
        _options = options.Value;
        _client = client;
        _client.BaseAddress = new Uri(_options.BaseUrl);
        _client.Timeout = new TimeSpan(0, 0, _options.Timeout);
        _jsonSerializerOptionsWrapper = jsonSerializerOptionsWrapper;
        _logger = logger;
    }

    public async Task<RecordCountResponse> GetRecordCountAsync(RecordCountRequest recordCountRequest)
    {
        using (var memoryContentStream = new MemoryStream())
        {
            await JsonSerializer.SerializeAsync(
                memoryContentStream,
                recordCountRequest);

            memoryContentStream.Seek(0, SeekOrigin.Begin);

            using (var request = new HttpRequestMessage(
                HttpMethod.Post,
                "/record/count"))
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
                                        _logger.LogError($"Bad Request calling Record Service.");
                                        return null;
                                    }
                                    else if (response.StatusCode == HttpStatusCode.Unauthorized)
                                    {
                                        return null;
                                    }
                                    else if (response.StatusCode == HttpStatusCode.BadRequest)
                                    {
                                        return null;
                                    }

                                    response.EnsureSuccessStatusCode();
                                }

                                var stream = await response.Content.ReadAsStreamAsync();

                                var recordCountResponse = await JsonSerializer.DeserializeAsync
                            <RecordCountResponse>(stream, _jsonSerializerOptionsWrapper.Options);


                                return recordCountResponse;
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

    public async Task<RecordMatchResponse> GetRecordMatchAsync(RecordMatchRequest recordMatchRequest)
    {
        using (var memoryContentStream = new MemoryStream())
        {
            await JsonSerializer.SerializeAsync(
                memoryContentStream,
                recordMatchRequest);

            memoryContentStream.Seek(0, SeekOrigin.Begin);

            using (var request = new HttpRequestMessage(
                HttpMethod.Post,
                "/record/match"))
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
                                        _logger.LogError($"Bad Request calling Record Service.");
                                        return null;
                                    }
                                    else if (response.StatusCode == HttpStatusCode.Unauthorized)
                                    {
                                        return null;
                                    }
                                    else if (response.StatusCode == HttpStatusCode.BadRequest)
                                    {
                                        return null;
                                    }

                                    response.EnsureSuccessStatusCode();
                                }

                                var stream = await response.Content.ReadAsStreamAsync();

                                var recordMatchResponse = await JsonSerializer.DeserializeAsync
                            <RecordMatchResponse>(stream, _jsonSerializerOptionsWrapper.Options);

                                return recordMatchResponse;
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