using OpenMart.ExtraSharp.Enums;

namespace OpenMart.EmailService.Constants;

public class TextPartType : Enumeration<int, string>
{
    public static TextPartType Plain => new(0, "plain");
    public static TextPartType Html => new(1, "html");
    
    private TextPartType(int id, string value) : base(id, value)
    {
    }
}