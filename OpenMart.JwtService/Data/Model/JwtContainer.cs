using System.Text;
using System.Text.Json;
using OpenMart.ExtraSharp.Conversion;

namespace OpenMart.JwtService.Data.Model;

public class JwtContainer<T> where T : new()
{
    public JwtHeader Header { get; set; } = new();
    public T Payload { get; set; } = new T();

    public override string ToString()
    {
        var headerJson = JsonSerializer.Serialize(this.Header);
        var headerBase64 = Encoding.UTF8.GetBytes(headerJson).ToBase64UrlString();
        
        var payloadJson = JsonSerializer.Serialize(this.Payload);
        var payloadBase64 = Encoding.UTF8.GetBytes(payloadJson).ToBase64UrlString();
        
        return $"{headerBase64}.{payloadBase64}.";
    }
    public static explicit operator JwtContainer<T>(string value)
    {
        var parts = value.Split('.');
        var headerJson = Encoding.UTF8.GetString(parts[0].FromBase64UrlString());
        var payloadJson = Encoding.UTF8.GetString(parts[1].FromBase64UrlString());
        
        var header = JsonSerializer.Deserialize<JwtHeader>(headerJson);
        var payload = JsonSerializer.Deserialize<T>(payloadJson);

        if (header is null || payload is null)
        {
            throw new InvalidCastException("The provided value is not a valid JWT.");
        }
        
        return new JwtContainer<T>
        {
            Header = header,
            Payload = payload
        };
    }
}