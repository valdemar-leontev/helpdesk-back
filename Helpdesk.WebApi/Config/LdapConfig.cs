namespace Helpdesk.WebApi.Config;

public class LdapConfig
{
    public required string Address { get; set; }

    public int Port { get; set; }

    public int Version { get; set; }

    public required string DomainName { get; set; }
}