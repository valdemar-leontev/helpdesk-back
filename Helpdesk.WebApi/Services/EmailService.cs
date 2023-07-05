using System.Security.Authentication;
using Helpdesk.WebApi.Config;
using Helpdesk.WebApi.Models;
using MimeKit;
using SmtpClient = MailKit.Net.Smtp.SmtpClient;

namespace Helpdesk.WebApi.Services;

public sealed class EmailService
{
    private readonly ApplicationConfig _applicationConfig;

    public EmailService(IApplicationConfig applicationConfig)
    {
        _applicationConfig = (ApplicationConfig)applicationConfig;
    }

    public async Task SendMessageAsync(Message message, string name, string email)
    {
        var emailConfig = _applicationConfig.EmailConfig;

        var mimeMessage = new MimeMessage();
        mimeMessage.From.Add(new MailboxAddress(_applicationConfig.AppTitle, emailConfig!.Address));
        mimeMessage.To.Add(new MailboxAddress(name, email));
        mimeMessage.Subject = message.Title;
        mimeMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
        {
            Text = message.Content
        };

        using var smtpClient = new SmtpClient();

        smtpClient.ServerCertificateValidationCallback = (_, _, _, _) => true;
        smtpClient.CheckCertificateRevocation = false;
        smtpClient.SslProtocols = SslProtocols.None
                                  | SslProtocols.Tls12
                                  | SslProtocols.Tls13;

        await smtpClient.ConnectAsync(emailConfig.Host, emailConfig.Port, emailConfig.UseSsl);
        await smtpClient.AuthenticateAsync(emailConfig.UserName, emailConfig.Password);
        await smtpClient.SendAsync(mimeMessage);
        await smtpClient.DisconnectAsync(true);
    }
}