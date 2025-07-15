using Shall.Verify.Common.Enums;

namespace Shall.Verify.Common.Entities.Configuration;

public class FeatureConfiguration : Configuration
{
    /// <summary>
    /// Set the features you would like to use: phone, email, record etc.
    /// </summary>
    public IList<Feature> Features { get; set; }
}

public class Feature
{
    /// <summary>
    /// Set the feature type: phone, email, record etc.
    /// </summary>
    public FeatureTypes FeatureType { get; set; }
    /// <summary>
    /// Set to enable this feature.
    /// </summary>
    public bool Enabled { get; set; }
}