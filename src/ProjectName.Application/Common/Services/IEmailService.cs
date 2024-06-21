using ProjectName.Domain.Common.Services.Models;

namespace ProjectName.Application.Common.Services;

public interface IEmailService
{
    ErrorOr<Success> Send(
        string subject,
        string body,
        List<string> toEmails,
        List<string>? ccEmails = default,
        List<string>? bccEmails = null,
        List<EmailAttachment>? attachments = null,
        string? emailServerName = null);
}