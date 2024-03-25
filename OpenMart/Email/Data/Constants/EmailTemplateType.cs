using OpenMart.ExtraSharp.Enums;

namespace OpenMart.Email.Data.Constants;

public class EmailTemplateType : Enumeration<int, string>
{
    public static EmailTemplateType UserEmailConfirmation => new(0, "UsuarioEmailConfirmacion");
    public static EmailTemplateType UserAccountLocked => new(1, "UsusarioBloqueoCuenta");
    public static EmailTemplateType UserResetPassword => new(2, "UsuarioReestablecimientoContraseña");

    private EmailTemplateType(int id, string value) : base(id, value)
    {
    }
}