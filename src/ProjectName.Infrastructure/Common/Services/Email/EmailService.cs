using ProjectName.Application.Common.Services;
using ProjectName.Domain.Common.Services.Models;

using ErrorOr;

namespace ProjectName.Infrastructure.Common.Services.Email;

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