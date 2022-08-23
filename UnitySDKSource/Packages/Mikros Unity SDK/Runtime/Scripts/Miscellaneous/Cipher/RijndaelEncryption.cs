using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;

namespace MikrosClient.Cipher
{
    /// <summary>
    /// Rijndael Encryption and Decryption.
    /// Source: https://docs.microsoft.com/en-us/dotnet/api/system.security.cryptography.rijndaelmanaged?view=net-6.0
    /// </summary>
    internal sealed class RijndaelEncryption
    {
        private static int bufferKeySize = 32;
        private static int blockSize = 256;
        private static int keySize = 256;

        /// <summary>
        /// Update encryption key settings.
        /// </summary>
        /// <param name="bufferKeySize">Buffer key size.</param>
        /// <param name="blockSize">Block size.</param>
        /// <param name="keySize">Key size.</param>
        internal static void UpdateEncryptionKeySize(int bufferKeySize = 32, int blockSize = 256, int keySize = 256)
        {
            RijndaelEncryption.bufferKeySize = bufferKeySize;
            RijndaelEncryption.blockSize = blockSize;
            RijndaelEncryption.keySize = keySize;
        }

        /// <summary>
        /// Standard Rijndael(AES) encrypt.
        /// </summary>
        /// <param name="plain">Plain text.</param>
        /// <param name="password">Encrypt key.</param>
        /// <returns>Encrypted and converted to Base64 string.</returns>
        internal static string Encrypt(string plain, string password)
        {
            byte[] encrypted = Encrypt(Encoding.UTF8.GetBytes(plain), password);
            return Convert.ToBase64String(encrypted);
        }

        /// <summary>
        /// Standard Rijndael(AES) encrypt.
        /// </summary>
        /// <param name="src">Plain binary.</param>
        /// <param name="password">Encrypt key.</param>
        /// <returns>Encrypted binary.</returns>
        internal static byte[] Encrypt(byte[] src, string password)
        {
            RijndaelManaged rij = SetupRijndaelManaged;

            // A pseudorandom number is newly generated based on the inputted password
            Rfc2898DeriveBytes deriveBytes = new Rfc2898DeriveBytes(password, bufferKeySize);
            // The missing parts are specified in advance to fill in 0 length
            byte[] salt = new byte[bufferKeySize];
            // Rfc2898DeriveBytes gets an internally generated satl
            salt = deriveBytes.Salt;
            // The 32-byte data extracted from the generated pseudorandom number is used as a password
            byte[] bufferKey = deriveBytes.GetBytes(bufferKeySize);

            rij.Key = bufferKey;
            rij.GenerateIV();

            using (ICryptoTransform encrypt = rij.CreateEncryptor(rij.Key, rij.IV))
            {
                byte[] dest = encrypt.TransformFinalBlock(src, 0, src.Length);
                // first 32 bytes of salt and second 32 bytes of IV for the first 64 bytes
                List<byte> compile = new List<byte>(salt);
                compile.AddRange(rij.IV);
                compile.AddRange(dest);
                return compile.ToArray();
            }
        }

        /// <summary>
        /// Standard Rijndael(AES) decrypt.
        /// </summary>
        /// <param name="encrtpted">Encrypted string.</param>
        /// <param name="password">Decrypt key.</param>
        /// <returns>Decrypted string.</returns>
        internal static string Decrypt(string encrtpted, string password)
        {
            byte[] decripted = Decrypt(Convert.FromBase64String(encrtpted), password);
            return Encoding.UTF8.GetString(decripted);
        }

        /// <summary>
        /// Standard Rijndael(AES) decrypt.
        /// </summary>
        /// <param name="src">Encrypted binary.</param>
        /// <param name="password">Decrypt key.</param>
        /// <returns>Decrypted binary.</returns>
        internal static byte[] Decrypt(byte[] src, string password)
        {
            RijndaelManaged rij = SetupRijndaelManaged;

            List<byte> compile = new List<byte>(src);

            // First 32 bytes are salt.
            List<byte> salt = compile.GetRange(0, bufferKeySize);
            // Second 32 bytes are IV.
            List<byte> iV = compile.GetRange(bufferKeySize, bufferKeySize);
            rij.IV = iV.ToArray();

            Rfc2898DeriveBytes deriveBytes = new Rfc2898DeriveBytes(password, salt.ToArray());
            byte[] bufferKey = deriveBytes.GetBytes(bufferKeySize);    // Convert 32 bytes of salt to password
            rij.Key = bufferKey;

            byte[] plain = compile.GetRange(bufferKeySize * 2, compile.Count - (bufferKeySize * 2)).ToArray();

            using (ICryptoTransform decrypt = rij.CreateDecryptor(rij.Key, rij.IV))
            {
                byte[] dest = decrypt.TransformFinalBlock(plain, 0, plain.Length);
                return dest;
            }
        }

        /// <summary>
        /// Basic setup for Rijndael Encryption
        /// </summary>
        private static RijndaelManaged SetupRijndaelManaged
        {
            get
            {
                RijndaelManaged rij = new RijndaelManaged();
                rij.BlockSize = blockSize;
                rij.KeySize = keySize;
                rij.Mode = CipherMode.CBC;
                rij.Padding = PaddingMode.PKCS7;
                return rij;
            }
        }
    }
}