using Shall.Verify.Common.Dtos.Lookup;
using System.ComponentModel.DataAnnotations;

namespace Shall.Verify.Common.Dtos.Record;

public class RecordMatchRequest
{
    [Required]
    public Guid VerifyId { get; set; }
    [Required]
    public Guid SiteId { get; set; }
    [Required]
    public HashSet<RecordMatchSearchItem> RecordMatchSearchItems { get; set; }
}
