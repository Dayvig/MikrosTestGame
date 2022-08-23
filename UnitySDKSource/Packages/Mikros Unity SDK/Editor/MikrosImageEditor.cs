using MikrosClient.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.UI;
using UnityEngine;

namespace MikrosEditor
{
	/// <summary>
	/// Editor class for Mikros Image display in Inspector.
	/// </summary>
	[CustomEditor(typeof(MikrosImage)), CanEditMultipleObjects]
	internal sealed class MikrosImageEditor : ImageEditor
	{
		/// <summary>
		/// Called every frame in Unity editor.
		/// </summary>
		public override void OnInspectorGUI()
		{
			serializedObject.Update();
			using(new EditorGUI.DisabledScope(true))
			{
				SpriteGUI();
				AppearanceControlsGUI();
			}
			NativeSizeButtonGUI();
			serializedObject.ApplyModifiedProperties();
		}
	}
}
