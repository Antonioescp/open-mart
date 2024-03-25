using System.Reflection;
using JinianNet.JNTemplate;
using OpenMart.Email.Data.Model;
using OpenMart.EmailService;

namespace OpenMart.Email;

public static class StmMailerExtensions
{
    public static SmtpMailer TemplateAlternatives(this SmtpMailer local, IEnumerable<EmailTemplate> templates, object? args)
    {
        foreach (var template in templates)
        {
            var jnTemplate = Engine.CreateTemplate(template.Content);
            var properties = args?.GetType().GetProperties(BindingFlags.GetProperty 
                                                           | BindingFlags.Public 
                                                           | BindingFlags.Instance) 
                             ?? Array.Empty<PropertyInfo>();
            
            foreach (var prop in properties)
            {
                jnTemplate.Set(prop.Name, prop.GetValue(args));
            }
            
            local.Alternative(template.Type, jnTemplate.Render());
        }

        return local;
    } 
}