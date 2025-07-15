using Shall.Verify.Common.Entities.Verify;
using Shall.Verify.Common.Enums;

namespace Shall.Verify.Common.Dtos.Verify;

public class VerifyResponse
{
    public Guid VerifyId { get; set; }
    public Guid SiteId { get; set; }
    public string ReferenceId { get; set; }
    public int Score { get; set; }
    public int FailThreshold { get; set; }
    public int FlagThreshold { get; set; }
    public ResultTypes Result { get; set; }
    public List<EmailVerifyResult> EmailResults { get; set; }
    public List<PhoneVerifyResult> PhoneResults { get; set; }
    public List<RecordCountVerifyResult> RecordCountResults { get; set; }
    public List<RecordMatchVerifyResult> RecordMatchResults { get; set; }

}