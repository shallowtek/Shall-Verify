namespace Shall.Verify.Common.Entities.Record;

public class RecordAttributes
{
    public Guid VerifyId { get; set; }
    public Guid SiteId { get; set; }
    /// <summary>
    /// When record is first created.
    /// </summary>
    public DateTime CreatedDate { get; set; }
    /// <summary>
    /// When record is modified: New status, email, phone added etc.
    /// </summary>
    public DateTime ModifiedDate { get; set; }
    public HashSet<string> Email { get; set; }
    public HashSet<string> Phone { get; set; }
    public HashSet<string> Status { get; set; }
}