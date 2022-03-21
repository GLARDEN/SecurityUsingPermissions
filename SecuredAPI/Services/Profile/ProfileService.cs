using Security.Data;
using Security.Shared.Models;

namespace SecuredAPI.Services.Profile;

public class ProfileService : IProfileService
{
    private readonly AppDbContext _appDbContext;

    public ProfileService(AppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }

}
