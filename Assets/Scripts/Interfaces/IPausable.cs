using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

namespace TeaspoonTools.Utils
{
	/// <summary>
	/// Interface for implementing event-based systems for pausing and unpausing
	/// various parts of your game.
	/// </summary>
	public interface IPausable
	{

		UnityEvent Paused 		{ get; }
		UnityEvent Unpaused 	{ get; }
		bool isPaused 			{ get; }
		void Pause();
		void Unpause();
		
	}
}