using Elastic.Clients.Elasticsearch;
using Shall.Verify.Common.Entities.Configuration;
using System.Text.Json;

namespace Shall.Verify.ConfigurationService.Services
    ;

public class ConfigurationService : IConfigurationService
{
    private readonly ElasticsearchClient _elasticClient;
    const string ruleConfigurationIndex = "rule-configuration";
    const string featureConfigurationIndex = "feature-configuration";

    public ConfigurationService(ElasticsearchClient elasticClient)
    {
        _elasticClient = elasticClient;
    }

    #region - Rule Configuration
    public async Task<RuleConfiguration> GetRuleConfigurationAsync(Guid clientId)
    {
        var response = await _elasticClient.GetAsync<RuleConfiguration>(ruleConfigurationIndex, clientId);

        if (!response.IsSuccess())
        {
            return null;
        }

        return response.Source;
    }

    public async Task<bool> CreateOrUpdateRuleConfigurationAsync(RuleConfiguration ruleConfiguration)
    {
        var response = await _elasticClient.IndexAsync(ruleConfiguration, ruleConfigurationIndex, ruleConfiguration.SiteId);

        if (!response.IsSuccess())
        {
            return false;
        }

        return true;
    }

    public async Task<bool> DeleteRuleConfigurationAsync(Guid siteId)
    {
        var response = await _elasticClient.DeleteAsync(index:ruleConfigurationIndex, id:siteId);

        if (!response.IsSuccess())
        {
            return false;
        }

        return true;
    }
    #endregion - Rule Configuration

    #region - Feature Configuration
    public async Task<FeatureConfiguration> GetFeatureConfigurationAsync(Guid siteId)
    {
        var response = await _elasticClient.GetAsync<FeatureConfiguration>(featureConfigurationIndex, siteId);

        if (!response.IsSuccess())
        {
            return null;
        }

        return response.Source;
    }

    public async Task<bool> CreateOrUpdateFeatureConfigurationAsync(FeatureConfiguration featureConfiguration)
    {

        var response = await _elasticClient.IndexAsync(featureConfiguration, featureConfigurationIndex, featureConfiguration.SiteId);

        if (!response.IsSuccess())
        {
            return false;
        }

        return true;
    }

    public async Task<bool> DeleteFeatureConfigurationAsync(Guid siteId)
    {
        var response = await _elasticClient.DeleteAsync(index:featureConfigurationIndex, id: siteId);

        if (!response.IsSuccess())
        {
            return false;
        }

        return true;
    }
    #endregion - Feature Configuration
}
