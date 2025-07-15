using Shall.Verify.Common.Helpers;
using Shall.Verify.OrchestrationService.Clients;
using Shall.Verify.OrchestrationService.Configurations;
using Shall.Verify.OrchestrationService.Extensions;
using Shall.Verify.OrchestrationService.Services;
using System.Net;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.Services.AddScoped<IOrchestrationService, OrchestrationService>();

builder.Services.AddSingleton<JsonSerializerOptionsWrapper>();

builder.Services.Configure<VerifyServiceOptions>(
    builder.Configuration.GetSection(VerifyServiceOptions.VerifyService));

builder.Services.Configure<RecordServiceOptions>(
    builder.Configuration.GetSection(RecordServiceOptions.RecordService));

builder.Services.AddHttpClient<VerifyClient>()
    .ConfigurePrimaryHttpMessageHandler(() =>
    {
        var handler = new SocketsHttpHandler();
        handler.AutomaticDecompression = DecompressionMethods.GZip;
        return handler;
    });

builder.Services.AddHttpClient<RecordClient>()
    .ConfigurePrimaryHttpMessageHandler(() =>
    {
        var handler = new SocketsHttpHandler();
        handler.AutomaticDecompression = DecompressionMethods.GZip;
        return handler;
    });

var app = builder.Build();

app.MapDefaultEndpoints();

app.RegisterOrchestrationEndpoints();

app.Run();