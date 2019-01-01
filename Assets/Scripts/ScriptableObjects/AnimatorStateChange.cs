using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

/// <summary>
/// To let people change the state of an animator in the inspector using Scriptable Objects.
/// </summary>
[System.Serializable]
public class AnimatorStateChange
{
	// To customize the state change in the inspector
	[SerializeField] string stateName;
	[SerializeField] AnimatorState stateToChange = 	AnimatorState.Bool;
	[SerializeField] string value;

	/// <summary>
	/// Applies this state change to the passed animator.
	/// </summary>
	public virtual void Execute(Animator animator)
	{
		// May need to make editor scripts to simplify this function somewhere down the road.

		switch (stateToChange)
		{
			case AnimatorState.Bool:
				bool boolVal = 					false;

				if (value.ToLower() == "true")
					boolVal = 					true;

				else if (value.ToLower() != "false")
				{
					// The user needs to properly type true or false in the value field
					string errMessage = 		"Cannot set a bool to anything besides true or false.";
					throw new System.ArgumentException(errMessage);
				}

				animator.SetBool(stateName, boolVal);
				break;
				
			case AnimatorState.Integer:
				int intVal = 			0;

				// Got to be careful!
				bool valIsInt = 		int.TryParse(value, out intVal);

				if (valIsInt)
					animator.SetInteger(stateName, intVal);
				else
				{
					// Let user know they didn't properly type an integer in the value field
					string errMessage = 		"An Animator State Change tried to set an int to a non-int.";
					throw new System.ArgumentException(errMessage);
				}

				break;

			case AnimatorState.Trigger:
				animator.SetTrigger(stateName);
				break;
		}
	}

}

public enum AnimatorState 
{
	Bool, Integer, Trigger
}