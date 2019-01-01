using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

/// <summary>
/// Helper class for setting up flexible input sequences.
/// </summary>
[System.Serializable]
public class PlayerInput
{
	public static readonly List<InputType> axisTypes = new List<InputType>
	{
		InputType.axis, InputType.axisPositive, InputType.axisNegative,
		
	};

	public static readonly List<InputType> axisDownTypes = new List<InputType>
	{
		InputType.axisDown, InputType.axisDownPositive, InputType.axisDownNegative,

	};

	public string name; 				// To make things more readable in the Inspector

	public enum InputType
	{
		axis, axisDown,
		axisPositive, axisNegative, 
		axisDownPositive, axisDownNegative,
		hardware, hardwareDown, 
		
	}

	[Tooltip("Time in seconds to execute this input. Negative value means infinite time.")]
	public float timeLimit = 			1;
	
	public InputType type;
	public string axis;
	public KeyCode hardwareInput = 		KeyCode.None;
	public float postDelay;

	// Ranges from -1 to 1 depending on this input being an axis type and what was inputted during the 
	// frame this was accessed from
	public float axisValue
	{
		get 
		{
			if (string.IsNullOrEmpty(axis))
				return 0;

			bool typeIsAxis = 			axisTypes.Contains(type);
			bool typeIsAxisDown = 		axisDownTypes.Contains(type);

			if (typeIsAxis)
				return Input.GetAxis(axis);

			else if (typeIsAxisDown)
			{
				bool axisDown = 		Input.GetButtonDown(axis);

				if (axisDown)
					return Input.GetAxis(axis);
				else
					return 0;
			}

			return 0;
		}
	}

	/// <summary>
	/// Returns true or false depenidng on whether this input was executed during the frame 
	/// this method was called.
	/// </summary>
	public virtual bool IsExecuted()
	{
		// Check for the axis types, especially if we need to check for a particular axis sign.
		bool anyAxis = 				type == InputType.axis || type == InputType.axisDown;
		bool positiveAxis = 		type == InputType.axisPositive || type == InputType.axisDownPositive;
		bool negativeAxis = 		type == InputType.axisNegative || type == InputType.axisDownNegative;

		float axVal = 				axisValue; // caching the value

		if (anyAxis)
			return axVal != 0;
		else if (positiveAxis)
			return axVal > 0;
		else if (negativeAxis)
			return axVal < 0;

		// At this point, the type must be hardware, so check for that. And make sure that KeyCode.None returns 
		// true when there is no keyboard input (apparently Unity didn't design it that way to begin with)
		bool checkForNone = 		hardwareInput == KeyCode.None;

		if (type == InputType.hardware)
			if (checkForNone)
				return !Input.anyKey;
			else
				return Input.GetKey(hardwareInput);

		else if (type == InputType.hardwareDown)
			if (checkForNone)
				return !Input.anyKeyDown;
			else
				return Input.GetKeyDown(hardwareInput);

		return false;

	}

	
}
