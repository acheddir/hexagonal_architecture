namespace Me.Acheddir.Hexagonal.Persistence.Account.Entities;

public class ActivityEntity
{
    public ActivityEntity(long id, long ownerAccountId, long sourceAccountId, long targetAccountId, DateTime timestamp, long amount)
    {
        Id = id;
        OwnerAccountId = ownerAccountId;
        SourceAccountId = sourceAccountId;
        TargetAccountId = targetAccountId;
        Timestamp = timestamp;
        Amount = amount;
    }

    [Key]
    public long Id { get; set; }
    public long OwnerAccountId { get; set; }
    public long SourceAccountId { get; set; }
    public long TargetAccountId { get; set; }
    public DateTime Timestamp { get; set; }
    public long Amount { get; set; }
}