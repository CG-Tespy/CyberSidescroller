using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEditor;

[CustomEditor(typeof(ScreenFader))]
public class ScreenFaderEditor : Editor 
{
	//ScreenFader fader;
	float targetOpacity, duration;
	string opacityText = "", durationText = "";

	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI();
		DrawNewElements();
	}

	void DrawNewElements()
	{
		ScreenFader fader = 		target as ScreenFader;

		GUILayoutOption widthLimit = GUILayout.MaxWidth(100);

		// Target Opacity section
		GUILayout.BeginHorizontal();
		GUILayout.Label("Target Opacity");

		opacityText = 		GUILayout.TextField(opacityText, 10, widthLimit);
		float.TryParse(opacityText, out targetOpacity);
		GUILayout.EndHorizontal();

		// Duration section
		GUILayout.BeginHorizontal();
		GUILayout.Label("Duration");

		durationText = 		GUILayout.TextField(durationText, 10, widthLimit);
		float.TryParse(durationText, out duration);
		GUILayout.EndHorizontal();
		
		if (GUILayout.Button("Fade"))
		{
			fader.Fade(duration, targetOpacity, Color.white);
		//	fader = GameObject.FindObjectOfType<ScreenFader>();

			//if (fader != null)
			//	Debug.Log("Fader found!");
		}
	}
}
