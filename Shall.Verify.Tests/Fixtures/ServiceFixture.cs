namespace Shall.Verify.Testing.Integration.Fixtures;

public class ServiceFixture : IAsyncLifetime
{
    public IDistributedApplicationTestingBuilder appHost;

    public DistributedApplication app;

    public HttpClient orchestrationClient;
    public HttpClient recordClient;
    public HttpClient configurationClient;
    public HttpClient lookupClient;

    public async Task InitializeAsync() 
    {
        appHost = await DistributedApplicationTestingBuilder.CreateAsync<Projects.Shall_Verify_AppHost>();
        appHost.Services.ConfigureHttpClientDefaults(clientBuilder =>
        {
            clientBuilder.AddStandardResilienceHandler();
        });

        app = await appHost.BuildAsync();
        var resourceNotificationService = app.Services.GetRequiredService<ResourceNotificationService>();
        await app.StartAsync();

        // Clients
        configurationClient = app.CreateHttpClient("configurationservice");
        recordClient = app.CreateHttpClient("recordservice");
        orchestrationClient = app.CreateHttpClient("orchestrationservice");
        lookupClient = app.CreateHttpClient("lookupservice");

        // Act
        await resourceNotificationService.WaitForResourceAsync("elasticsearch", KnownResourceStates.Running).WaitAsync(TimeSpan.FromSeconds(60));
        await resourceNotificationService.WaitForResourceAsync("configurationservice", KnownResourceStates.Running).WaitAsync(TimeSpan.FromSeconds(30));
        await resourceNotificationService.WaitForResourceAsync("recordservice", KnownResourceStates.Running).WaitAsync(TimeSpan.FromSeconds(30));
        await resourceNotificationService.WaitForResourceAsync("orchestrationservice", KnownResourceStates.Running).WaitAsync(TimeSpan.FromSeconds(30));
        await resourceNotificationService.WaitForResourceAsync("verifyservice", KnownResourceStates.Running).WaitAsync(TimeSpan.FromSeconds(30));
        await resourceNotificationService.WaitForResourceAsync("lookupservice", KnownResourceStates.Running).WaitAsync(TimeSpan.FromSeconds(30));
        await resourceNotificationService.WaitForResourceAsync("messaging", KnownResourceStates.Running).WaitAsync(TimeSpan.FromSeconds(30));

        // Elastic takes much longer to get up and running locally
        Thread.Sleep(90000);
    }

    public async Task DisposeAsync()
    {
        // Pretty sure all of these will get disposed but sure better safe than sorry.
        configurationClient.Dispose();
        orchestrationClient.Dispose();
        recordClient.Dispose();
        lookupClient.Dispose();
        await app.DisposeAsync();
    }
}