namespace OpenMart.ExtraSharp.Conversion;

public static class ConvertExtension
{
    public static string ToBase64UrlString(this byte[] value)
    {
        return Convert.ToBase64String(value)
            .Trim('=')
            .Replace('+', '-')
            .Replace('/', '_');
    }
    
    public static byte[] FromBase64UrlString(this string value)
    {
        var base64 = value
            .Replace('-', '+')
            .Replace('_', '/');
        
        switch (base64.Length % 4)
        {
            case 2:
                base64 += "==";
                break;
            case 3:
                base64 += "=";
                break;
        }
        
        return Convert.FromBase64String(base64);
    } 
}