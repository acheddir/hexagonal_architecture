namespace Me.Acheddir.Hexagonal.Persistence.Account.Entities;

public class ActivityEntity
{
    [Key]
    public long Id { get; set; }
    public long OwnerAccountId { get; set; }
    public long SourceAccountId { get; set; }
    public long TargetAccountId { get; set; }
    public DateTime Timestamp { get; set; }
    public long Amount { get; set; }
}