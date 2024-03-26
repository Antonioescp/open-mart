using OpenMart.JwtService.Data.Model;

namespace OpenMart.JwtService.Test;

public class JsonWebTokenTests
{
    private JwtContainer<JwtRegisteredPayload> _jwtContainer = null!;

    [SetUp]
    public void Setup()
    {
        _jwtContainer = new JwtContainer<JwtRegisteredPayload>()
        {
            Payload = new JwtRegisteredPayload()
            {
                Subject = "testing",
                Issuer = "OpenMart",
            }
        };
    }

    [Test]
    public void JWT_EncodesAndDecodes()
    {
        var jwtString = _jwtContainer.ToString();
        var jwtContainer = (JwtContainer<JwtRegisteredPayload>)jwtString;
        
        Assert.Multiple(() =>
        {
            Assert.That(jwtContainer.Payload.Subject, Is.EqualTo(_jwtContainer.Payload.Subject));
            Assert.That(jwtContainer.Payload.Issuer, Is.EqualTo(_jwtContainer.Payload.Issuer));
            Assert.That(jwtContainer.ToString(), Is.EqualTo(_jwtContainer.ToString()));
        });
    }
}