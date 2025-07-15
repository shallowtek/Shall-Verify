using Shall.Verify.Common.Enums;
using System.ComponentModel.DataAnnotations;

namespace Shall.Verify.Common.Dtos.Record;

public class RecordStatusRequest
{
    [Required]
    public Guid VerifyId { get; set; }
    [Required]
    public RecordStatusType Status { get; set; }
}
