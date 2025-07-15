using System.ComponentModel.DataAnnotations;
using Shall.Verify.Common.Entities.Record;

namespace Shall.Verify.Common.Dtos.Orchestration;

public class OrchestrationVerifyRequest
{
    public string ReferenceId { get; set; }
    /// <summary>
    /// Set the siteId so we can identify the calling site and apply the correct feature and rule configurations.
    /// </summary>
    [Required]
    public Guid SiteId { get; set; }
    /// <summary>
    /// Record of data you would like verified.
    /// </summary>
    [Required]
    public RecordData Record { get; set; }
}