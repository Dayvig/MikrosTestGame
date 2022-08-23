#if UNITY_EDITOR

using MikrosClient;
using UnityEditor;
using UnityEngine;

namespace MikrosEditor
{
	/// <summary>
	/// Menu Editor class for Mikros Settings.
	/// </summary>
	internal class MikrosSettingsMenuEditor
	{
		private readonly static string mikrosSettingsFileName = EditorConstants.MikrosSettingsFileName;
		private readonly static string extensionName = EditorConstants.MikrosSettingsExtensionName;
		private readonly static string mikrosSettingsAssetPath = EditorConstants.MikrosSettingsAssetPath;

		/// <summary>
		/// Generate or returns a Mikros Settings scriptable object on first import of Mikros package.
		/// </summary>
		/// <returns>MikrosSettings instance.</returns>
		[InitializeOnLoadMethod]
		private static MikrosSettings InitializeMikrosSettingsAsset()
		{
			MikrosSettings mikrosSettingsAsset = GetMikrosSettingsAsset();
			if(mikrosSettingsAsset == null)
			{
				Debug.LogWarning(EditorConstants.MikrosSettingsWarningMessage);
				return CreateMikrosSettingsAsset();
			}
			else
			{
				return mikrosSettingsAsset;
			}
		}

		/// <summary>
		/// Returns Mikros Settings asset. (returns null, if not found)
		/// </summary>
		/// <returns>MikrosSettings instance.</returns>
		private static MikrosSettings GetMikrosSettingsAsset()
		{
			MikrosSettings mikrosSettingsObject = Resources.Load<MikrosSettings>(mikrosSettingsFileName);
			return mikrosSettingsObject;
		}

		/// <summary>
		/// Create a new Mikros Settings asset file.
		/// </summary>
		/// <returns>MikrosSettings instance.</returns>
		private static MikrosSettings CreateMikrosSettingsAsset()
		{
			MikrosSettings mikrosSettingsAsset = ScriptableObject.CreateInstance<MikrosSettings>();
			string[] splittedPath = mikrosSettingsAssetPath.Split('/');
			if(!AssetDatabase.IsValidFolder(mikrosSettingsAssetPath))
				AssetDatabase.CreateFolder(splittedPath[0], splittedPath[1]);
			AssetDatabase.CreateAsset(mikrosSettingsAsset, mikrosSettingsAssetPath + "/" + mikrosSettingsFileName + extensionName);
			AssetDatabase.SaveAssets();
			//FocusMikrosSettingsAsset(mikrosSettingsAsset);
			return mikrosSettingsAsset;
		}

		/// <summary>
		/// Select and focus Mikros Settings asset in inspector.
		/// </summary>
		[MenuItem(EditorConstants.MenuItemPath, false, 1)]
		private static void EditMikrosSettings()
		{
			MikrosSettings mikrosSettingsObject = InitializeMikrosSettingsAsset();
			FocusMikrosSettingsAsset(mikrosSettingsObject);
		}

		/// <summary>
		/// Focus an object in project to show in inspector.
		/// </summary>
		/// <param name="mikrosSettingsObject">MikrosSettings instance as Object.</param>
		private static void FocusMikrosSettingsAsset(Object mikrosSettingsObject)
		{
			EditorUtility.FocusProjectWindow();
			Selection.activeObject = mikrosSettingsObject;
		}
	}
}

#endif