namespace Helpdesk.WebApi.Config;

public class ApplicationConfig : IApplicationConfig
{
    public string? AppTitle { get; set; }

    public string? AppSecret { get; set; }

    public AuthenticationConfig? AuthenticationConfig { get; set; }

    public EmailConfig? EmailConfig { get; set; }

    public LdapConfig? LdapConfig { get; set; }
}