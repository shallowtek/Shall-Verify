using Shall.Verify.ConfigurationService.EndpointHandlers;

namespace Shall.Verify.ConfigurationService.Extensions;
public static class EndpointRouteBuilderExtensions
{
    public static void RegisterConfigurationEndpoints(this IEndpointRouteBuilder endpointRouteBuilder)
    {
        var ruleConfigurationEndpoints = endpointRouteBuilder.MapGroup("/ruleconfiguration");

        var ruleConfigurationWithGuidIdEndpoints = endpointRouteBuilder.MapGroup("/ruleconfiguration/{siteId:Guid}");

        var featureConfigurationEndpoints = endpointRouteBuilder.MapGroup("/featureconfiguration");

        var featureConfigurationWithGuidIdEndpoints = endpointRouteBuilder.MapGroup("/featureconfiguration/{siteId:Guid}");

        #region - Rule Configuration
        ruleConfigurationWithGuidIdEndpoints.MapGet("", ConfigurationHandlers.GetRuleConfigurationAsync)
        .WithName("GetRuleConfiguration")
        .WithOpenApi();

        ruleConfigurationEndpoints.MapPost("", ConfigurationHandlers.CreateOrUpdateRuleConfigurationAsync)
        .WithName("CreateOrUpdateRuleConfiguration")
        .WithOpenApi();

        ruleConfigurationWithGuidIdEndpoints.MapDelete("", ConfigurationHandlers.DeleteRuleConfigurationAsync)
        .WithName("DeleteRuleConfiguration")
        .WithOpenApi();
        #endregion - Rule Configuration

        #region - Feature Configuration
        featureConfigurationWithGuidIdEndpoints.MapGet("", ConfigurationHandlers.GetFeatureConfigurationAsync)
        .WithName("GetFeatureConfiguration")
        .WithOpenApi();

        featureConfigurationEndpoints.MapPost("", ConfigurationHandlers.CreateOrUpdateFeatureConfigurationAsync)
        .WithName("CreateOrUpdateFeatureConfiguration")
        .WithOpenApi();

        featureConfigurationWithGuidIdEndpoints.MapDelete("", ConfigurationHandlers.DeleteFeatureConfigurationAsync)
        .WithName("DeleteFeatureConfiguration")
        .WithOpenApi();
        #endregion - Feature Configuration
    }
}