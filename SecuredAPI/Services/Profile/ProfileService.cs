using Security.Infrastructure;

namespace SecuredAPI.Services.Profile;

public class ProfileService : IProfileService
{
    private readonly AppDbContext _appDbContext;

    public ProfileService(AppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }

}
