using Shall.Verify.RecordService.Extensions;
using Shall.Verify.Common.Helpers;
using Shall.Verify.RecordService.Services;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.Services.AddScoped<IRecordService, RecordService>();

builder.Services.AddSingleton<JsonSerializerOptionsWrapper>();

builder.AddElasticsearchClient("elasticsearch");

var app = builder.Build();

app.MapDefaultEndpoints();

app.RegisterRecordEndpoints();

app.Run();