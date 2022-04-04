using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Security.Shared.Models.UserManagement;
public class RoleDisplayDto
{
    public string RoleName { get; set; } = null!;
    public List<PermissionInfoDto> Permissions { get; set; } = null!;
    public bool IsSelected { get; set; }
}
