using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

/// <summary>
/// For now, just has a number to help the Game Controller tell how far in the level 
/// each spawn point is.
/// </summary>
public class SpawnPoint : MonoBehaviour 
{
	[Tooltip("The higher this is, the further in the level this point is supposed to be.")]
	[SerializeField] int _number;

	public int number 						{ get { return _number; } }


}
