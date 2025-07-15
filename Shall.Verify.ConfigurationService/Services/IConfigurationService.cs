using Shall.Verify.Common.Entities.Configuration;

namespace Shall.Verify.ConfigurationService.Services;

public interface IConfigurationService
{
    #region - Rule Configuration
    Task<RuleConfiguration> GetRuleConfigurationAsync(Guid siteId);
    Task<bool> CreateOrUpdateRuleConfigurationAsync(RuleConfiguration ruleConfiguration);
    Task<bool> DeleteRuleConfigurationAsync(Guid siteId);
    #endregion - Rule Configuration

    #region - Feature Configuration
    Task<FeatureConfiguration> GetFeatureConfigurationAsync(Guid siteId);
    Task<bool> CreateOrUpdateFeatureConfigurationAsync(FeatureConfiguration featureConfiguration);
    Task<bool> DeleteFeatureConfigurationAsync(Guid siteId);
    #endregion - Feature Configuration
}