using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;

/// <summary>
/// RSA Encryption and Decryption.
/// Source: https://docs.microsoft.com/en-us/dotnet/api/system.security.cryptography.rsa?view=net-6.0
/// </summary>
internal sealed class RSAEncryption
{
    /// <summary>
    /// Generate Public And Private KeyPair.
    /// </summary>
    /// <param name="keySize">Key size.</param>
    /// <returns>Public key and private key KeyValuePair.</returns>
    internal static KeyValuePair<string, string> GenrateKeyPair(int keySize)
    {
        RSACryptoServiceProvider rsa = new RSACryptoServiceProvider(keySize);
        string publicKey = rsa.ToXmlString(false);
        string privateKey = rsa.ToXmlString(true);
        return new KeyValuePair<string, string>(publicKey, privateKey);
    }

    /// <summary>
    /// Standard RSA encrypt.
    /// </summary>
    /// <param name="plain">Plain text.</param>
    /// <param name="publicKey">Public key.</param>
    /// <returns>Encrypted and converted to Base64 string.</returns>
    internal static string Encrypt(string plain, string publicKey)
    {
        byte[] encrypted = Encrypt(Encoding.UTF8.GetBytes(plain), publicKey);
        return Convert.ToBase64String(encrypted);
    }

    /// <summary>
    /// Standard RSA encrypt.
    /// </summary>
    /// <param name="src">Plain binary.</param>
    /// <param name="publicKey">Public key.</param>
    /// <returns>Encrypted binary.</returns>
    internal static byte[] Encrypt(byte[] src, string publicKey)
    {
        using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider())
        {
            rsa.FromXmlString(publicKey);
            byte[] encrypted = rsa.Encrypt(src, false);
            return encrypted;
        }
    }

    /// <summary>
    /// Standard RSA decrypt.
    /// </summary>
    /// <param name="encrtpted">Encrypted string.</param>
    /// <param name="privateKey">Private key.</param>
    /// <returns>Decrypted string.</returns>
    internal static string Decrypt(string encrtpted, string privateKey)
    {
        byte[] decripted = Decrypt(Convert.FromBase64String(encrtpted), privateKey);
        return Encoding.UTF8.GetString(decripted);
    }

    /// <summary>
    /// Standard RSA decrypt.
    /// </summary>
    /// <param name="src">Encrypted binary.</param>
    /// <param name="privateKey">Private key.</param>
    /// <returns>Decrypted binary.</returns>
    internal static byte[] Decrypt(byte[] src, string privateKey)
    {
        using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider())
        {
            rsa.FromXmlString(privateKey);
            byte[] decrypted = rsa.Decrypt(src, false);
            return decrypted;
        }
    }
}
