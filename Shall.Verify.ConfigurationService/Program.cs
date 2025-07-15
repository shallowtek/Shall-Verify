using Shall.Verify.ConfigurationService.Extensions;
using Shall.Verify.ConfigurationService.Services;
using Shall.Verify.Common.Helpers;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.Services.AddScoped<IConfigurationService, ConfigurationService>();

builder.Services.AddSingleton<JsonSerializerOptionsWrapper>();

builder.AddElasticsearchClient("elasticsearch");

var app = builder.Build();

app.MapDefaultEndpoints();

app.RegisterConfigurationEndpoints();

app.Run();