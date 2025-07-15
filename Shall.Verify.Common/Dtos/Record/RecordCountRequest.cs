using System.ComponentModel.DataAnnotations;
using Shall.Verify.Common.Dtos.Lookup;

namespace Shall.Verify.Common.Dtos.Record;

public class RecordCountRequest
{
    [Required]
    public Guid VerifyId { get; set; }
    [Required]
    public Guid SiteId { get; set; }
    [Required]
    public IList<RecordCountSearchItem> RecordCountSearchItems { get; set; }
}