
var builder = DistributedApplication.CreateBuilder(args);

//var cache = builder.AddRedis("cache");

//var shallVerifyDB = builder.AddPostgres("postgres")
//    .AddDatabase("ShallVerifyPostgres");

var elasticsearch = builder.AddElasticsearch("elasticsearch")
    .WithDataVolume();

var messaging = builder.AddKafka("messaging")
                       .WithKafkaUI();

var recordService = builder.AddProject<Projects.Shall_Verify_RecordService>("recordservice")
    .WithReference(elasticsearch);

var lookupService = builder.AddProject<Projects.Shall_Verify_LookupService>("lookupservice")
    .WithReference(recordService);

var configurationService = builder.AddProject<Projects.Shall_Verify_ConfigurationService>("configurationservice")
    .WithReference(elasticsearch);

var verifyService = builder.AddProject<Projects.Shall_Verify_VerifyService>("verifyservice")
    .WithReference(configurationService)
    .WithReference(lookupService)
    .WithReference(messaging);

var orchestrationService = builder.AddProject<Projects.Shall_Verify_OrchestrationService>("orchestrationservice")
    .WithReference(verifyService)
    .WithReference(recordService)
    .WithExternalHttpEndpoints();

builder.AddProject<Projects.Shall_Verify_Dashboard>("dashboard")
    .WithExternalHttpEndpoints();
    //.WithReference(cache);
//.WithReference(shallVerifyDB);

builder.Build().Run();
