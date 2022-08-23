using UnityEngine;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEditor;
using MikrosClient;
using System;
using Object = UnityEngine.Object;

namespace MikrosEditor
{
	/// <summary>
	/// Pre-build check for Mikros setup.
	/// </summary>
	public sealed class BuildProcessor : IPreprocessBuildWithReport
	{
		private readonly static string mikrosSettingsFileName = "Mikros Settings";
		private readonly static string mikrosSettingsErrorMessage = "There are some changes required in the Mikros settings.\nTo update these settings, go to Mikros → Edit Settings.";

		private readonly static string mikrosLogoFileName = "MikrosLogo";
		private readonly static string mikrosLogoErrorMessage = "Some Mikros files has been tampered with. Please import Mikros SDK again to resolve this issue.\nNote: It is prohibited to delete or move any Mikros files";

		/// <summary>
		/// Setting the order of this build processor execution.
		/// Note: 'callbackOrder' is a Unity internal variable. Do not alter this.
		/// </summary>
		public int callbackOrder => -1;

		/// <summary>
		/// Unity internal event that invokes during build process.
		/// </summary>
		/// <param name="report">Full build report.</param>
		public void OnPreprocessBuild(BuildReport report)
		{
			CheckMikrosSettings();
			CheckMikrosLogo();
		}

		/// <summary>
		/// Check for correct setup of Mikros Settings.
		/// </summary>
		private void CheckMikrosSettings()
		{
			CheckAsset<MikrosSettings>(mikrosSettingsFileName, mikrosSettingsErrorMessage,
				mikrosAsset => string.IsNullOrEmpty(mikrosAsset.AppGameID) || string.IsNullOrEmpty(mikrosAsset.GetCurrentApiKey()));
		}

		/// <summary>
		/// Check for presence of Mikros Logo in correct path.
		/// </summary>
		private void CheckMikrosLogo()
		{
			CheckAsset<Sprite>(mikrosLogoFileName, mikrosLogoErrorMessage);
		}

		/// <summary>
		/// Check for any Mikros asset at relevant path.
		/// </summary>
		/// <typeparam name="T">Generic Object.</typeparam>
		/// <param name="assetFileName">File name of the Mikros asset.</param>
		/// <param name="errorMessage">Error message to display.</param>
		/// <param name="additionalErrorCondition">Additional conditions to check for Mikros asset.</param>
		private void CheckAsset<T>(string assetFileName, string errorMessage, Func<T, bool> additionalErrorCondition = null) where T : Object
		{
			T mikrosAsset = Resources.Load<T>(assetFileName);
			bool isError = false;
			if(mikrosAsset == null)
				isError = true;
			else if(additionalErrorCondition != null && additionalErrorCondition(mikrosAsset))
				isError = true;

			if(isError)
			{
				EditorUtility.DisplayDialog("Mikros Setup Error", errorMessage, "OK"); // Show a dialog box pointing out the issue
				throw new BuildFailedException(errorMessage); // Throw exceptions during build postprocessing as BuildFailedException, so we don't pretend the build was fine.
			}
		}
	}
}
