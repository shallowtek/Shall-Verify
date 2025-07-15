using Shall.Verify.Common.Entities.Configuration;
using Shall.Verify.ConfigurationService.Services;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Shall.Verify.ConfigurationService.EndpointHandlers;

public static class ConfigurationHandlers
{
    #region - Rule Configuration
    public static async Task<Results<Ok<RuleConfiguration>, NotFound>> GetRuleConfigurationAsync(
        Guid siteId,
        IConfigurationService configurationService)
    {
        var ruleConfiguration = await configurationService.GetRuleConfigurationAsync(siteId);

        if (ruleConfiguration == null)
        {
            return TypedResults.NotFound();
        }

        return TypedResults.Ok(ruleConfiguration);
    }

    public static async Task<Results<Ok, ProblemHttpResult>> CreateOrUpdateRuleConfigurationAsync(
        RuleConfiguration ruleConfiguration,
        IConfigurationService configurationService)
    {
        var result = await configurationService.CreateOrUpdateRuleConfigurationAsync(ruleConfiguration);

        if (!result)
        {
            return TypedResults.Problem();
        }

        return TypedResults.Ok();
    }

    public static async Task<Results<Ok, NotFound>> DeleteRuleConfigurationAsync(
       Guid siteId,
       IConfigurationService configurationService)
    {
        var result = await configurationService.DeleteRuleConfigurationAsync(siteId);

        if (!result)
        {
            return TypedResults.NotFound();
        }

        return TypedResults.Ok();
    }
    #endregion - Rule Configuration

    #region - Feature Configuration
    public static async Task<Results<Ok<FeatureConfiguration>, NotFound>> GetFeatureConfigurationAsync(
       Guid siteId,
       IConfigurationService configurationService)
    {
        var featureConfiguration = await configurationService.GetFeatureConfigurationAsync(siteId);

        if (featureConfiguration == null)
        {
            return TypedResults.NotFound();
        }

        return TypedResults.Ok(featureConfiguration);
    }

    public static async Task<Results<Ok, ProblemHttpResult>> CreateOrUpdateFeatureConfigurationAsync(
       FeatureConfiguration featureConfiguration,
       IConfigurationService configurationService)
    {
        var result = await configurationService.CreateOrUpdateFeatureConfigurationAsync(featureConfiguration);

        if (!result)
        {
            return TypedResults.Problem();
        }

        return TypedResults.Ok();
    }

    public static async Task<Results<Ok, NotFound>> DeleteFeatureConfigurationAsync(
       Guid siteId,
       IConfigurationService configurationService)
    {
        var result = await configurationService.DeleteFeatureConfigurationAsync(siteId);

        if (!result)
        {
            return TypedResults.NotFound();
        }

        return TypedResults.Ok();
    }

    #endregion - Feature Configuration
}
