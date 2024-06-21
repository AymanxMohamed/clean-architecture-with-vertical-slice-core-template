using System.Net.Mail;

namespace ProjectName.Domain.Common.Services.Models;

public class EmailAttachment
{
    private EmailAttachment(string attachmentFilePath, string? attachmentDisplayName = null)
    {
        var attachmentName = attachmentDisplayName is null
            ? Path.GetFileName(attachmentFilePath) 
            : $"{attachmentDisplayName}{Path.GetExtension(attachmentFilePath)}";
        
        Attachment = new Attachment(
            contentStream: new MemoryStream(buffer: File.ReadAllBytes(attachmentFilePath).ToArray()), 
            name: attachmentName);
    }
    
    private EmailAttachment(string attachmentDisplayName, byte[] attachmentBuffer, string fileExtension)
    {
        Attachment = new Attachment(
            contentStream: new MemoryStream(buffer: attachmentBuffer), 
            name: $"{attachmentDisplayName}.{fileExtension}");
    }

    public Attachment Attachment { get; }
    
    public static implicit operator Attachment(EmailAttachment attachment) => attachment.Attachment;

    public static ErrorOr<EmailAttachment> Create(string attachmentDisplayName, string attachmentFilePath)
    {
        if (!File.Exists(attachmentFilePath))
        {
            return Errors.Errors.Email.FileNotFound;
        }

        return new EmailAttachment(attachmentDisplayName, attachmentFilePath);
    }

    public static ErrorOr<EmailAttachment> Create(
        string attachmentDisplayName, 
        byte[] attachmentBuffer,
        string fileExtension)
    {
        return new EmailAttachment(attachmentDisplayName, attachmentBuffer, fileExtension);
    }
}