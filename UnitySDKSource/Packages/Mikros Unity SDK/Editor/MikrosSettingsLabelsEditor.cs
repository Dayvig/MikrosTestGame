using MikrosClient;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEditor;
using UnityEngine;

namespace MikrosEditor
{
	/// <summary>
	/// This script shows how to call upon variables from the "MikrosSettings" Script to make custom fields in the Inspector for these variables.
	/// </summary>
	[CustomEditor(typeof(MikrosSettings))]
	public class MikrosSettingsLabelsEditor : Editor
	{

		private Dictionary<SerializedProperty, string> propertyAndLabelPair = new Dictionary<SerializedProperty, string>();
		private const string MikrosIntroductionWebpage = EditorConstants.DeveloperPage;

		private Color bgColor = new Color(0.3f,0.3f,0.3f,0.3f);

		/// <summary>
		/// Add hyperlink text for Mikros documentation link.
		/// </summary>
		private void AddSuggestionText()
		{
			using (var horizontalScope = new EditorGUILayout.HorizontalScope(GUILayout.Width(1f)))
			{
				var style = GUI.skin.label;
				style.richText = true;
				style.padding = new RectOffset();
				style.fontSize = 18;
				GUILayout.Label(EditorConstants.ToLabel, style);
				string caption = EditorConstants.CaptionLabel;
                var styleLayout = new GUIStyle
                {
                    richText = true,
                    padding = new RectOffset(),
                    fontSize = 18
                };
				if (GUILayout.Button(caption, style))
				{
					Application.OpenURL(MikrosIntroductionWebpage);
				}
				GUILayout.Label(EditorConstants.DocumentationLabel, style);
			}
		}

		/// <summary>
		/// Called every frame in Unity editor.
		/// </summary>
		public override void OnInspectorGUI()
		{
			MikrosHeaderSetupEditor();
			MikrosSetupEditor();
			EditorGUILayout.Space(10);
			MikrosConfigurationEditor();
			EditorGUILayout.Space(10);
			MikrosCrashEditor();
			EditorGUILayout.Space(25);
			MikrosSSOEditor();
			EditorGUILayout.Space(25);

			AddSuggestionText();
			// Apply changes to the serializedProperty - always do this at the end of OnInspectorGUI.
			serializedObject.ApplyModifiedProperties();
		}

		/// <summary>
		///  Editor Method for showing the Mikros logo and Mikros brand name in the Mikros Configuration settings tab.
		/// </summary>
		private void MikrosHeaderSetupEditor()
        {
			Texture settingsLogo = Resources.Load<Sprite>(EditorConstants.MikrosLogoAssetName).texture;
			GUI.DrawTexture(new Rect(10, 10, 50, 50), settingsLogo, ScaleMode.StretchToFill, true, 10.0F);
			GUIStyle headingStyle = new GUIStyle
			{
				fontSize = 38,
				fontStyle = FontStyle.Bold
			};
			//if (ColorUtility.TryParseHtmlString("#86c4c2", out Color txtColor))
				headingStyle.normal.textColor = Color.white;
			GUI.Label(new Rect(65, 12, 60, 60), EditorConstants.Mikros.ToUpper(), headingStyle);
			EditorGUILayout.Space(70);

			var rectPos = EditorGUILayout.BeginHorizontal();
			Handles.DrawLine(new Vector2(rectPos.x - 30, rectPos.y), new Vector2(rectPos.width + 20, rectPos.y));
			EditorGUILayout.Space(5);
			EditorGUILayout.EndHorizontal();
		}

		/// <summary>
		/// Editor Method for showing the Mikros APP id and other necessary key id's that is used for the Mikros Environmental setup.
		/// </summary>
		private void MikrosSetupEditor()
		{
			var rectPos = EditorGUILayout.BeginVertical();
			Handles.DrawSolidRectangleWithOutline(new Rect(rectPos.x - 10, rectPos.y + 5, rectPos.width +10, rectPos.height),bgColor, bgColor);
			EditorGUILayout.Space(5);
			GUIStyle headingStyle = new GUIStyle
			{
				fontSize = 20,
				fontStyle = FontStyle.Bold
			};
			headingStyle.normal.textColor = Color.white;
			GUI.Label(new Rect(15, 90, 60, 60), EditorConstants.MikrosSetupHeader.ToUpper(), headingStyle);

			EditorGUILayout.Space(35);
			EditorGUILayout.PropertyField(serializedObject.FindProperty("appGameID"), new GUIContent(EditorConstants.AppGameIdLabel));
			EditorGUILayout.PropertyField(serializedObject.FindProperty("apiKeyProduction"), new GUIContent(EditorConstants.ApiKeyProductionLabel));
			EditorGUILayout.PropertyField(serializedObject.FindProperty("apiKeyQA"), new GUIContent(EditorConstants.ApiKeyQALabel));
			EditorGUILayout.PropertyField(serializedObject.FindProperty("apiKeyDevelopment"), new GUIContent(EditorConstants.ApiKeyDevLabel));
			EditorGUILayout.PropertyField(serializedObject.FindProperty("apiKeyToUse"), new GUIContent(EditorConstants.ApiKeyLabel));
			EditorGUILayout.EndVertical();
		}

		/// <summary>
		///Editor Method for showing the Mikros Event configutation.
		/// </summary>
		private void MikrosConfigurationEditor()
		{
			var rectPos = EditorGUILayout.BeginVertical();
			Handles.DrawSolidRectangleWithOutline(new Rect(rectPos.x -10, rectPos.y + 5, rectPos.width+10 , rectPos.height), bgColor, bgColor);
			GUIStyle configStyle = new GUIStyle
			{
				fontSize = 20,
				fontStyle = FontStyle.Bold
			};
			configStyle.normal.textColor = Color.white;
			GUI.Label(new Rect(17, 240, 60, 80), EditorConstants.MikrosConfigurationHeader.ToUpper(), configStyle);

			EditorGUILayout.Space(15);
			EditorGUILayout.PropertyField(serializedObject.FindProperty("autoInitializeAtStart"), new GUIContent(EditorConstants.AutoInitializeMikrosLabel));
			EditorGUILayout.PropertyField(serializedObject.FindProperty("autoTrackSession"), new GUIContent(EditorConstants.AutoTrackUserSessionLabel));
			EditorGUILayout.PropertyField(serializedObject.FindProperty("autoTrackMetadata"), new GUIContent(EditorConstants.AutoTrackUserMetadataLabel));
			EditorGUILayout.PropertyField(serializedObject.FindProperty("isEventLogging"), new GUIContent(EditorConstants.EventLoggingLabel));
			EditorGUILayout.PropertyField(serializedObject.FindProperty("isTrackDeviceMemory"), new GUIContent(EditorConstants.MemoryTrackingLabel));			
			EditorGUILayout.EndVertical();
		}

		/// <summary>
		/// Editor Method for showing the Mikros Crash Configuration.
		/// </summary>
		private void MikrosCrashEditor()
		{
			var rectPos = EditorGUILayout.BeginVertical();
			Handles.DrawSolidRectangleWithOutline(new Rect(rectPos.x - 10, rectPos.y + 5, rectPos.width + 10, rectPos.height + 5), bgColor, bgColor);
			GUIStyle crashstyle = new GUIStyle
			{
				fontSize = 20,
				fontStyle = FontStyle.Bold
			};
			crashstyle.normal.textColor = Color.white;
			GUI.Label(new Rect(17, 395, 560, 60), EditorConstants.MikrosCrashReportingHeader.ToUpper(), crashstyle);

			EditorGUILayout.Space(20);
			EditorGUILayout.PropertyField(serializedObject.FindProperty("isCrashReporting"), new GUIContent(EditorConstants.CrashReportingLabel));
			EditorGUILayout.EndVertical();
		}

		private void MikrosSSOEditor()
		{
			var rectPos = EditorGUILayout.BeginVertical();
			Handles.DrawSolidRectangleWithOutline(new Rect(rectPos.x - 10, rectPos.y + 5, rectPos.width + 10, rectPos.height + 5), bgColor, bgColor);
			GUIStyle SSOStyle = new GUIStyle
			{
				fontSize = 20,
				fontStyle = FontStyle.Bold
			};
			SSOStyle.normal.textColor = Color.white;
			GUI.Label(new Rect(17, 480, 560, 60), EditorConstants.MikrosSSOHeader.ToUpper(), SSOStyle);

			EditorGUILayout.Space(40);
			EditorGUILayout.PropertyField(serializedObject.FindProperty("isEnableUserNameSpecialCharacters"), new GUIContent(EditorConstants.EnableUserNameSpecialCharacters));
			EditorGUILayout.EndVertical();
		}
	}
}