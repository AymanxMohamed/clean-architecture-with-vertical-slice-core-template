using Core.Application.Common.Services;
using Core.Domain.Common.Services.Models;

using ErrorOr;

namespace Core.Infrastructure.Common.Services.Email;

public class EmailService(EmailServerSettings emailServer) : IEmailService
{
    public ErrorOr<Success> Send(
        string subject, 
        string body, 
        List<string> toEmails, 
        List<string>? ccEmails = null, 
        List<string>? bccEmails = null,
        List<EmailAttachment>? attachments = null,
        string? emailServerName = null)
    {
        return emailServer.Send(subject, body, toEmails, ccEmails, bccEmails, attachments);
    }
}