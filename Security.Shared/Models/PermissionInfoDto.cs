namespace Security.Shared.Models;

public class PermissionInfoDto
{
    public string ShortName { get; set; }
    public string Description { get; set; }
    public string PermissionName { get; set; }
    public bool IsSelected { get; set; }

    public override string ToString()
    {
        return $"{ShortName} - {Description}";
    }
}