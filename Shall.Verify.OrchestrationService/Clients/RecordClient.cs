using Shall.Verify.Common.Dtos;
using Shall.Verify.Common.Helpers;
using Shall.Verify.OrchestrationService.Configurations;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.IO.Compression;
using System.Net;
using System.Net.Http.Headers;
using System.Text.Json;
using Shall.Verify.Common.Entities.Record;
using Shall.Verify.Common.Dtos.Record;

namespace Shall.Verify.OrchestrationService.Clients;

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

    public async Task<bool> CreateRecordAsync(RecordAttributes record, CancellationToken cancellationToken)
    {
        using (var memoryContentStream = new MemoryStream())
        {
            await JsonSerializer.SerializeAsync(
                memoryContentStream,
                record);

            memoryContentStream.Seek(0, SeekOrigin.Begin);

            using (var request = new HttpRequestMessage(
                HttpMethod.Post,
                "/record"))
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
                                        _logger.LogError($"Error calling Record Service. RecordId: {record.VerifyId} HttpStatus: {response.StatusCode}");
                                        return false;
                                    }

                                    response.EnsureSuccessStatusCode();
                                }
                            }

                            catch (OperationCanceledException ocException)
                            {
                                _logger.LogError($"An operation was cancelled with message {ocException.Message}");
                            }
                            finally
                            {
                                gzipStream.Dispose();
                            }

                            return false;
                        }
                    }
                }
            }
        }
    }

    public async Task<bool> UpdateRecordStatusAsync(RecordStatusRequest record, CancellationToken cancellationToken)
    {
        using (var memoryContentStream = new MemoryStream())
        {
            await JsonSerializer.SerializeAsync(
                memoryContentStream,
                record);

            memoryContentStream.Seek(0, SeekOrigin.Begin);

            using (var request = new HttpRequestMessage(
                HttpMethod.Post,
                "/record/status"))
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
                                        _logger.LogError($"Error calling Record Service. VerifyId: {record.VerifyId} HttpStatus: {response.StatusCode}");
                                        return false;
                                    }

                                    response.EnsureSuccessStatusCode();
                                }
                            }

                            catch (OperationCanceledException ocException)
                            {
                                _logger.LogError($"An operation was cancelled with message {ocException.Message}");
                            }
                            finally
                            {
                                gzipStream.Dispose();
                            }

                            return false;
                        }
                    }
                }
            }
        }
    }

    public async Task<bool> BatchUpdateRecordStatusAsync(BatchRecordStatusRequest batchRecordStatus, CancellationToken cancellationToken)
    {
        using (var memoryContentStream = new MemoryStream())
        {
            await JsonSerializer.SerializeAsync(
                memoryContentStream,
                batchRecordStatus);

            memoryContentStream.Seek(0, SeekOrigin.Begin);

            using (var request = new HttpRequestMessage(
                HttpMethod.Post,
                "/record/status/batch"))
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
                                        _logger.LogError($"Error calling Record Service. HttpStatus: {response.StatusCode}");
                                        return false;
                                    }

                                    response.EnsureSuccessStatusCode();
                                }
                            }

                            catch (OperationCanceledException ocException)
                            {
                                _logger.LogError($"An operation was cancelled with message {ocException.Message}");
                            }
                            finally
                            {
                                gzipStream.Dispose();
                            }

                            return false;
                        }
                    }
                }
            }
        }
    }
}