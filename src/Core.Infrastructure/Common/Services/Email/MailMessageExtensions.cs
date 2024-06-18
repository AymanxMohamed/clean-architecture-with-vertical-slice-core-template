using System.Net.Mail;

using Core.Domain.Common.Errors;
using Core.Domain.Common.Services.Models;

using ErrorOr;

namespace Core.Infrastructure.Common.Services.Email;

public static class MailMessageExtensions
{
    public static ErrorOr<MailMessage> AddParticipants(
        this MailMessage emailMessage, 
        List<string> toEmails, 
        List<string>? ccEmails = null, 
        List<string>? bccEmails = null)
    {
        if (toEmails.Count < 1)
        {
            return Errors.Email.MissingParticipant;
        }
        
        ccEmails ??= [];
        bccEmails ??= [];
        
        toEmails.ForEach(emailMessage.To.Add);
        ccEmails.ForEach(emailMessage.CC.Add);
        bccEmails.ForEach(emailMessage.Bcc.Add);

        return emailMessage;
    }
    
    public static ErrorOr<MailMessage> AddAttachments(
        this ErrorOr<MailMessage> emailMessage, 
        List<EmailAttachment>? attachments = null)
    {
        if (emailMessage.IsError)
        {
            return emailMessage.Errors;
        }
        
        attachments ??= [];
        
        attachments.Select(x => x.Attachment).ToList().ForEach(emailMessage.Value.Attachments.Add);

        return emailMessage;
    }
}