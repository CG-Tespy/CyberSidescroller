using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TeaspoonTools.Utils;
using System.Linq;

public class MovementHandler : MonoBehaviour
{
	
	[Header("Movement Limiters")]
	[SerializeField] bool _canMove = 				true;
	[SerializeField] bool _canMoveVertically = 		true;
	[SerializeField] bool _canMoveHorizontally = 	true;

	#region Exposes movement limiters

	public bool canMove 
	{
		get { return _canMove; }
		set { _canMove = value; }
	}

	public bool canMoveHorizontally
	{
		get { return _canMoveHorizontally; } 
		set { _canMoveHorizontally = value; }
	}

	public bool canMoveVertically
	{
		get { return _canMoveVertically; }
		set { _canMoveVertically = value; }
	}
	#endregion
	
	[SerializeField] List<MovementAbility2D> _movementAbilities;
	public List<MovementAbility2D> movementAbilities 
	{
		get { return _movementAbilities; }
		protected set { _movementAbilities = value; }
	}

	[SerializeField] List<string> groundTags;
	[SerializeField] List<LayerMask> groundLayers;

	Check2D groundCheck, headCheck; // Helps keep track of and decide movement state
	new Rigidbody2D rigidbody;
	new Collider2D collider;

	bool grounded = 				false;

	void Awake()
	{
		/*
		mover = 		GetComponent<IMover>();

		if (mover == null)
		{
			string message = "Movement Handler needs to work with a behaviour than can move.";
			throw new System.NullReferenceException(message);
		}
		*/
		rigidbody = 				GetComponent<Rigidbody2D>();
		collider = 					GetComponent<Collider2D>();
		InitializeAbilities();
		GetPhysicsChecks();
		ObservePhysicsChecks();
	}
	
	// Update is called once per frame
	void Update () 
	{
		// Don't allow movement while the game is paused or the character can't move
		if (Time.timeScale == 0 || !canMove)
			return;

		UpdateGroundedStatus();
		//grounded = true;
		HandleAbilities();

		Debug.Log("Grounded status: " + grounded);

		//canMoveVertically = 			grounded;
		
	}

	protected virtual void UpdateGroundedStatus()
	{
		// Look through all the colliders touching the ground check, and see if any are walkable
		List<GameObject> walkables = 			new List<GameObject>
												(
												from Collider2D coll in groundCheck.collidersTouching
												where groundLayers.Contains(coll.gameObject.layer) || 
													  groundTags.Contains(coll.gameObject.tag)
												select coll.gameObject
												);
		
		if (walkables.Count == 0)
		{
			grounded = 							false;
			return;
		}

		// If so, linecast down to the ground check. Then look through the hits to see if any are among the 
		// walkables its touching.
		Vector3 justBelowCheck = 				groundCheck.pos + Vector3.down;
		RaycastHit2D[] hits = 					Physics2D.LinecastAll(transform.position, justBelowCheck);

		foreach (RaycastHit2D hit in hits)
		{
			grounded = 							walkables.Contains(hit.transform.gameObject);

			if (grounded)
				return;
		}

	}

	protected virtual void HandleAbilities()
	{
		foreach (MovementAbility2D movementAbility in movementAbilities)
			if (ShouldExecuteAbility(movementAbility))
				movementAbility.Apply(this);
			else
				movementAbility.SetActive(false);
		
	}
	bool ShouldExecuteAbility(MovementAbility2D moveAbility)
	{
		if (!moveAbility.isActive || !canMove)
			return false;

		// Execute the abilities depending on what movement types the mover is capable of 
		// at the time of this func's execution, if the mover can use them.
		MovementAbility2D.MovementType[] moveTypes = 	moveAbility.movementTypes;

		foreach (MovementAbility2D.MovementType moveType in moveTypes)
		{
			switch (moveType)
			{
				case MovementAbility2D.MovementType.horizontal:
					if (!canMoveHorizontally)
						return false;
					break;
				case MovementAbility2D.MovementType.vertical:
					if (!canMoveVertically)
						return false;
					break;

			}
		}

		return true;

	}


	// Helpers
	void InitializeAbilities()
	{
		// Set up movement abilities so they have all the tools they need to do their jobs.
		foreach (MovementAbility2D movementAbility in movementAbilities)
		{
			movementAbility.Init();
			//movementAbility.isActive = 		true;
		}
		
	}

	void GetPhysicsChecks()
	{

		foreach (Check2D check in GetComponentsInChildren<Check2D>())
		{
			if (check.type == Check2D.CheckType2D.ground )
				groundCheck = check;
			
			if (check.type == Check2D.CheckType2D.head)
				headCheck = check;
		}
	}

	void ObservePhysicsChecks()
	{
		//groundCheck.contactEvents.TriggerStay2D.AddListener(UpdateGroundedStatus);
	}

	void UpdateAbilityActives()
	{
		// Activates or deactivates movement abilities based on their types and the movement
		// this allows at the time

		MovementAbility2D.MovementType moveType;

		
	}
	
	
}
