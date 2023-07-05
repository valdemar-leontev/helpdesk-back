namespace Helpdesk.WebApi.Config;

public class EmailConfig
{
    public required string Address { get; set; }

    public required string Host { get; set; }

    public int Port { get; set; }

    public required string UserName { get; set; }

    public required string Password { get; set; }

    public bool UseSsl { get; set; }
}