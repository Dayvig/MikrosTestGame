#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace MikrosEditor
{
	/// <summary>
	/// Setup required gradle files in Unity editor.
	/// </summary>
	internal sealed class GradleSetup
	{
		private const string currentMikrosNativeFrameworkVersion = "1.1.0-beta05";
		private const string currentMikrosNativeFrameworkAuthToken = "jp_rip3jgkbjb2fs8l4fba55tdm2f";
		private const string mikrosPackageIdentifier = "com.tatumgames.mikros";

		private const string mikrosStartTag = "// MIKROS Start - ";
		private const string mikrosEndTag = "// MIKROS End - ";

		private static string editorPathToGradleTemplates = "";
		private const string pathToGradleFiles = "Assets/Plugins/Android/";
		private const string pathToSampleGradleFiles = "Packages/" + mikrosPackageIdentifier + "/Runtime/Sample Gradle Files/";

		private const string gradleTemplatePropertiesFileName = "gradleTemplate.properties";
		private static string editorPathToGradlePropertiesTemplate = "";
		private static string fullGradlePropertiesFilePath = Path.Combine(pathToGradleFiles, gradleTemplatePropertiesFileName);
		private static string pathToSampleGradleProperties = Path.Combine(pathToSampleGradleFiles, string.Concat(gradleTemplatePropertiesFileName, ".DISABLED"));
		private static List<string> gradlePropertiesLinesToAdd = new List<string>()
		{
			"#" + mikrosStartTag + "Properties",
			"android.enableJetifier=true",
			"authToken=" + currentMikrosNativeFrameworkAuthToken,
			"#" + mikrosEndTag + "Properties"
		};
		private static List<string> gradlePropertiesLinesOptional = new List<string>
		{
			"android.useAndroidX=true"
		};

		private const string baseProjectGradleTemplateFileName = "baseProjectTemplate.gradle";
		private static string editorPathToBaseProjectGradleTemplate = "";
		private static string fullBaseProjectGradleTemplateFilePath = Path.Combine(pathToGradleFiles, baseProjectGradleTemplateFileName);
		private static string pathToSampleBaseProjectGradle = Path.Combine(pathToSampleGradleFiles, string.Concat(baseProjectGradleTemplateFileName, ".DISABLED"));
		private static List<string> baseProjectGradleLinesToAdd = new List<string>()
		{
			"		" + mikrosStartTag + "Repositories",
			"		maven {",
			"			url \"https://jitpack.io\"",
			"			credentials{ username authToken }",
			"		}",
			"		" + mikrosEndTag + "Repositories"
		};

		private const string mainGradleTemplateFileName = "mainTemplate.gradle";
		private static string editorPathToMainGradleTemplate = "";
		private static string fullMainGradleTemplateFilePath = Path.Combine(pathToGradleFiles, mainGradleTemplateFileName);
		private static string pathToSampleMainGradle = Path.Combine(pathToSampleGradleFiles, string.Concat(mainGradleTemplateFileName, ".DISABLED"));
		private static List<string> mainGradleLinesToAdd = new List<string>()
		{
			mikrosStartTag + "Dependencies",
			"	implementation 'com.github.TATUMGAMES:TG-MIKROS-FRAMEWORK-android:" + currentMikrosNativeFrameworkVersion + "'",
			mikrosEndTag + "Dependencies"
		};

		/// <summary>
		/// Automatically generate all required gradle files for Mikros on first import of Mikros package.
		/// </summary>
		[InitializeOnLoadMethod]
		[MenuItem("Mikros/Native Dependency Setup/Setup All", false, 4)]
		private static void SetupNativeDependencyFiles()
		{
			GenerateMainGradleTemplate();
			GenerateBaseProjectGradleTemplate();
			GenerateGradlePropertiesTemplate();
			AdjustAndroidMinApiLevel();
		}

		/// <summary>
		/// Sets the Android minimum API level according to that of Mikros Native Framework.
		/// </summary>
		[MenuItem("Mikros/Native Dependency Setup/Adjust Android Minimum API Level", false, 0)]
		private static void AdjustAndroidMinApiLevel()
		{
			if((int) PlayerSettings.Android.minSdkVersion < 24)
				PlayerSettings.Android.minSdkVersion = AndroidSdkVersions.AndroidApiLevel24;
		}

		/// <summary>
		/// Generate or modify the "gradleTemplate.properties" file.
		/// </summary>
		[MenuItem("Mikros/Native Dependency Setup/Gradle Properties Setup", false, 3)]
		private static void GenerateGradlePropertiesTemplate()
		{
			CreateTemplate(editorPathToGradlePropertiesTemplate, pathToSampleGradleProperties, fullGradlePropertiesFilePath, gradlePropertiesLinesToAdd,
				existingLines =>
				{
					//does not work from DLL
					//#if UNITY_2020_1_OR_NEWER
					//					string additionalLineToAdd = "android.enableR8=**MINIFY_WITH_R_EIGHT**";
					//					if(!existingLines.Any(line => line.Contains(additionalLineToAdd.Split('=')[0])))
					//						existingLines.Add(additionalLineToAdd);
					//#endif
					if(!gradlePropertiesLinesOptional.All(line => existingLines.Contains(line)))
						gradlePropertiesLinesOptional.ForEach(line => existingLines.Add(line));

					return existingLines.Count;
				});
		}

		/// <summary>
		/// Generate or modify the "baseProjectTemplate.gradle" file.
		/// </summary>
		[MenuItem("Mikros/Native Dependency Setup/Base Project Gradle Setup", false, 2)]
		private static void GenerateBaseProjectGradleTemplate()
		{
			CreateTemplate(editorPathToBaseProjectGradleTemplate, pathToSampleBaseProjectGradle, fullBaseProjectGradleTemplateFilePath, baseProjectGradleLinesToAdd,
				existingLines =>
				{
					int matchIndex = existingLines.FindIndex(item => item.Contains("flatDir"));
					int indexToAdd = matchIndex + 1;
					for(int i = matchIndex; i < existingLines.Count; i++)
					{
						indexToAdd++;
						if(existingLines[i].Contains("}"))
							break;
					}
					return indexToAdd;
				});
		}

		/// <summary>
		/// Generate or modify the "mainTemplate.gradle" file.
		/// </summary>
		[MenuItem("Mikros/Native Dependency Setup/Main Gradle Setup", false, 1)]
		private static void GenerateMainGradleTemplate()
		{
			CreateTemplate(editorPathToMainGradleTemplate, pathToSampleMainGradle, fullMainGradleTemplateFilePath, mainGradleLinesToAdd,
				existingLines =>
				{
					int matchIndex = existingLines.FindIndex(item => item.Contains("**DEPS**"));
					int indexToAdd = matchIndex;
					return indexToAdd;
				});
		}

		/// <summary>
		/// Generic function to generate each required .gradle file.
		/// </summary>
		/// <param name="editorTemplateFilePath">Path of gradle related files in Unity installation location.</param>
		/// <param name="sampleFilePath">Path of sample gradle related files provided with Mikros.</param>
		/// <param name="actualFilePath">Path of actual gradle related files in Plugins folder.</param>
		/// <param name="linesToAdd">Lines to be added in the specified file.</param>
		/// <param name="getLinesInsertIndexOperation">Delegate to determine location of the lines to be added in file.</param>
		private static void CreateTemplate(string editorTemplateFilePath, string sampleFilePath, string actualFilePath, List<string> linesToAdd, Func<List<string>, int> getLinesInsertIndexOperation)
		{
			DetermineTemplateAssetPath();
			CheckAndCreateAndroidPluginDirectory();
			bool editorTemplateFilePresent = File.Exists(editorTemplateFilePath);
			bool sampleFilePresent = File.Exists(sampleFilePath);
			bool actualFilePresent = File.Exists(actualFilePath);
			if(editorTemplateFilePresent || sampleFilePresent || actualFilePresent)
			{
				try
				{
					StreamReader streamReader = null;
					if(actualFilePresent)
					{
						streamReader = new StreamReader(actualFilePath);
					}
					else if(editorTemplateFilePresent)
					{
						streamReader = new StreamReader(editorTemplateFilePath);
					}
					else
					{
						streamReader = new StreamReader(sampleFilePath);
					}

					List<string> linesInExistingFile = new List<string>();
					while(!streamReader.EndOfStream)
					{
						string tempLine = streamReader.ReadLine();
						linesInExistingFile.Add(tempLine);
					}
					streamReader.Close();
					streamReader.Dispose();
					if(linesToAdd.All(line => linesInExistingFile.Contains(line)))
						return;

					DeleteExistingMikrosDependency(ref linesInExistingFile);

					int indexToAdd = getLinesInsertIndexOperation(linesInExistingFile);

					linesInExistingFile.InsertRange(indexToAdd, linesToAdd);

					StreamWriter streamWriter = new StreamWriter(actualFilePath);
					for(int i = 0; i < linesInExistingFile.Count; i++)
					{
						streamWriter.WriteLine(linesInExistingFile[i]);
					}
					streamWriter.Flush();
					streamWriter.Close();
					AssetDatabase.Refresh();
				}
				catch(OutOfMemoryException e)
				{
					Debug.LogError("File generation Failed!\nException detail: " + e.Message);
				}
				catch(IOException e)
				{
					Debug.LogError("File generation Failed!\nException detail: " + e.Message);
				}
				catch(Exception e)
				{
					Debug.LogError("File generation Failed!\nException detail: " + e.Message);
				}
			}
			else
			{
				Debug.LogError("File generation Failed!");
			}
		}

		/// <summary>
		/// Delete any previous Mikros dependency already present in file.
		/// </summary>
		/// <param name="linesInExistingFile">All lines in existing file.</param>
		private static void DeleteExistingMikrosDependency(ref List<string> linesInExistingFile)
		{
			while(linesInExistingFile.Any(item => item.Contains(mikrosStartTag) || item.Contains(mikrosEndTag)))
			{
				int deleteStartIndex = linesInExistingFile.FindIndex(item => item.Contains(mikrosStartTag));
				int deleteEndIndex = -1;
				if((deleteStartIndex + 1) >= 0 && deleteStartIndex < linesInExistingFile.Count)
					deleteEndIndex = linesInExistingFile.FindIndex(deleteStartIndex + 1, item => item.Contains(mikrosEndTag));

				if(deleteStartIndex >= 0 && deleteEndIndex > 0 && deleteEndIndex > deleteStartIndex)
				{
					linesInExistingFile.RemoveRange(deleteStartIndex, deleteEndIndex - deleteStartIndex + 1);
				}
				else if(deleteStartIndex == -1 && deleteEndIndex == -1)
				{
					return;
				}
				else
				{
					throw new Exception("Irregular MIKROS tags detected");
				}
			}
		}

		/// <summary>
		/// Determine the path of the template asset to be used.
		/// </summary>
		private static void DetermineTemplateAssetPath()
		{
			string basePath = "";
			if(Application.platform == RuntimePlatform.WindowsEditor)
				basePath = EditorApplication.applicationContentsPath;
			else if(Application.platform == RuntimePlatform.OSXEditor)
				basePath = Path.GetDirectoryName(EditorApplication.applicationPath);

			editorPathToGradleTemplates = Path.Combine(basePath + "/", "PlaybackEngines/AndroidPlayer/Tools/GradleTemplates");
			editorPathToGradlePropertiesTemplate = Path.Combine(editorPathToGradleTemplates + "/", gradleTemplatePropertiesFileName);
			editorPathToBaseProjectGradleTemplate = Path.Combine(editorPathToGradleTemplates + "/", baseProjectGradleTemplateFileName);
			editorPathToMainGradleTemplate = Path.Combine(editorPathToGradleTemplates + "/", mainGradleTemplateFileName);
		}

		/// <summary>
		/// Check if the Android plugin folder exists in project and creates it, if not found.
		/// </summary>
		private static void CheckAndCreateAndroidPluginDirectory()
		{
			if(!Directory.Exists(pathToGradleFiles))
				Directory.CreateDirectory(pathToGradleFiles);
		}
	}
}
#endif