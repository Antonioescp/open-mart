using OpenMart.ExtraSharp.Enums;

namespace OpenMart.Email.Data.Constants;

public class EmailTemplateName : Enumeration<int, string>
{
    public static EmailTemplateName UserEmailConfirmation => new(0, "UsuarioEmailConfirmacion");
    public static EmailTemplateName UserAccountLocked => new(1, "UsusarioBloqueoCuenta");
    public static EmailTemplateName UserResetPassword => new(2, "UsuarioReestablecimientoContraseña");

    private EmailTemplateName(int id, string value) : base(id, value)
    {
    }
}