using Shall.Verify.Common.Entities.Record;

namespace Shall.Verify.Common.Dtos.Verify;

public class VerifyRequest
{
    public Guid VerifyId { get; set; }
    public string ReferenceId { get; set; }
    /// <summary>
    /// Set the siteId so we can identify the calling site and apply the correct feature and rule configurations.
    /// </summary>
    public Guid SiteId { get; set; }
    /// <summary>
    /// Record of data you would like verified.
    /// </summary>
    public RecordData Record { get; set; }
}