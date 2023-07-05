using Helpdesk.DataAccess;
using Helpdesk.Domain.Models.Admin;
using Helpdesk.WebApi.Config;
using Helpdesk.WebApi.Models;
using Helpdesk.WebApi.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Novell.Directory.Ldap;

namespace Helpdesk.WebApi.Controllers;

[ApiController]
[Route("/login")]
public class LoginController : ControllerBase
{
    private readonly AppDatabaseContext _appDatabaseContext;

    private readonly TokenService _tokenService;

    private readonly ApplicationConfig _applicationConfig;

    public LoginController(IApplicationConfig applicationConfig, AppDatabaseContext appDatabaseContext, TokenService tokenService)
    {
        _applicationConfig = (ApplicationConfig)applicationConfig;
        _appDatabaseContext = appDatabaseContext;
        _tokenService = tokenService;
    }

    [HttpGet]
    public async Task<IActionResult> LoginAsync([FromQuery] LoginModel login)
    {
        // TODO: extract to command
        var userEntity = await _appDatabaseContext
            .Set<UserDataModel>()
            .Include(u => u.Role)
            .Include(u => u.Profile)
            .FirstOrDefaultAsync(u => u.Email == login.Email && u.ObjectSid != null);

        if (userEntity is not null)
        {
            try
            {
                var ldapConnection = new LdapConnection();
                var ldapConfig = _applicationConfig.LdapConfig;
                ldapConnection.Connect(ldapConfig!.Address, ldapConfig.Port);
                ldapConnection.Bind(ldapConfig.Version, $"domain\\{userEntity.Name}", login.Password);
                ldapConnection.Disconnect();
            }
            catch (LdapException e)
            {
                return Problem(e.Message);
            }
            catch (Exception e)
            {
                return Problem(e.Message);
            }
        }
        else
        {
            HttpContext.Request.Cookies.ContainsKey("AD-SID");

            login.Password = HttpContext.Request.Cookies.ContainsKey("AD-SID") && login.IsInternal
                ? HttpContext.Request.Cookies["AD-SID"]!
                : login.Password;

            var hashedPassword = login.IsInternal
            ? login.Password
            : await DataProtectionService.GetHashStringAsync(login.Password);

            // TODO: extract to command
            userEntity = await _appDatabaseContext
                .Set<UserDataModel>()
                .Include(u => u.Role)
                .Include(u => u.Profile)
                .FirstOrDefaultAsync(user => (user.Email == login.Email || login.IsInternal) && user.Password == hashedPassword);
        }

        if (userEntity is null)
        {
            return Problem();
        }

        var authUser = await _tokenService.CreateAuthUserAsync(userEntity);

        return authUser is not null ? Ok(authUser) : Problem();
    }

    [HttpPost]
    public async Task<IActionResult> RefreshToken(AuthUserModel authUser)
    {
        var refreshedAuthUser = await _tokenService.RefreshAuthUserAsync(authUser);

        return refreshedAuthUser is not null ? Ok(refreshedAuthUser) : Problem();
    }
};