using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Interview_Test.Models;

[Table("UserRoleMappingTb")]
public class UserRoleMappingModel
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid UserRoleMappingId { get; set; }
    public Guid UserId { get; set; }
    public UserModel User { get; set; } = null!;
    public int RoleId { get; set; }
    public RoleModel Role { get; set; } = null!;
}