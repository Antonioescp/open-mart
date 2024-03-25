using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Security;

namespace OpenMart.Crypto;

public class PasswordHasher
{
    private readonly SecureRandom _cryptoRandom = new();

    public byte[] CreateSalt(int size)
    {
        var salt = new byte[size];
        _cryptoRandom.NextBytes(salt);
        return salt;
    }
    
    public string PBKDF2_SHA256_GetHash(string password, string saltAsBase64String, int iterations, int hashByteSize)
    {
        var saltBytes = Convert.FromBase64String(saltAsBase64String);

        var hash = PBKDF2_SHA256_GetHash(password, saltBytes, iterations, hashByteSize);

        return Convert.ToBase64String(hash);
    }

    private static byte[] PBKDF2_SHA256_GetHash(string password, byte[] salt, int iterations, int hashByteSize)
    {
        var pdb = new Pkcs5S2ParametersGenerator(new Org.BouncyCastle.Crypto.Digests.Sha256Digest());
        pdb.Init(PbeParametersGenerator.Pkcs5PasswordToBytes(password.ToCharArray()), salt,
                     iterations);
        var key = (KeyParameter)pdb.GenerateDerivedMacParameters(hashByteSize * 8);
        return key.GetKey();
    }
    
    public bool ValidatePassword(string password, string salt, int iterations, int hashByteSize, string hashAsBase64String)
    {
        var saltBytes = Convert.FromBase64String(salt);
        var actualHashBytes = Convert.FromBase64String(hashAsBase64String);
        return ValidatePassword(password, saltBytes, iterations, hashByteSize, actualHashBytes);
    }

    private bool ValidatePassword(string password, byte[] saltBytes, int iterations, int hashByteSize, byte[] actualGainedHasAsByteArray)
    {
        var testHash = PBKDF2_SHA256_GetHash(password, saltBytes, iterations, hashByteSize);
        return SlowEquals(actualGainedHasAsByteArray, testHash);
    }
    
    private static bool SlowEquals(byte[] a, byte[] b)
    {
        var diff = (uint)a.Length ^ (uint)b.Length;
        for (var i = 0; i < a.Length && i < b.Length; i++)
            diff |= (uint)(a[i] ^ b[i]);
        return diff == 0;
    }
}