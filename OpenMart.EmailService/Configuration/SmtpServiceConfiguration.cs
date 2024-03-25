namespace OpenMart.EmailService.Configuration;

public class SmtpServiceConfiguration
{
    public SmtpServiceReceiverConfiguration Receiver { get; set; } = null!;
    
    public string Url { get; set; } = null!;
    public string Sender { get; set; } = null!;
    public int Port { get; set; }
    public string Password { get; set; } = null!;
    public string Email { get; set; } = null!;
}

public class SmtpServiceReceiverConfiguration
{
    public string Name { get; set; } = null!;
    public string Email { get; set; } = null!;
}