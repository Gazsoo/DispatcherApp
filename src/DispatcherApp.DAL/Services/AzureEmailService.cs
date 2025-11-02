using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime;
using System.Text;
using System.Threading.Tasks;
using Ardalis.GuardClauses;
using Azure.Communication.Email;
using DispatcherApp.Common.Abstractions;
using DispatcherApp.DAL.Configurations;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace DispatcherApp.DAL.Services;
public class AzureEmailService : IEmailSender<IdentityUser> , IEmailSender
{
    private readonly EmailClient _emailClient;
    private readonly ILogger<AzureEmailService> _logger;
    private readonly EmailSettings _settings;
    public AzureEmailService(
        EmailClient emailClient,
        ILogger<AzureEmailService> logger,
        IOptions<EmailSettings> settings
        )
    {
        _settings = settings.Value;
        _logger = logger;
        _emailClient = emailClient;
    }

    public Task SendConfirmationLinkAsync(IdentityUser user, string email, string confirmationLink)
    {
        throw new NotImplementedException();
    }

    public async Task SendEmailAsync(string email, string subject, string htmlMessage)
    {
        var senderAddress = _settings.SenderAddress;
        Guard.Against.NullOrEmpty(senderAddress, nameof(senderAddress), "Sender address is not configured.");

        var decoded = WebUtility.HtmlDecode(htmlMessage);
        _logger.LogInformation("Email would be sent to {Email} with link {decoded}",
            email, decoded);

        var message = new EmailMessage(
            senderAddress: senderAddress,
            content: new EmailContent(subject) { Html = htmlMessage },
            recipients: new EmailRecipients(new[] { new EmailAddress(email) })
        );

        var a = await _emailClient.SendAsync(Azure.WaitUntil.Completed, message);
    }

    public Task SendPasswordResetCodeAsync(IdentityUser user, string email, string resetCode)
    {
        throw new NotImplementedException();
    }

    public Task SendPasswordResetLinkAsync(IdentityUser user, string email, string resetLink)
    {
        throw new NotImplementedException();
    }
}
