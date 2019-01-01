using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class Block2D : CSS_MonoBehaviour2D, IWalkable
{
	public UnityEvent Activated 				{ get; protected set; }
	public UnityEvent Deactivated 				{ get; protected set; }
	[SerializeField] bool _activated = 			false;
	[SerializeField] GameObject thingToSpawn;
	public bool activated
	{
		get { return _activated; }
		set 
		{
			_activated = 					value; 

			if (activated)
			{
				Activated.Invoke();
				Debug.Log(this.name + " was activated!");
				// Spawn whatever this has to spawn, if it has any
				if (thingToSpawn != null)
				{
					Vector3 spawnPos = 			transform.position;
					spawnPos.y += 				height;

					Instantiate<GameObject>(thingToSpawn, spawnPos, Quaternion.identity);
				}
			}
			else 
				Deactivated.Invoke();
		
		}
	}

	// Use this for initialization
	protected override void Awake () 
	{
		base.Awake();
		Activated = 							new UnityEvent();
		Deactivated = 							new UnityEvent();
		
	}

	/*
	protected virtual void OnCollisionEnter2D(Collision2D other)
	{
		// Only respond to a character touching this if this isn't already activated
		SidescrollerCharacter character = 	other.gameObject.GetComponent<SidescrollerCharacter>();
		if (activated || character == null)
			return;

		// If a character touched this from below (and not the side)...
		Collider2D otherColl = 			other.collider;

		float topOfThis = 				transform.position.y + (height / 2);
		float topOfOther = 				other.transform.position.y + (height / 2);

		float leftSideOfThis = 			transform.position.x - (width / 2);
		float rightSideOfThis = 		transform.position.x + (width / 2);

		float otherXPos = 				other.transform.position.x;

		// ... activate this block and spawn whatever it has to spawn.
		activated = 					(topOfThis > topOfOther) && 
										(otherXPos > leftSideOfThis) &&
										(otherXPos < rightSideOfThis);

		if (activated && thingToSpawn != null)
		{
			
		}
	}
	*/
}
