using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

/// <summary>
/// Platform that certain objects can go through under set conditions.
/// </summary>
public class TwoWayPlatform2D : CSS_MonoBehaviour2D, IWalkable 
{
	[System.Serializable]
	public class PassThroughCondition2D
	{
		[System.Serializable]
		public class PassThroughMovement2D 
		{
			public bool up = 			true;
			public bool down = 			false;
			public bool left = 			false;
			public bool right = 		false;
		}

		[Tooltip("Pressing any of these individually allows passing through the platform.")]
		[SerializeField] List<KeyCode> _buttonPresses;
		public List<KeyCode> buttonPresses
		{
			get { return _buttonPresses; }
			protected set { _buttonPresses = value; }
		}

		[Tooltip("Pressing a button on any of these axes allows passing through the platform.")]
		[SerializeField] List<string> _inputAxes;
		public List<string> inputAxes 
		{
			get { return _inputAxes; }
			protected set { _inputAxes = value; }
		}

		[Tooltip("Pressing these simultaneously allows passing through the platform.")]
		[SerializeField] List<KeyCode> _multiButtonPresses;
		public List<KeyCode> multiButtonPresses
		{
			get { return _buttonPresses; }
			protected set { _buttonPresses = value; }
		}

		[Tooltip("Moving through the collider in these directions allows passage.")]
		public PassThroughMovement2D movementDirections;

		
	}

	[SerializeField] PassThroughCondition2D passCondition;
	Collider2D solidCollider;
	// ^To avoid confusion with any trigger colliders associated with the platform

	[SerializeField] Collider2D triggerCollider;


	// Use this for initialization
	protected override void Awake () 
	{
		base.Awake();
		solidCollider = 			GetComponent<Collider2D>();
	}
	

	void HandlePassingThrough(Collider2D otherCollider)
	{
		//Debug.Log(this.name + ": Handling passing through!");
	
		// Non-simultaneous button presses
		foreach (KeyCode key in passCondition.buttonPresses)
		{
			if (Input.GetKey(key))
			{
				Physics2D.IgnoreCollision(solidCollider, otherCollider);
				//Debug.Log("Ignoring collision with: " + otherCollider.name );
				return;
			}
		}
		
		// Non-simultaenous input axes
		foreach (string axis in passCondition.inputAxes)
		{
			if (Input.GetAxis(axis) < 0)
			{
				Physics2D.IgnoreCollision(solidCollider, otherCollider);
				//Debug.Log("Ignoring collision with: " + otherCollider.name );
                //GameController.S.player.NoJump();
				
                //Debug.Log("Ignoring collision due to input axes!");
                return;
			}
		}

		/*
		// Simultaneous button presses
		bool simulButtonPress = 		passCondition.multiButtonPresses.Count > 0;
		foreach (KeyCode key in passCondition.multiButtonPresses)
		{
			if (!Input.GetKey(key))
			{
				simulButtonPress = 		false;
				break;
			}
		}


		if (simulButtonPress)
			Physics2D.IgnoreCollision(solidCollider, otherCollider);

		// TODO: direction other collider is moving
		Rigidbody2D otherRb = otherCollider.GetComponent<Rigidbody2D>();

		if (otherRb == null)
			return;

		//PassThroughCondition2D.PassThroughMovement2D movementDirections = 	passCondition.movementDirections;
		*/

	}

	protected override void OnTriggerEnter2D(Collider2D otherCollider)
	{
		base.OnTriggerEnter2D(otherCollider);
		//Debug.Log(this.name + ": Two-way trigger enter with " + otherCollider.name);

		// Ignore collisions between this and the other object until the collision is exited
		if (triggerCollider.IsTouching(otherCollider))
			Physics2D.IgnoreCollision(solidCollider, otherCollider);

	}

	protected override void OnCollisionStay2D(Collision2D other)
	{
		//Debug.Log(this.name + ": On Collision Stay");
		base.OnCollisionStay2D(other);
		HandlePassingThrough(other.collider);
	}

	protected override void OnTriggerStay2D(Collider2D otherCollider)
	{
		base.OnTriggerStay2D(otherCollider);
		
		
		HandlePassingThrough(otherCollider);
	}

	protected override void OnTriggerExit2D(Collider2D otherCollider)
	{
		base.OnTriggerExit2D(otherCollider);
		//Debug.Log(this.name + ": Two-way trigger exit with " + otherCollider.name);

		// If collisions between this platform and the other collider were being 
		// ignored, then undo that ignorance
		
		if (!triggerCollider.IsTouching(otherCollider))
			Physics2D.IgnoreCollision(solidCollider, otherCollider, false);
			
	}
}
