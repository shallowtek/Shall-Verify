using Shall.Verify.Common.Enums;

namespace Shall.Verify.Common.Entities.Configuration;

public class RuleConfiguration : Configuration
{
    /// <summary>
    /// Set the threshold that is used with the rule weightings to determine the verify result.
    /// </summary>
    public int FailThreshold { get; set; }
    /// <summary>
    /// Set the flag percentage that is used with the verify threshold and the rule weightings to determine the verify result.
    /// </summary>
    public int FlagThreshold { get; set; }
    /// <summary>
    /// Set standard score related rules.
    /// </summary>
    public IList<Rule> Rules { get; set; }
    /// <summary>
    /// Set the record rules that use a specific data type and time range.
    /// </summary>
    public IList<RecordCountRule> RecordCountRules { get; set; }

    public IList<RecordMatchRule> RecordMatchRules { get; set; }
}

public class Rule
{
    /// <summary>
    /// Set the feature type: phone, email, record etc.
    /// </summary>
    public FeatureTypes FeatureType { get; set; }
    /// <summary>
    /// Set the threshold for the rule to trigger.
    /// </summary>
    public int FailThreshold { get; set; }
    /// <summary>
    /// Set the weighting that will be applied if the rule triggers.
    /// </summary>
    public int Weighting { get; set; }
    /// <summary>
    /// Set the negative weighting flag if you want the rule weighting to be deducted if the rule triggers.
    /// </summary>
    public bool IsNegativeWeighting { get; set; }
}

public class RecordCountRule : Rule
{
    /// <summary>
    /// Set the data type: phone, email, firstname etc
    /// </summary>
    public RecordDataTypes DataType { get; set; }
    /// <summary>
    /// Set the time range in seconds used to determine the time range to search over with maximum of 1 year.
    /// 1 year = 31536000 seconds
    /// 1 month = 2628000 seconds
    /// 1 week = 604800 seconds
    /// 1 day = 86400 seconds
    /// 1 hour = 3600 seconds
    /// </summary>
    public int TimeRangeInSeconds { get; set; }
}

public class RecordMatchRule : Rule
{
    public IList<MatchWeighting> MatchWeightings { get; set; }
}

public class MatchWeighting 
{
    public RecordDataTypes DataType { get; set; }
    public int DataWeight { get; set; }
}