using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

/// <summary>
/// You can go up from below this platform, but not down from above it.
/// 
/// </summary>
public class SemipermeablePlatform2D : Platform2D
{
	Collider2D solidCollider;

	protected override void Awake()
	{
		base.Awake();
		solidCollider = 			GetComponent<Collider2D>();
		// ^ To avoid confusion when the trigger collider handles trigger-entering
	}

	protected override void OnTriggerEnter2D(Collider2D otherCollider)
	{
		/*
		Note that we're using trigger collisions so that the player doesn't have to hit his 
		head against the underside of the platform before being allowed to jump and go 
		through it. 
		 
		The trigger also needs to be big enough for the player to enter, so that 
		the trigger collision executes before a physical collision can. Hence why the 
		TriggerBody part of the prefab has a way bigger collider than the actual
		physical platform.
		*/

		base.OnTriggerEnter2D(otherCollider);
			
		// If the other object is below this, ignore the collision between the solid 
		// collider and the other collider
		float bottomOfOther = 			otherCollider.transform.position.y - 
										(otherCollider.Height() / 2);
		float bottomOfThis = 			transform.position.y - (GetComponent<Collider2D>().Height() / 2);
		bool otherBelowThis = 			bottomOfOther < bottomOfThis;

		if (otherBelowThis)
			Physics2D.IgnoreCollision(solidCollider, otherCollider);
		
	}

	protected override void OnTriggerExit2D(Collider2D otherCollider)
	{
		base.OnTriggerExit2D(otherCollider);

		// Now that the other object is above the solid collider, let collisions between them
		// register again.
		Physics2D.IgnoreCollision(solidCollider, otherCollider, false);
	}
			
	


}
