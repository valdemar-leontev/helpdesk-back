using Helpdesk.DataAccess;
using Helpdesk.Domain.Models.Admin;
using Helpdesk.WebApi.Helpers;
using Helpdesk.WebApi.Models;
using Helpdesk.WebApi.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Helpdesk.WebApi.Controllers;

[ApiController]
[Route("/registrations")]
public class RegistrationController : ControllerBase
{
    private readonly RegistrationService _registrationService;

    private readonly TokenService _tokenService;

    private readonly AppDatabaseContext _appDatabaseContext;

    public RegistrationController(
        RegistrationService registrationService,
        TokenService tokenService,
        AppDatabaseContext appDatabaseContext)
    {
        _registrationService = registrationService;
        _tokenService = tokenService;
        _appDatabaseContext = appDatabaseContext;
    }

    [HttpGet("invite-corporate")]
    public async Task<IActionResult> Invite([FromQuery] CorporateUserActivatorModel corporateUserActivator)
    {
        var user = await _appDatabaseContext
            .Set<UserDataModel>()
            .FirstOrDefaultAsync(u => u.Email == corporateUserActivator.Email && u.ObjectSid != null);

        if (user is null)
        {
            return Problem();
        }

        var passwordGenerator = new StrongPasswordGenerator(8);
        corporateUserActivator.Password = passwordGenerator.Next();
        corporateUserActivator.Name = user.Name;

        await _registrationService.SendInvitationAsync(corporateUserActivator);

        return Ok();
    }

    [HttpGet("confirm-corporate")]
    public async Task<IActionResult> ConfirmCorp(string email, string registrationResponse)
    {
        var userEntity = await _registrationService.GetConfirmationCorpAsync(email, registrationResponse);

        if (userEntity is null)
        {
            return Problem();
        }

        var authUser = await _tokenService.CreateAuthUserAsync(userEntity);

        return Ok(authUser);
    }

    [HttpGet("invite")]
    public async Task<IActionResult> Invite([FromQuery] UserRegistrationModel userRegistration)
    {
        await _registrationService.SendInvitationAsync(userRegistration);

        return Ok(userRegistration);
    }

    [HttpGet("confirm")]
    public async Task<IActionResult> Confirm(string email, string registrationResponse)
    {
        var userEntity = await _registrationService.GetConfirmationAsync(email, registrationResponse);

        if (userEntity is null)
        {
            return Problem();
        }

        var authUser = await _tokenService.CreateAuthUserAsync(userEntity);

        return authUser is not null ? Ok(authUser) : Problem();
    }
}