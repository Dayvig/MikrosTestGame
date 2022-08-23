using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using MikrosClient;

namespace MikrosEditor
{
    /// <summary>
    /// Editor utility class.
    /// </summary>
    internal static class EditorUtils
    {
        private const string mikrosPackageIdentifier = "com.tatumgames.mikros";
        internal static string PathToMikrosPackageAssets = Path.Combine("Packages", mikrosPackageIdentifier);

        /// <summary>
        /// Reset all Mikros auth data.
        /// </summary>
        [MenuItem("Mikros/Tools/Reset Auth Data", false, 3)]
        private static void ResetAuthData()
        {
            if(EditorUtility.DisplayDialog("Confirm", "Are you sure to reset all data?\nThis process is irreversible", "Yes", "No"))
            {
                Utils.ResetMikrosAuth();
            }
        }

        /// <summary>
        /// Save all prefabs again.
        /// </summary>
        //[MenuItem("Mikros/Tools/Reserialize Prefabs")] // TODO: Do Not delete. Might be potential feature in future.
        private static void ReserializePrefabs()
        {
            foreach(string prefabPath in GetAllPrefabs())
            {
                GameObject prefabAsset = AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath);
                if(!PrefabUtility.IsPartOfImmutablePrefab(prefabAsset))
                {
					PrefabUtility.SavePrefabAsset(prefabAsset);
					Debug.Log("Re-serialized Prefab: " + prefabAsset.name);
                }
            }
        }

        /// <summary>
        /// Get path of all Mikros prefabs.
        /// </summary>
        /// <returns>Collection of path of all prefabs.</returns>
        private static string[] GetAllPrefabs()
        {
            return Directory.GetFiles(PathToMikrosPackageAssets, "*.prefab", SearchOption.AllDirectories);
        }
    }
}
