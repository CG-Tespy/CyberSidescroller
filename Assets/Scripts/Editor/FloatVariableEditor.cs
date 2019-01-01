using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEditor;

[CustomEditor(typeof(FloatVariable))]
public class FloatVariableEditor : Editor
{
	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI();

		// Add a button that sets the value to its default

		if (GUILayout.Button("Reset to Default"))
		{
			FloatVariable variable = 		target as FloatVariable;
			variable.value = 				variable.defaultValue;
		}
	}
}
