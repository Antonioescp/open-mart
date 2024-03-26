using OpenMart.ExtraSharp.Enums;

namespace OpenMart.JwtService.Data.Constants;

public class JwtEnryptionAlgorithm : Enumeration<int, string>
{
    public static JwtEnryptionAlgorithm None = new(0, "none");
    public static JwtEnryptionAlgorithm HS256 = new(1, "HS256");
    
    private JwtEnryptionAlgorithm(int id, string value) : base(id, value)
    {
    }
}