using Microsoft.AspNetCore.Authentication;

namespace Helpdesk.WebApi.Services;

public sealed class ApiKeyAuthenticationOptions : AuthenticationSchemeOptions
{
    public const string DefaultScheme = "API Key";

    public string Scheme => DefaultScheme;

    public string AuthenticationType = DefaultScheme;
}