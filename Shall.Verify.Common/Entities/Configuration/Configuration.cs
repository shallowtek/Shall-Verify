namespace Shall.Verify.Common.Entities.Configuration;

public class Configuration
{
    public Guid SiteId { get; set; }
    public DateTime ModifiedDate { get; internal set; } = DateTime.UtcNow;
}