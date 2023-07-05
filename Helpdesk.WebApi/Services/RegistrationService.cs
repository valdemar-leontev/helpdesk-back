using System.Text;
using System.Text.Json;
using Helpdesk.DataAccess;
using Helpdesk.Domain.Models.Admin;
using Helpdesk.Domain.Models.Dictionaries.Enums;
using Helpdesk.WebApi.Config;
using Helpdesk.WebApi.Helpers;
using Helpdesk.WebApi.Models;
using Microsoft.EntityFrameworkCore;

namespace Helpdesk.WebApi.Services;

public sealed class RegistrationService
{
    private readonly ApplicationConfig _applicationConfig;

    private readonly AppDatabaseContext _appDatabaseContext;

    private readonly EmailService _emailService;

    private readonly DataProtectionService _cryptoService;

    public RegistrationService(
        IApplicationConfig applicationConfig,
        AppDatabaseContext appDatabaseContext,
        DataProtectionService cryptoService,
        EmailService emailService)
    {
        _applicationConfig = (ApplicationConfig)applicationConfig;
        _appDatabaseContext = appDatabaseContext;
        _cryptoService = cryptoService;
        _emailService = emailService;
    }

    public async Task<bool> SendInvitationAsync(UserRegistrationModel userRegistration)
    {
        var json = JsonSerializer.Serialize(userRegistration);
        var response = _cryptoService.Encrypt(json);

        var message = new Message
        {
            Title = $"{_applicationConfig.AppTitle} Registration",
            Content = $"{_applicationConfig.AuthenticationConfig!.Audience}/login?registrationConfirmResponse=" +
                      Convert.ToBase64String
                      (
                          Encoding.UTF8.GetBytes($"email={userRegistration.Email}&registrationResponse={response}")
                      )
        };

        await _emailService.SendMessageAsync(message, $"{_applicationConfig.AppTitle} Client", userRegistration.Email);

        return true;
    }

    public async Task<bool> SendInvitationAsync(CorporateUserActivatorModel corporateUserActivator)
    {
        var json = JsonSerializer.Serialize(corporateUserActivator);
        var response = _cryptoService.Encrypt(json);

        var message = new Message
        {
            Title = "Helpdesk Authorization",
            Content = $"Вы активировали вашу корпоративную учетную запись в системе Helpdesk. " +
                      $"Вам назначен пароль <b>{corporateUserActivator.Password}</b> для входа в систему. <br /> <br />" +
                      $"Для подтверждения активации перейдите по указанной ссылке, чтобы завершить авторизацию. " +
                      $"{_applicationConfig.AuthenticationConfig!.Audience}/login?corporateUserConfirmResponse=" +
                      Convert.ToBase64String
                      (
                          Encoding.UTF8.GetBytes($"email={corporateUserActivator.Email}&registrationResponse={response}")
                      )
        };

        await _emailService.SendMessageAsync(message, corporateUserActivator.Name, corporateUserActivator.Email);

        return true;
    }

    public async Task<UserDataModel?> GetConfirmationCorpAsync(string email, string registrationResponse)
    {
        var response = _cryptoService.Decrypt(registrationResponse);
        var corporateAccount = JsonSerializer.Deserialize<CorporateUserActivatorModel>(response);

        if (corporateAccount is null || corporateAccount.Password.IsNullOrEmpty())
        {
            return null;
        }

        var updatedUser = await _appDatabaseContext
            .Set<UserDataModel>()
            .FirstOrDefaultAsync(u => u.Email == email);

        if (updatedUser is not null)
        {
            updatedUser.Password = await DataProtectionService.GetHashStringAsync(corporateAccount.Password!);
        }

        await _appDatabaseContext.SaveChangesAsync();

        return updatedUser;
    }

    public async Task<UserDataModel?> GetConfirmationAsync(string email, string registrationResponse)
    {
        var response = _cryptoService.Decrypt(registrationResponse);
        var userRegistration = JsonSerializer.Deserialize<UserRegistrationModel>(response);

        if (userRegistration != null && email == userRegistration.Email &&
            userRegistration.Password == userRegistration.ConfirmedPassword)
        {
            var newUser = await _appDatabaseContext
                .Set<UserDataModel>()
                .AddAsync(new UserDataModel
                {
                    Name = userRegistration.Name,
                    Email = userRegistration.Email,
                    Password = await DataProtectionService.GetHashStringAsync(userRegistration.Password),
                    RoleId = (int)Roles.User,
                });

            var affectedEntitiesCount = await _appDatabaseContext.SaveChangesAsync();

            return affectedEntitiesCount > default(int) ? newUser.Entity : null;
        }

        return null;
    }
}