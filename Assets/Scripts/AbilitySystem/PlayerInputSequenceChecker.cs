using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;


public class PlayerInputSequenceChecker : System.IDisposable
{
	GameObject player = 				null;
	MonoBehaviour coroutineHandler = 	null;
	// ^ Need these to execute the continuous checking

	// Events
	// Invoked based on the success (or lack thereof) of the sequence being executed.
	public UnityEvent Success 			{ get; protected set; }
	public UnityEvent Failure 			{ get; protected set; }

	PlayerInput[] inputs;

	bool _isChecking = 					false;
	public bool isChecking
	{
		get { return _isChecking; }
		set 
		{
			// If told to stop checking, force stop the coroutine to do just that.
			// If told to start checking, do the same to make sure you're only 
			// running one of the coroutine at a time.
			if (_isChecking != value)
				if (coroutine != null)
				{
					Debug.Log("Stopping input-checking coroutine");
					coroutineHandler.StopCoroutine(coroutine);
				}

			// Start (or restart) the coroutine if asked to check
			if (value == true && isChecking != true)
			{
				Debug.Log("Sequence checker activated");
				_isChecking = 				value;
				coroutine = 			CheckInputs();
				coroutineHandler.StartCoroutine(coroutine);
			}
			
			_isChecking = 				value;

		}
	}

	IEnumerator coroutine = 			null; 	// We may want to stop the checking mid-way

	public PlayerInputSequenceChecker(MonoBehaviour user, PlayerInput[] inputs, bool checkImmediately = false)
	{
		this.player = 					user.gameObject;
		this.inputs = 					inputs;
		isChecking = 					checkImmediately;
		coroutineHandler = 				user;
		Success = 						new UnityEvent();
		Failure = 						new UnityEvent();
	}

	/// <summary>
	/// Goes through the inputs, checking for their execution based on their individual
	/// time limits.
	/// </summary>
	IEnumerator CheckInputs()
	{
		// Make sure we have inputs to work with
		if (inputs == null || inputs.Length == 0)
		{
			Debug.LogWarning("There are no inputs to check for game object " + player.name);
			isChecking = 					false;
			yield break;
		}

		// To keep track of the inputs and execute the countdowns
		int inputIndex = 					0;
		float timer = 						0;

		PlayerInput currentInput = 			null;
		bool infiniteTimeForInput = 		false;

		while (isChecking)
		{
			currentInput = 					inputs[inputIndex];
			timer = 						currentInput.timeLimit;
			infiniteTimeForInput = 			timer < 0;

			// Keep checking for the current input while its time limit isn't up
			while (timer > 0 || infiniteTimeForInput)
			{
				if (!infiniteTimeForInput)
					timer -= 				Time.deltaTime;

				if (currentInput.IsExecuted())
				{
					//Debug.Log("Input success!");
					inputIndex++;
					break;
				}
				else if (Input.anyKeyDown) 
				// Some other input was executed, so reset the sequence-checking and announce failure
				{
					inputIndex = 			0;
					//Debug.Log("Input failure.");
					Failure.Invoke();
					break;
				}

				yield return null;
			}

			// Check if the whole sequence was successfully-executed
			bool didLastInput = 			inputIndex == inputs.Length;
			bool inputSuccess = 			timer > 0 || infiniteTimeForInput;
			bool lastInputSuccess = 		didLastInput && inputSuccess;

			if (lastInputSuccess)
				Success.Invoke();

			// To check for the sequence all over again
			inputIndex = 					0;

			yield return null;


		}

	}

	public void Dispose()
	{
		isChecking = 		false;
		
		if (coroutine != null)
			coroutineHandler.StopCoroutine(coroutine);

		Success.RemoveAllListeners();
		Failure.RemoveAllListeners();
	}

	
}
