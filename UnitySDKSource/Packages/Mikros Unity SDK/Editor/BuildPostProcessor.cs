#if UNITY_EDITOR && UNITY_IOS
using System;
using System.IO;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.iOS.Xcode;
using UnityEditor.iOS.Xcode.Extensions;
#endif

namespace MikrosEditor
{
    /// <summary>
    /// Handle iOS build post process.
    /// </summary>
    public class BuildPostProcessor
    {
#if UNITY_EDITOR && UNITY_IOS
        private static string frameworkPath = "Frameworks/com.tatumgames.mikros/Runtime/Plugins/iOS/";
        private static string frameworkName = "mikros_framework_ios.framework";

        /// <summary>
        /// Event raised by Unity Editor for initiation of post build process.
        /// </summary>
        /// <param name="target">The build target.</param>
        /// <param name="path">Path of built project.</param>
        [PostProcessBuildAttribute(1)]
        public static void OnPostprocessBuild(BuildTarget target, string path)
        {
            if (target == BuildTarget.iOS)
            {
                AddCustomFramework(path);
            }
        }

        /// <summary>
        /// Add custom framework.
        /// </summary>
        /// <param name="path">Path of the main built project.</param>
        private static void AddCustomFramework(string path)
        {
            var projectPath = PBXProject.GetPBXProjectPath(path);
            PBXProject project = new PBXProject();
            project.ReadFromString(File.ReadAllText(projectPath));
            string[] targets = {
                project.GetUnityMainTargetGuid(),
                project.TargetGuidByName(PBXProject.GetUnityTestTargetName()),
                project.GetUnityFrameworkTargetGuid()
            };
            foreach (string target in targets)
            {
                // Modify build properties
                project.AddBuildProperty(target, "OTHER_LDFLAGS", "-ObjC");
                project.AddBuildProperty(target, "FRAMEWORK_SEARCH_PATHS", "./Frameworks/Plugins/iOS");
                project.AddBuildProperty(target, "FRAMEWORK_SEARCH_PATHS", "./Libraries/Plugins/iOS");
                project.AddBuildProperty(target, "FRAMEWORK_SEARCH_PATHS", "./Libraries");
                project.AddBuildProperty(target, "FRAMEWORK_SEARCH_PATHS", "./Libraries/com.tatumgames.mikros/Runtime/Plugins/iOS");
                project.AddBuildProperty(target, "FRAMEWORK_SEARCH_PATHS", "./Frameworks/com.tatumgames.mikros/Runtime/Plugins/iOS");
                project.SetBuildProperty(target, "ALWAYS_EMBED_SWIFT_STANDARD_LIBRARIES", "YES");
                project.SetBuildProperty(target, "GCC_ENABLE_OBJC_EXCEPTIONS", "Yes");
                project.UpdateBuildProperty(target, "SWIFT_VERSION", new[] { "5.0" }, null);
                AddFileToEmbedFrameworks(project, target, frameworkPath + frameworkName);
            }
            File.WriteAllText(projectPath, project.WriteToString());
        }

        /// <summary>
        /// Add custom embed frameworks.
        /// </summary>
        /// <param name="project">The main XCode project built from Unity.</param>
        /// <param name="target">The target of project.</param>
        /// <param name="filePath">Path in project of the framework to be added.</param>
        private static void AddFileToEmbedFrameworks(PBXProject project, string target, string filePath)
        {
            string fileGuid = project.FindFileGuidByProjectPath(filePath);
            project.AddFileToBuild(target, fileGuid);
            project.AddFileToEmbedFrameworks(target, fileGuid);
        }
#endif
    }
}