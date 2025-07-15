using Shall.Verify.Common.Dtos.Lookup;
using Shall.Verify.Common.Dtos.Orchestration;
using Shall.Verify.Common.Entities.Configuration;
using Shall.Verify.Common.Entities.Record;
using Shall.Verify.Common.Enums;
using Shall.Verify.Common.Helpers;
using Shall.Verify.Testing.Integration.Attributes;
using Shall.Verify.Testing.Integration.Fixtures;
using System.Net.Http.Headers;
using System.Text.Json;

namespace Shall.Verify.Testing.Integration.Configuration;

[TestCaseOrderer(
    ordererTypeName: "Shall.Verify.Testing.Integration.Orderers.PriorityOrderer",
    ordererAssemblyName: "Shall.Verify.Testing.Integration")]
public class ConfigurationTests : IClassFixture<ServiceFixture>
{
    private readonly ServiceFixture _serviceFixture;
    JsonSerializerOptionsWrapper jsonSerializerOptionsWrapper = new JsonSerializerOptionsWrapper();

    private readonly Guid _siteId = new Guid("3fa85f64-5717-4562-b3fc-2c963f66afa6");
    private readonly Guid _verifyId = new Guid("c8e053f5-0e8b-4659-aae8-62764cb2f839");

    public ConfigurationTests(ServiceFixture serviceFixture)
    {
        _serviceFixture = serviceFixture;
    }

    [Fact, TestPriority(0)]
    public async Task WriteAndReadFeatureConfigurationReturnsOkStatusCode()
    {
        // Arrange
        var featureConfiguration = new FeatureConfiguration
        {
            SiteId = _siteId,
            Features = new List<Feature> {
                new Feature {
                    FeatureType = FeatureTypes.Email,
                    Enabled = true
                },
                new Feature {
                    FeatureType = FeatureTypes.Phone,
                    Enabled = true
                },
                new Feature {
                    FeatureType = FeatureTypes.RecordCount,
                    Enabled = true
                },
            }
        };

        var data = JsonSerializer.Serialize(featureConfiguration);
        var content = new StringContent(data);
        content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

        // Act
        var responseWrite = await _serviceFixture.configurationClient.PostAsync("/featureconfiguration", content);
        var responseRead = await _serviceFixture.configurationClient.GetAsync($"/featureconfiguration/{_siteId}");

        // Assert
        Assert.Equal(HttpStatusCode.OK, responseWrite.StatusCode);
        Assert.Equal(HttpStatusCode.OK, responseRead.StatusCode);
    }

    [Fact, TestPriority(1)]
    public async Task WriteAndReadRuleConfigurationReturnsOkStatusCode()
    {
        // Arrange
        var ruleConfiguration = new RuleConfiguration
        {
            SiteId = _siteId,
            FailThreshold = 10,
            FlagThreshold = 5,
            Rules = new List<Rule>
            {
                new Rule
                {
                    FeatureType = FeatureTypes.Email,
                    FailThreshold = 10,
                    Weighting = 10,
                    IsNegativeWeighting = false
                },
                new Rule
                {
                    FeatureType = FeatureTypes.Phone,
                    FailThreshold = 10,
                    Weighting = 10,
                    IsNegativeWeighting = false
                }
            },
            RecordCountRules = new List<RecordCountRule>
            {
                new RecordCountRule
                {
                    FeatureType = FeatureTypes.RecordCount,
                    DataType = RecordDataTypes.Email,
                    FailThreshold = 10,
                    Weighting = 10,
                    TimeRangeInSeconds = 10,
                    IsNegativeWeighting = false
                },
                new RecordCountRule
                {
                    FeatureType = FeatureTypes.RecordCount,
                    DataType = RecordDataTypes.Email,
                    FailThreshold = 20,
                    Weighting = 20,
                    TimeRangeInSeconds = 20,
                    IsNegativeWeighting = false
                },
                new RecordCountRule
                {
                    FeatureType = FeatureTypes.RecordCount,
                    DataType = RecordDataTypes.Phone,
                    FailThreshold = 30,
                    Weighting = 30,
                    TimeRangeInSeconds = 30,
                    IsNegativeWeighting = false
                },
                new RecordCountRule
                {
                    FeatureType = FeatureTypes.RecordCount,
                    DataType = RecordDataTypes.Phone,
                    FailThreshold = 40,
                    Weighting = 40,
                    TimeRangeInSeconds = 40,
                    IsNegativeWeighting = false
                }
            }
        };

        var data = JsonSerializer.Serialize(ruleConfiguration);

        var content = new StringContent(data);
        content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

        // Act
        var response = await _serviceFixture.configurationClient.PostAsync("/ruleconfiguration", content);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact, TestPriority(2)]
    public async Task WriteAndReadRecordReturnsOkStatusCode()
    {
        // Arrange
        var recordAttributes = new RecordAttributes
        {
            VerifyId = _verifyId,
            SiteId = _siteId,
            Email = new HashSet<string>
            {
                {"highrisk@gmail.com"},
                {"mediumrisk@gmail.com"},
                {"lowrisk@gmail.com"},
            },
            Phone = new HashSet<string>
            {
                {"0871111111"},
                {"0872222222"},
                {"0873333333"},
            }
        };

        var data = JsonSerializer.Serialize(recordAttributes);

        var content = new StringContent(data);
        content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

        // Act
        var responseWrite = await _serviceFixture.recordClient.PostAsync("/record", content);

        var responseRead = await _serviceFixture.recordClient.GetAsync($"/record/{_verifyId}");

        // Assert
        Assert.Equal(HttpStatusCode.OK, responseWrite.StatusCode);
        Assert.Equal(HttpStatusCode.OK, responseRead.StatusCode);
    }

    [Fact, TestPriority(3)]
    public async Task CallLookupReturnsOkStatusCode()
    {
        // Arrange
        var lookupRequest = new LookupRequest
        {
            VerifyId = _verifyId,
            SiteId = _siteId,
            Record = new RecordData
            {
                Email = new List<Email>
                {
                    new Email
                    {
                        EmailAddress = "highrisk@gmail.com"
                    },
                    new Email
                    {
                        EmailAddress = "mediumrisk@gmail.com"
                    },
                    new Email
                    {
                        EmailAddress = "lowrisk@gmail.com"
                    }
                },
                Phone = new List<Phone>
                {
                    new Phone
                    {
                        PhoneNumber = "0871111111"
                    },
                    new Phone
                    {
                        PhoneNumber = "0872222222"
                    },
                    new Phone
                    {
                        PhoneNumber = "0873333333"
                    }
                }
            }
        };

        var data = JsonSerializer.Serialize(lookupRequest);

        var content = new StringContent(data);
        content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

        // Act
        var response = await _serviceFixture.lookupClient.PostAsync("/lookup", content);
        var stream = await response.Content.ReadAsStreamAsync();
        var result = await JsonSerializer.DeserializeAsync
            <LookupResponse>(stream, jsonSerializerOptionsWrapper.Options);

        // Assert
        Assert.True(result.EmailResults.Count() == 3);
        Assert.True(result.PhoneResults.Count() == 3);
    }

    [Fact, TestPriority(4)]
    public async Task CallOrchestrationVerifyReturnsOkStatusCode()
    {
        // Arrange
        var orchestrationVerifyRequest = new OrchestrationVerifyRequest
        {
            SiteId = _siteId,
            ReferenceId = "Some_Ref_Id",
            Record = new RecordData
            {
                Email = new List<Email>
                {
                    new Email {EmailAddress = "highrisk@gmail.com"},
                    new Email {EmailAddress = "mediumrisk@gmail.com"},
                    new Email {EmailAddress = "lowrisk@gmail.com"},
                },
                Phone = new List<Phone>
                {
                    new Phone {PhoneNumber = "0871111111"},
                    new Phone {PhoneNumber = "0872222222"},
                    new Phone {PhoneNumber = "0873333333"},
                }
            }
        };

        var data = JsonSerializer.Serialize(orchestrationVerifyRequest);

        var content = new StringContent(data);
        content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

        // Act
        var response = await _serviceFixture.orchestrationClient.PostAsync("/verify", content);
        var stream = await response.Content.ReadAsStreamAsync();
        var result = await JsonSerializer.DeserializeAsync
            <OrchestrationVerifyResponse>(stream, jsonSerializerOptionsWrapper.Options);

        // Assert
        Assert.True(result.EmailResults.Count() == 3);
        Assert.True(result.PhoneResults.Count() == 3);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }
}
