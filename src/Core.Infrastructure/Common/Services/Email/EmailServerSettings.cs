using System.Net;
using System.Net.Mail;

using Core.Domain.Common.Services.Models;

using ErrorOr;

namespace Core.Infrastructure.Common.Services.Email;

public class EmailServerSettings
{
    public string SmtpServerName { get; init; } = null!;
    
    public string SmtpServer { get; init; } = null!;
    
    public int Port { get; init; }
    
    public string Username { get; init; } = null!;
    
    public string Password { get; init; } = null!;
    
    public string DisplayName { get; init; } = null!;
    
    public string? TargetName { get; set; }
    
    private SmtpClient SmtpClient => new(SmtpServer, Port)
    {
        UseDefaultCredentials = false,
        Credentials = new NetworkCredential(Username, Password),
        EnableSsl = true,
        TargetName = TargetName
    };
    
    public ErrorOr<Success> Send(
        string subject, 
        string body, 
        List<string> toEmails,
        List<string>? ccEmails = null, 
        List<string>? bccEmails = null,
        List<EmailAttachment>? attachments = null)
    {
        var result = GetEmailMessage(subject, body, toEmails, ccEmails, bccEmails, attachments)
            .ThenDo(mailMessage => SmtpClient.Send(mailMessage));
        
        if (result.IsError)
        {
            return result.Errors;
        }
        
        return Result.Success;
    }

    private ErrorOr<MailMessage> GetEmailMessage(
        string subject, 
        string body, 
        List<string> toEmails,
        List<string>? ccEmails = null, 
        List<string>? bccEmails = null,
        List<EmailAttachment>? attachments = null)
    {
        return new MailMessage
            {
                From = new MailAddress(address: Username, displayName: DisplayName),
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            }.AddParticipants(toEmails, ccEmails, bccEmails)
            .AddAttachments(attachments);
    }
}