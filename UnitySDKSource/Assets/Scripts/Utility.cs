using System.Collections;
using System.Collections.Generic;
using System.Linq;
using MikrosClient;
using UnityEngine;
using Random = System.Random;

public static class Utility
{
	private const string FileEncryptKey = "WoGIrrX6GEA9fHaD5cahucLG88OHWlOK";

	private const string MikrosSavedAccountsFileName = "zKf8PICWw4wyyIi7";

	/// <summary>
	/// Get all saved Mikros accounts for app.
	/// </summary>
	/// <param name="savedMikrosAccounts">Collection of all saved Mikros accounts.</param>
	/// <returns>True if saved accounts found, or else False.</returns>
	internal static bool GetSavedMikrosAccounts(out List<MikrosUser> savedMikrosAccounts)
	{
		return SecureFileHandler.ReadFromFile(MikrosSavedAccountsFileName, out savedMikrosAccounts, FileEncryptKey);
	}

	/// <summary>
	/// Generate a random sequence of alpha-numeric characters.
	/// </summary>
	/// <param name="length">Length of the string to be generated.</param>
	/// <returns>Random sequence of alpha-numeric characters.</returns>
	internal static string GetRandomAlphanumericSequence(int length = 8)
    {
		Random random = new Random();
		const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
		return new string(Enumerable.Repeat(chars, length)
		  .Select(s => s[random.Next(s.Length)]).ToArray());
	}
}
