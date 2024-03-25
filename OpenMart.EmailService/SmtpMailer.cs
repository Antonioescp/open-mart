using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;

namespace OpenMart.EmailService;

public class SmtpMailer
{
    public MimeMessage Message { get; } = new();
    public SmtpClient Client { get; } = new();
    
    public string Smtp { get; }
    public int Port { get; }
    public string AccountEmail { get; }
    public string AccountPassword { get; }

    public SmtpMailer(string smtp, int port, string email, string password)
    {
        this.Smtp = smtp;
        this.Port = port;
        this.AccountEmail = email;
        this.AccountPassword = password;
    }

    private static MimePart GetFileMimePart(string filePath, string type, string subtype) => new(type, subtype)
    {
        Content = new MimeContent(File.OpenRead(filePath)),
        ContentDisposition = new ContentDisposition(ContentDisposition.Attachment),
        ContentTransferEncoding = ContentEncoding.Base64,
        FileName = Path.GetFileName(filePath)
    };

    public SmtpMailer Sender(string name, string address)
    {
        this.Message.From.Add(new MailboxAddress(name, address));
        return this;
    }

    public SmtpMailer Receiver(string name, string address)
    {
        this.Message.To.Add(new MailboxAddress(name, address));
        return this;
    }
    
    public SmtpMailer Subject(string subject)
    {
        this.Message.Subject = subject;
        return this;
    }

    public SmtpMailer Body(string textType, string text)
    {
        var textPart = new TextPart(textType)
        {
            Text = text,
        };
        this.UpdateBody(textPart);
        return this;
    }
    
    public SmtpMailer Attachment(string filePath, string type, string subtype)
    {
        var attachment = GetFileMimePart(filePath, type, subtype);
        this.UpdateBody(attachment);
        return this;
    }

    public SmtpMailer Alternative(string type, string text)
    {
        var body = this.Message.Body;
        var textPart = new TextPart(type) { Text = text };
        
        switch (body)
        {
            case TextPart currentTextPart:
            {
                var alternative = new MultipartAlternative();
                alternative.Add(currentTextPart);
                alternative.Add(textPart);
                this.Message.Body = null;
                this.UpdateBody(alternative);
                break;
            }
            case Multipart multipart:
            {
                var existingAlternatives = multipart
                    .FirstOrDefault(part => part is MultipartAlternative) as MultipartAlternative;

                existingAlternatives?.Add(textPart);

                if (existingAlternatives is null 
                    && multipart.FirstOrDefault(part => part is TextPart) is TextPart existingTextPart)
                {
                    var alternative = new MultipartAlternative();
                    alternative.Add(existingAlternatives);
                    alternative.Add(textPart);
                    this.UpdateBody(alternative);
                }

                break;
            }
            case null:
            {
                this.UpdateBody(textPart);
                break;
            }
        }

        return this;
    }

    public async void SendAsync(SecureSocketOptions option = SecureSocketOptions.StartTls)
    {
        await this.Client.ConnectAsync(this.Smtp, this.Port, option);
        await this.Client.AuthenticateAsync(this.AccountEmail, this.AccountPassword);
        await this.Client.SendAsync(this.Message);
        await this.Client.DisconnectAsync(true);
    }

    public void Send(SecureSocketOptions option = SecureSocketOptions.StartTls)
    {
        this.Client.Connect(this.Smtp, this.Port, option);
        this.Client.Authenticate(this.AccountEmail, this.AccountPassword);
        this.Client.Send(this.Message);
        this.Client.Disconnect(true);
    }

    private void UpdateBody(MimeEntity mimePart)
    {
        switch (Message.Body)
        {
            case null when mimePart is not MultipartAlternative:
                Message.Body = mimePart;
                break;
            default:
            {
                if (Message.Body is not Multipart)
                {
                    var multipartBody = new Multipart();
                    multipartBody.Add(mimePart);
            
                    var currentBody = Message.Body;
                    if (currentBody is not null)
                    {
                        multipartBody.Add(currentBody);
                    }

                    this.Message.Body = multipartBody;
                } 
                else if (Message.Body is Multipart multipart)
                {
                    multipart.Add(mimePart);
                }

                break;
            }
        }
    }
}