using AutoMapper;

using Security.Shared.Models;
using Security.Shared.Models.Authentication;
using Security.Shared.Models.UserManagement;
using Security.Shared.Permissions.Extensions;

using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;

namespace SecuredAPI.Services;

public class AuthenticationService : IAuthenticationService
{
    private readonly IMapper _mapper;
    private readonly AppDbContext _appDbContext;
    private readonly IJwtTokenService _jwtTokenService;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public AuthenticationService(IMapper mapper, AppDbContext appDbContext, IJwtTokenService jwtTokenService, IHttpContextAccessor httpContextAccessor)
    {
        _mapper = mapper;
        _appDbContext = appDbContext;
        _jwtTokenService = jwtTokenService;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<ChangePasswordResponse> ChangePassword(ChangePasswordRequest changePasswordRequest)
    {
        User? user = await _appDbContext.Users.FindAsync(changePasswordRequest.UserId);

        if (user == null)
        {
            return new ChangePasswordResponse() { PasswordChangeSuccessfull = false, ErrorMessage = "User Not Found." };
        }

        CreatePasswordHash(changePasswordRequest.Password, out byte[] passwordHash, out byte[] passwordSalt);
        user.SetPasswordHash(passwordHash, passwordSalt);

        await _appDbContext.SaveChangesAsync();

        return new ChangePasswordResponse() { PasswordChangeSuccessfull = true };

    }

    public async Task<LoginResponseDto> LoginAsync(LoginRequestDto loginRequest)
    {
        LoginResponseDto loginResponse = new();

        User? user = await _appDbContext.Users.Include(u => u.UserRoles).FirstOrDefaultAsync(u => u.Email.ToLower().Equals(loginRequest.Email.ToLower()));

        if (user == null)
        {
            loginResponse.IsAuthenticationSuccessful = false;
            loginResponse.ErrorMessage = "Login attempt failed";
        }
        else if (!VerifyPasswordHash(loginRequest.Password, user.PasswordHash, user.PasswordSalt))
        {
            loginResponse.IsAuthenticationSuccessful = false;
            loginResponse.ErrorMessage = "Login attempt failed";
        }
        else
        {
            //TODO: Pull user info and claims (permissions) from database
            var userTokenInfo = new UserTokenDetails()
            {
                Id = user.Id,
                Email = user.Email,
                Permissions = GetPermissionsForUser(user.UserRoles.ToList() ?? new List<UserRole>())
            };

            loginResponse.Token = _jwtTokenService.GenerateToken(userTokenInfo);
            loginResponse.IsAuthenticationSuccessful = true;
        }

        return loginResponse;
    }

    public async Task<RegistrationResponseDto> RegisterAsync(RegistrationRequestDto registrationRequest, string password)
    {

        if (await UserExists(registrationRequest.Email))
        {
            return new RegistrationResponseDto()
            {
                IsRegistrationSuccessful = false,
                Errors = new List<string>() { "User already exists." }
            };
        }

        CreatePasswordHash(password, out byte[] passwordHash, out byte[] passwordSalt);

        User _newUser = new User(registrationRequest.Email, passwordHash, passwordSalt);

        _appDbContext.Users.Add(_newUser);

        await _appDbContext.SaveChangesAsync();

        RegistrationResponseDto _registrationResponse = new RegistrationResponseDto()
        {
            Id = _newUser.Id,
            IsRegistrationSuccessful = true,

        };

        return _registrationResponse;
    }

    public Guid GetUserId()
    {
        string userIdResult = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty;

        return Guid.Parse(userIdResult);
    }

    public async Task<bool> UserExists(string email)
    {
        if (await _appDbContext.Users.AnyAsync(u => u.Email.ToLower().Equals(email.ToLower())))
        {
            return true;
        }
        return false;
    }

    private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
    {
        using HMACSHA512 hmac = new HMACSHA512(passwordSalt);

        byte[] computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));

        return computedHash.SequenceEqual(passwordHash);
    }

    private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
    {
        using var hmac = new HMACSHA512();

        passwordSalt = hmac.Key;
        passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
    }

    private string GetPermissionsForUser(List<UserRole> userRoles)
    {
        List<string> assignedPermissions = userRoles.Select(ur => ur.AssignedPermissions).ToList();

        if (!assignedPermissions.Any())
            return "";

        //thanks to https://stackoverflow.com/questions/5141863/how-to-get-distinct-characters
        var packedPermissionsForUser = new string(string.Concat(assignedPermissions).Distinct().ToArray());

        return packedPermissionsForUser;
    }
}
