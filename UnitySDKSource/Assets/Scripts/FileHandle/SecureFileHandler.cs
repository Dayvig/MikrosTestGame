using System.IO;
using UnityEngine;
using Newtonsoft.Json;

internal enum EncryptionMode
{
    Rijndael,
    RSA
}

/// <summary>
/// Encrypted file handling.
/// </summary>
internal sealed class SecureFileHandler
{
    /// <summary>
    /// Reads a secure object from file in specified path.
    /// </summary>
    /// <typeparam name="T">Generic data-structure class.</typeparam>
    /// <param name="filePath">File path.</param>
    /// <param name="data">Retrieved data.</param>
    /// <param name="password">Decrypt key.</param>
    /// <param name="encryptionMode">Mode of encryption.</param>
    /// <returns>True if file read success, else False.</returns>
    internal static bool ReadFromFile<T>(string filePath, out T data, string password, EncryptionMode encryptionMode = EncryptionMode.Rijndael)
    {
        data = default(T);

        if(FileHandler.ReadFromFile(filePath, out string encryptedData))
        {
            string decryptedData = encryptionMode == EncryptionMode.Rijndael ? RijndaelEncryption.Decrypt(encryptedData, password) : RSAEncryption.Decrypt(encryptedData, password);
            data = JsonConvert.DeserializeObject<T>(decryptedData);
            return true;
        }
        else
        {
            return false;
        }
    }

    /// <summary>
    /// Reads a secure string from file in specified path.
    /// </summary>
    /// <param name="filePath">File path.</param>
    /// <param name="data">Retrieved data.</param>
    /// <param name="password">Decrypt key.</param>
    /// <param name="encryptionMode">Mode of encryption.</param>
    /// <returns>True if file read success, else False.</returns>
    internal static bool ReadFromFile(string filePath, out string data, string password, EncryptionMode encryptionMode = EncryptionMode.Rijndael)
    {
        data = string.Empty;

        if(FileHandler.ReadFromFile(filePath, out string encryptedData))
        {
            data = encryptionMode == EncryptionMode.Rijndael ? RijndaelEncryption.Decrypt(encryptedData, password) : RSAEncryption.Decrypt(encryptedData, password);
            return true;
        }
        else
        {
            return false;
        }
    }

    /// <summary>
    /// Saves a secure file with given string content to specified path.
    /// </summary>
    /// <param name="data">Data to be saved in string format.</param>
    /// <param name="filePath">File path.</param>
    /// <param name="password">Encrypt key.</param>
    /// <param name="encryptionMode">Mode of encryption.</param>
    /// <param name="fileMode">Mode of access of file.</param>
    internal static void SaveToFile(string data, string filePath, string password, EncryptionMode encryptionMode = EncryptionMode.Rijndael, FileMode fileMode = FileMode.Create)
    {
        string encryptedData = encryptionMode == EncryptionMode.Rijndael ? RijndaelEncryption.Encrypt(data, password) : RSAEncryption.Encrypt(data, password);
        FileHandler.SaveToFile(encryptedData, filePath, fileMode);
    }

    /// <summary>
    /// Saves a secure file with given object content to specified path.
    /// </summary>
    /// <typeparam name="T">Generic data-structure class.</typeparam>
    /// <param name="data">Data to be saved in generic data-structure format.</param>
    /// <param name="filePath">File path.</param>
    /// <param name="password">Encrypt key.</param>
    /// <param name="encryptionMode">Mode of encryption.</param>
    /// <param name="fileMode">Mode of access of file.</param>
    internal static void SaveToFile<T>(T data, string filePath, string password, EncryptionMode encryptionMode = EncryptionMode.Rijndael, FileMode fileMode = FileMode.Create)
    {
        string strData = JsonConvert.SerializeObject(data);
        string encryptedData = encryptionMode == EncryptionMode.Rijndael ? RijndaelEncryption.Encrypt(strData, password) : RSAEncryption.Encrypt(strData, password);
        FileHandler.SaveToFile(encryptedData, filePath, fileMode);
    }

    /// <summary>
    /// Saves a secure key with given string content to player prefs.
    /// </summary>
    /// <param name="key">Key of the player prefs.</param>
    /// <param name="value">Value of the player prefs.</param>
    /// <param name="password">Encrypt key.</param>
    /// <param name="encryptionMode">Mode of encryption.</param>
    internal static void SaveToPlayerPrefs(string key, string value, string password, EncryptionMode encryptionMode = EncryptionMode.Rijndael)
    {
        string encryptedValue = encryptionMode == EncryptionMode.Rijndael ? RijndaelEncryption.Encrypt(value, password) : RSAEncryption.Encrypt(value, password);
        PlayerPrefs.SetString(key, encryptedValue);
    }

    /// <summary>
    /// Reads a secure string from key in player prefs.
    /// </summary>
    /// <param name="key">Key of the player prefs.</param>
    /// <param name="value">Value of the player prefs.</param>
    /// <param name="password">Decrypt key.</param>
    /// <param name="encryptionMode">Mode of encryption.</param>
    /// <param name="defaultValue">Default value of the player prefs if the corresponding key doesn't exist.</param>
    /// <returns>True if player prefs reading success, else False.</returns>
    internal static bool ReadFromPlayerPrefs(string key, out string value, string password, EncryptionMode encryptionMode = EncryptionMode.Rijndael, string defaultValue = "")
    {
        value = string.Empty;

        if(PlayerPrefs.HasKey(key))
        {
            string encryptedValue = PlayerPrefs.GetString(key, defaultValue);
            value = encryptionMode == EncryptionMode.Rijndael ? RijndaelEncryption.Decrypt(encryptedValue, password) : RSAEncryption.Decrypt(encryptedValue, password);
            return true;
        }
        else
        {
            return false;
        }
    }
}