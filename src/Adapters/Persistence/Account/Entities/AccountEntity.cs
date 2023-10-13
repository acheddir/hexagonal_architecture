namespace Me.Acheddir.Hexagonal.Persistence.Account.Entities;

public class AccountEntity
{
    [Key]
    public long Id { get; set; }
    public bool Locked { get; set; }
}