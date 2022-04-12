namespace Security.Core.Models.UserManagement;
public class UserSummaryDto
{
    public Guid Id { get; set; }

    public string Email { get; set; } = null!;

    public DateTime CreatedWhen { get; set; } = DateTime.Now;

    public IEnumerable<string> RoleNames { get; set; } = new List<string>();
    public UserSummaryDto() { }

}
