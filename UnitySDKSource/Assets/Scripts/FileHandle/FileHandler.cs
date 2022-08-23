using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Newtonsoft.Json;
using System;

/// <summary>
/// File handling.
/// </summary>
internal sealed class FileHandler
{
    /// <summary>
    /// Path to store all Mikros related local data.
    /// </summary>
    internal static string MikrosFileStorageBasePath => Path.Combine(Application.persistentDataPath, "Mikros");

    /// <summary>
    /// Reads an object from file in specified path.
    /// </summary>
    /// <typeparam name="T">Generic data-structure class.</typeparam>
    /// <param name="filePath">File path.</param>
    /// <param name="data">Retrieved data.</param>
    /// <returns>True if file read success, else False.</returns>
    internal static bool ReadFromFile<T>(string filePath, out T data)
    {
        data = default;
        string fileLocation = Path.Combine(MikrosFileStorageBasePath, filePath);

        if(File.Exists(fileLocation))
        {
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            FileStream file = File.Open(fileLocation, FileMode.Open);
            string t_ReadString = (string) binaryFormatter.Deserialize(file);
            file.Close();
            data = JsonConvert.DeserializeObject<T>(t_ReadString);
            return true;
        }
        else
        {
            return false;
        }
    }

    /// <summary>
    /// Reads a string from file in specified path.
    /// </summary>
    /// <param name="filePath">File path.</param>
    /// <param name="data">Retrieved data in string format.</param>
    /// <returns>True if file read success, else False.</returns>
    internal static bool ReadFromFile(string filePath, out string data)
    {
        data = default;
        string fileLocation = Path.Combine(MikrosFileStorageBasePath, filePath);

        if(File.Exists(fileLocation))
        {
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            FileStream file = File.Open(fileLocation, FileMode.Open);
            data = (string) binaryFormatter.Deserialize(file);
            file.Close();
            return true;
        }
        else
        {
            return false;
        }
    }

    /// <summary>
    /// Saves a file with given string content to specified path.
    /// </summary>
    /// <param name="data">Data to be saved in string format.</param>
    /// <param name="filePath">File path.</param>
    /// <param name="fileMode">Mode of access of file.</param>
    internal static void SaveToFile(string data, string filePath, FileMode fileMode = FileMode.OpenOrCreate)
    {
        string fileLocation = Path.Combine(MikrosFileStorageBasePath, filePath);
        CreateDirectory();
        BinaryFormatter binaryFormatter = new BinaryFormatter();
        FileStream file = File.Open(fileLocation, fileMode);
        binaryFormatter.Serialize(file, data);
        file.Close();
    }

    /// <summary>
    /// Saves a file with given object content to specified path.
    /// </summary>
    /// <typeparam name="T">Generic data-structure class.</typeparam>
    /// <param name="data">Data to be saved in generic data-structure format.</param>
    /// <param name="filePath">File path.</param>
    /// <param name="fileMode">Mode of access of file.</param>
    internal static void SaveToFile<T>(T data, string filePath, FileMode fileMode = FileMode.OpenOrCreate)
    {
        string fileLocation = Path.Combine(MikrosFileStorageBasePath, filePath);
        CreateDirectory();
        BinaryFormatter binaryFormatter = new BinaryFormatter();
        FileStream file = File.Open(fileLocation, fileMode);
        binaryFormatter.Serialize(file, JsonConvert.SerializeObject(data));
        file.Close();
    }

    /// <summary>
    /// Create a new directory at specified path if it doesn't exist already.
    /// </summary>
    public static void CreateDirectory()
    {
        if(!Directory.Exists(MikrosFileStorageBasePath))
        {
            Directory.CreateDirectory(MikrosFileStorageBasePath);
        }
    }

    /// <summary>
    /// Delete a file from specified path.
    /// </summary>
    /// <param name="filePath">File path.</param>
    /// <returns>True if file delete success, else False.</returns>
    internal static bool DeleteFile(string filePath)
    {
        string fileLocation = Path.Combine(MikrosFileStorageBasePath, filePath);
        try
        {
            if(File.Exists(fileLocation))
            {
                File.Delete(fileLocation);
                return true;
            }
            else
			{
                return false;
			}
        }
        catch (Exception ex)
        {
            new Exception(ex.Message);
            return false;
        }
    }

    /// <summary>
    /// Delete all file(s), directory(s) in a given path.
    /// </summary>
    /// <param name="relativePath">Relative folder path.</param>
    /// <returns>True if delete is successful, else False.</returns>
    internal static bool DeleteAll(string relativePath = "")
    {
        DirectoryInfo di = new DirectoryInfo(Path.Combine(MikrosFileStorageBasePath, relativePath));
        if(di.Exists)
        {
            try
            {
                foreach(FileInfo file in di.EnumerateFiles())
                {
                    file.Delete();
                }
                foreach(DirectoryInfo dir in di.EnumerateDirectories())
                {
                    dir.Delete(true);
                }
                return true;
            }
            catch(Exception ex)
            {
                if(Application.isEditor)
                {
                    Debug.LogException(ex);
                }
                else
                {
                    new Exception(ex.Message);
                }
                return false;
            }
        }
        else
		{
            return false;
		}
    }

    /// <summary>
    /// Saves a key with given string content to player prefs.
    /// </summary>
    /// <param name="key">Key of the player prefs.</param>
    /// <param name="value">Value of the player prefs.</param>
    internal static void SaveToPlayerPrefs(string key, string value)
    {
        PlayerPrefs.SetString(key, value);
    }

    /// <summary>
    /// Reads a string from key in player prefs.
    /// </summary>
    /// <param name="key">Key of the player prefs.</param>
    /// <param name="value">Value of the player prefs.</param>
    /// <param name="defaultValue">Default value of the player prefs if the corresponding key doesn't exist.</param>
    /// <returns>True if player prefs reading success, else False.</returns>
    internal static bool ReadFromPlayerPrefs(string key, out string value, string defaultValue = "")
    {
        value = string.Empty;
        if(PlayerPrefs.HasKey(key))
        {
            value = PlayerPrefs.GetString(key, defaultValue);
            return true;
        }
        else
        {
            return false;
        }
    }

    /// <summary>
    /// Delete a key from player prefs.
    /// </summary>
    /// <param name="key">Key of the player prefs.</param>
    /// <returns>True if player prefs key delete success, else False.</returns>
    internal static bool DeletePlayerPrefsKey(string key)
    {
        if(PlayerPrefs.HasKey(key))
        {
            PlayerPrefs.DeleteKey(key);
            return true;
        }
        else
        {
            return false;
        }
    }

    /// <summary>
    /// Delete all keys from player prefs.
    /// </summary>
    internal static void ClearPlayerPrefs()
    {
        PlayerPrefs.DeleteAll();
    }
}