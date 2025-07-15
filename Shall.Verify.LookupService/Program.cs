using Shall.Verify.Common.Helpers;
using Shall.Verify.LookupService.Clients;
using Shall.Verify.LookupService.Configurations;
using Shall.Verify.LookupService.Extensions;
using Shall.Verify.LookupService.Services;
using System.Net;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.AddElasticsearchClient("elasticsearch");

builder.Services.AddScoped<ILookupService, LookupService>();

builder.Services.AddSingleton<JsonSerializerOptionsWrapper>();

builder.Services.Configure<AtDataServiceOptions>(
    builder.Configuration.GetSection(AtDataServiceOptions.AtDataService));

builder.Services.Configure<TelesignServiceOptions>(
    builder.Configuration.GetSection(TelesignServiceOptions.TelesignService));

builder.Services.Configure<RecordServiceOptions>(
    builder.Configuration.GetSection(RecordServiceOptions.RecordService));

builder.Services.AddHttpClient<AtDataClient>()
    .ConfigurePrimaryHttpMessageHandler(() =>
    {
        var handler = new SocketsHttpHandler();
        handler.AutomaticDecompression = DecompressionMethods.GZip;
        return handler;
    });

builder.Services.AddHttpClient<TelesignClient>()
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

app.RegisterLookupEndpoints();

app.Run();