
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Security.Core.Models;
public class PermissionGroupDto
{
    public string GroupName { get; set; } = null!;
    public List<PermissionInfoDto> Permissions { get; set; } = null!;

}
