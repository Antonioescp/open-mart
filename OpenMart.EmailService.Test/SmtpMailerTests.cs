using Microsoft.Extensions.Configuration;
using MimeKit;
using OpenMart.EmailService.Configuration;
using OpenMart.EmailService.Constants;

namespace OpenMart.EmailService.Test;

public class SmtpMailerTests
{
    private SmtpMailer _mailer = null!;
    private IConfiguration _config = null!;
    
    [SetUp]
    public void Setup()
    {
        _config = new ConfigurationBuilder()
            .AddUserSecrets<SmtpMailerTests>()
            .Build();

        var serviceConfig = _config.GetRequiredSection("SMTPService").Get<SmtpServiceConfiguration>();
        if (serviceConfig is null)
        {
            Assert.Fail("Couldn't get service configuration");
        }
        
        _mailer = new SmtpMailer(
            serviceConfig!.Url, 
            serviceConfig.Port, 
            serviceConfig.Email, 
            serviceConfig.Password);
        _mailer.Sender(serviceConfig.Sender, serviceConfig.Email);
        _mailer.Receiver(serviceConfig.Receiver.Name, serviceConfig.Receiver.Email);
    }

    [Test]
    public void Supports_SimpleBody_Email()
    {
        _mailer
            .Body(TextPartType.Html, "Hello world")
            .Subject("Email test");
        
        Assert.Multiple(() =>
        {
            Assert.That(_mailer.Message.Body, Is.TypeOf<TextPart>(), "Body should be a text part");
            Assert.That(_mailer.Message.BodyParts.Count(), Is.EqualTo(1), "There should only be one body part");
        });
    }

    [Test]
    public void Supports_AlternativeBody_Email()
    {
        _mailer
            .Alternative(TextPartType.Plain, "What's up suckers")
            .Alternative(TextPartType.Plain, "Hey there");
        
        Assert.Multiple(() =>
        {
            Assert.That(_mailer.Message.Body, Is.TypeOf<Multipart>(), "Message body should be multipart");
            Assert.That(_mailer.Message.BodyParts.Count(), Is.EqualTo(2), "Message should only contain 2 body parts");
        });
        
        foreach (var part in _mailer.Message.BodyParts)
        {
            Assert.That(part, Is.TypeOf<TextPart>(), "Body part should be of type TextPart");
        }
    }

    [Test]
    public void Supports_HtmlAlternative_Email()
    {
        _mailer
            .Alternative(TextPartType.Plain, "Hello world")
            .Alternative(TextPartType.Html, "<h1><i>Hello</i> world!</h1>");
        
        Assert.Multiple(() =>
        {
            Assert.That(_mailer.Message.BodyParts.Count(), Is.EqualTo(2), "Body should only have 2 parts");
            Assert.That(_mailer.Message.Body, Is.TypeOf<Multipart>(), "Body should be Multipart");
        });
    }

    [Test]
    public void Supports_Body_WithAttachments_Email()
    {
        _mailer
            .Alternative("text", "Here is the attachment")
            .Alternative("html", "<p>Here <i>is</i> the attachment</p>")
            .Attachment("./test-attachment.txt", "text", "plain");
        
        Assert.Multiple(() =>
        {
            Assert.That(_mailer.Message.BodyParts.Count(), Is.EqualTo(3), "Body should only have 2 parts");
            Assert.That(_mailer.Message.Body, Is.TypeOf<Multipart>(), "Body should be Multipart");
            Assert.That(
                _mailer.Message.BodyParts.Count(part => part is TextPart),
                Is.EqualTo(2),
                "Body should contain 2 text parts");
            Assert.That(
                _mailer.Message.BodyParts.Count(part => part is MimePart),
                Is.EqualTo(3),
                "Body should only have 3 mime part");
        });
    }

    [Test]
    public void Sends_WithBodyAndAttachment_Email()
    {
        _mailer
            .Subject("Unit test")
            .Alternative(TextPartType.Plain, "Title body with important text.")
            .Alternative(TextPartType.Html, "<h1>Title</h1><p>body with <em>important</em> text.</p>")
            .Attachment("./test-attachment.txt", "text", "plain")
            .Send();
    }
}