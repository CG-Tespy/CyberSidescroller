using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

/// <summary>
/// 2D platform that falls after some time.
/// </summary>
public class FallingPlatform2D : Platform2D
{
	[Tooltip("How long it'll take before this platform falls after being triggered to do so.")]
	[SerializeField] float fallDelay = 					1;
	[Tooltip("If this is true, the platform falls as soon as it loads into the scene.")]
	[SerializeField] bool fallAutomatically = 			false;

	[Tooltip("How fast it falls in meters per second.")]
	[SerializeField] float fallSpeed = 					1f;

	public bool isFalling  								{ get; protected set; }

	float fallTimer;

	[Tooltip("Only falls when an object with one of these tags touches this from above.")]
	[SerializeField] List<string> fallContactTags;
	
	// Use this for initialization
	protected override void Awake () 
	{
		base.Awake();
		rigidbody.isKinematic = 						true;
		// ^ So the platform only falls by this script's algorithm

		if (fallAutomatically)
		{
			isFalling = 								true;
			InvokeRepeating("Fall", fallDelay, Time.deltaTime);
		}
		else
			isFalling = 								false;

	}

	protected override void OnCollisionEnter2D(Collision2D other)
	{
		base.OnCollisionEnter2D(other);
		OnTriggerEnter2D(other.collider);
	}

	protected override void OnTriggerEnter2D(Collider2D otherCollider)
	{
		base.OnTriggerEnter2D(otherCollider);

		// Only consider falling if this isn't already doing so, and if colliding with a ground check

		Check2D groundCheck = 			otherCollider.gameObject.GetComponent<Check2D>();
		bool considerFalling = 			!isFalling && groundCheck != null;
		if (!considerFalling)
			return;

        
		InvokeRepeating("Fall", fallDelay, Time.deltaTime);
		isFalling = 					true;
	}

	void Fall()
	{
		transform.position -= 		new Vector3(0, fallSpeed, 0) * Time.deltaTime;
	}

    


}
