using System.Net;
using System.Net.Mail;
using Ardalis.GuardClauses;
using DispatcherApp.Common.Abstractions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Logging;

namespace DispatcherApp.DAL.Services;

public class DummyEmailSender: IEmailSender<IdentityUser> , IEmailSender
{
    private readonly ILogger<DummyEmailSender> _logger;

    public DummyEmailSender(
        ILogger<DummyEmailSender> logger
        )
    {
        _logger = logger;
    }
    public Task SendConfirmationLinkAsync(IdentityUser user, string email, string confirmationLink)
    {
        var emailAddress = Guard.Against.NullOrEmpty(user.Email, nameof(user.Email));
        var decoded = WebUtility.HtmlDecode(confirmationLink);
        // Use structured logging instead of Console.WriteLine
        _logger.LogInformation("Email would be sent to {Email} with link {decoded}",
            user.Email, decoded);

        return Task.CompletedTask;
    }

    public Task SendEmailAsync(string email, string subject, string htmlMessage)
    {
        var decoded = WebUtility.HtmlDecode(htmlMessage);
        // Use structured logging instead of Console.WriteLine
        _logger.LogInformation("Email would be sent to {Email} with link {decoded}",
            email, decoded);

        return Task.CompletedTask; ;
    }

    public Task SendPasswordResetCodeAsync(IdentityUser user, string email, string resetCode)
    {
        Console.WriteLine($"Email would be sent to {user.Email}:");
        Console.WriteLine($"email: {email}");
        Console.WriteLine($"resetCode: {resetCode}");
        return Task.CompletedTask;
    }

    public Task SendPasswordResetLinkAsync(IdentityUser user, string email, string resetLink)
    {
        Console.WriteLine($"Email would be sent to {user.Email}:");
        Console.WriteLine($"email: {email}");
        Console.WriteLine($"resetLink: {resetLink}");
        return Task.CompletedTask;
    }
}
