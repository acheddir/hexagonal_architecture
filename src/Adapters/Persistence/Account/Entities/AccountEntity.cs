using System.ComponentModel.DataAnnotations.Schema;

namespace Me.Acheddir.Hexagonal.Persistence.Account.Entities;

public class AccountEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long Id { get; set; }
    public bool Locked { get; set; }
}