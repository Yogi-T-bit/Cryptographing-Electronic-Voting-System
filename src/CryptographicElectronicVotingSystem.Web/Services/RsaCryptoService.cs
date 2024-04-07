using System.Security.Cryptography;
using System.Text;

namespace CryptographicElectronicVotingSystem.Web.Services;

public class RsaCryptoService
{
    public static string Encrypt(string publicKey, string data)
    {
        using var rsa = new RSACryptoServiceProvider();
        rsa.FromXmlString(publicKey);
        var encryptedData = rsa.Encrypt(Encoding.UTF8.GetBytes(data), false);
        return Convert.ToBase64String(encryptedData);
    }

    public static string Decrypt(string privateKey, string data)
    {
        using var rsa = new RSACryptoServiceProvider();
        rsa.FromXmlString(privateKey);
        var decryptedData = rsa.Decrypt(Convert.FromBase64String(data), false);
        return Encoding.UTF8.GetString(decryptedData);
    }

    public static (string publicKey, string privateKey) GenerateKeys()
    {
        using var rsa = new RSACryptoServiceProvider();
        return (rsa.ToXmlString(false), rsa.ToXmlString(true));
    }
    
    public static string GeneratePrivateKey(int keySize = 2048)
    {
        using (var rsa = new RSACryptoServiceProvider(keySize))
        {
            return rsa.ToXmlString(true); // True to include the private key
        }
    }
    
    // Generates an RSA key pair and returns the public key.
    public static string GeneratePublicKey(int keySize = 2048)
    {
        using (var rsa = new RSACryptoServiceProvider(keySize))
        {
            return rsa.ToXmlString(false); // False to exclude the private key
        }
    }
}