using Shall.Verify.VerifyService.Clients;
using Shall.Verify.VerifyService.Configurations;
using Shall.Verify.VerifyService.Extensions;
using Shall.Verify.VerifyService.Helpers;
using Shall.Verify.VerifyService.Services;
using System.Net;
using Shall.Verify.Common.Helpers;
using Shall.Verify.Common.Dtos.Verify;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.AddKafkaProducer<string, VerifyResponse>("messaging", producerBuilder =>
{
    producerBuilder.SetValueSerializer(new VerifySerializer());
});

builder.Services.AddScoped<IVerifyService, VerifyService>();

builder.Services.AddSingleton<JsonSerializerOptionsWrapper>();

builder.Services.Configure<LookupServiceOptions>(
    builder.Configuration.GetSection(LookupServiceOptions.LookupService));

builder.Services.Configure<ConfigurationServiceOptions>(
    builder.Configuration.GetSection(ConfigurationServiceOptions.ConfigurationService));

builder.Services.AddHttpClient<LookupClient>()
    .ConfigurePrimaryHttpMessageHandler(() =>
    {
        var handler = new SocketsHttpHandler();
        handler.AutomaticDecompression = DecompressionMethods.GZip;
        return handler;
    });

builder.Services.AddHttpClient<ConfigurationClient>()
    .ConfigurePrimaryHttpMessageHandler(() =>
    {
        var handler = new SocketsHttpHandler();
        handler.AutomaticDecompression = DecompressionMethods.GZip;
        return handler;
    });

var app = builder.Build();

app.MapDefaultEndpoints();

app.RegisterVerifyEndpoints();

app.Run();

