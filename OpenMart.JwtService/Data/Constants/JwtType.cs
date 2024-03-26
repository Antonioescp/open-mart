using OpenMart.ExtraSharp.Enums;

namespace OpenMart.JwtService.Data.Constants;

public class JwtType : Enumeration<int, string>
{
    public static JwtType Jwt { get; } = new(1, "JWT");
    
    private JwtType(int id, string value) : base(id, value)
    {
    }
}