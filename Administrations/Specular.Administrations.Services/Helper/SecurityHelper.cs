using System.Security.Cryptography;

namespace NgSoftware.Specular.Administrations.Services.Helper;

/// <summary>
/// Хелпер по работе с криптографией
/// </summary>
public class SecurityHelper
{
    private const byte MaximumBits = 32;
    private const byte MaximumIterations = 6;

    /// <summary>
    /// Генерировать соль в 32 бита
    /// </summary>
    public static string GenerateSalt32() => GenerateSalt(MaximumBits);

    /// <summary>
    /// Генерировать соль заданного размера
    /// </summary>
    public static string GenerateSalt(int nSalt)
    {
        var provider = RandomNumberGenerator.Create();
        var salt = new byte[nSalt];
        provider.GetNonZeroBytes(salt);

        return Convert.ToBase64String(salt);
    }

    /// <summary>
    /// Получить солированный хэш пароля в 32 бита
    /// </summary>
    public static string HashPassword32(string password, string salt)
        => HashPassword(password, salt, MaximumIterations, MaximumBits);

    /// <summary>
    /// Получить солированный хэш пароля
    /// </summary>
    public static string HashPassword(string password, string salt, int nIterations, int nHash)
    {
        var saltBytes = Convert.FromBase64String(salt);
        using var rfc2898DeriveBytes = new Rfc2898DeriveBytes(password, saltBytes, nIterations);
        return Convert.ToBase64String(rfc2898DeriveBytes.GetBytes(nHash));
    }
}
