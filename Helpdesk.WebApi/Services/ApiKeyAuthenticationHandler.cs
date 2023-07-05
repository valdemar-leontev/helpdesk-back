using System.Security.Claims;
using System.Text.Encodings.Web;
using Helpdesk.DataAccess;
using Helpdesk.Domain.Models.Admin;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Helpdesk.WebApi.Services;

public sealed class ApiKeyAuthenticationHandler : AuthenticationHandler<ApiKeyAuthenticationOptions>
{
    private readonly AppDatabaseContext _appDatabaseContext;

    private string? _failReasonPhrase;

    public ApiKeyAuthenticationHandler(
        IOptionsMonitor<ApiKeyAuthenticationOptions> options,
        ILoggerFactory logger,
        UrlEncoder encoder,
        ISystemClock clock,
        AppDatabaseContext appDatabaseContext
    ) : base(options, logger, encoder, clock)
    {
        _appDatabaseContext = appDatabaseContext;
    }

    protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        if (!Request.Headers.TryGetValue("X-Api-Key", out var apiKeyHeaderValues))
        {
            return AuthenticateResult.NoResult();
        }

        var providedApiKey = apiKeyHeaderValues.FirstOrDefault();

        if (apiKeyHeaderValues.Count == default || string.IsNullOrEmpty(providedApiKey))
        {
            return AuthenticateResult.NoResult();
        }

        var apiUser = await _appDatabaseContext
            .Set<UserDataModel>()
            .FirstOrDefaultAsync(u => u.Password == providedApiKey);

        if (apiUser == null)
        {
            _failReasonPhrase = "User not found";
            return AuthenticateResult.Fail(_failReasonPhrase);
        }

        var ticket = new AuthenticationTicket(
            new ClaimsPrincipal(new[]
            {
                new ClaimsIdentity(
                    new[]
                    {
                        new Claim(ClaimTypes.NameIdentifier, apiUser.Id.ToString()),
                        new Claim(ClaimTypes.Name, apiUser.Name)
                    },
                    Options.AuthenticationType
                )
            }),
            Options.Scheme
        );

        return AuthenticateResult.Success(ticket);
    }

    protected override Task HandleChallengeAsync(AuthenticationProperties properties)
    {
        if (_failReasonPhrase != null)
        {
            var responseFeature = Response.HttpContext.Features.Get<IHttpResponseFeature>();
            if (responseFeature is not null)
            {
                responseFeature.ReasonPhrase = _failReasonPhrase;
            }
        }

        Response.StatusCode = StatusCodes.Status401Unauthorized;

        return Task.CompletedTask;
    }

    protected override Task HandleForbiddenAsync(AuthenticationProperties properties)
    {
        if (_failReasonPhrase != null)
        {
            var responseFeature = Response.HttpContext.Features.Get<IHttpResponseFeature>();
            if (responseFeature is not null)
            {
                responseFeature.ReasonPhrase = _failReasonPhrase;
            }
        }

        Response.StatusCode = StatusCodes.Status403Forbidden;

        return Task.CompletedTask;
    }
}