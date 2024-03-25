using OpenMart.Email.Data.Model;
using OpenMart.EmailService;

namespace OpenMart.Email;

public static class StmMailerExtensions
{
    public static SmtpMailer Alternatives(this SmtpMailer local, ICollection<EmailTemplate> templates)
    {
        foreach (var template in templates)
        {
            local.Alternative(template.Type, template.Content);
        }

        return local;
    }
}