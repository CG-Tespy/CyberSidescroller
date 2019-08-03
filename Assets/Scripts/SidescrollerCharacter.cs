using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TeaspoonTools.Utils;
using System.Linq;
using UnityEngine.SceneManagement;

public class SidescrollerCharacter : 	CSS_MonoBehaviour2D, IHasHP, IDamageable<float>, 
										IPausable, IClimber, IHealable<float>, ICanMoveHandler
{
	public UnityEvent Died 						{ get; protected set; }
	// IPausable
	public UnityEvent Paused 					{ get; protected set; }
	public UnityEvent Unpaused 					{ get; protected set; }
	public bool isPaused 						{ get; protected set; }

    public SpawnPoint spawnPoint;
	public static SpawnPoint latestSpawnPoint 		{ get; set; }

    public float stageBottom, stageLeftBound, stageRightBound;

    // IDamageable
    public UnityEvent TookDamage 				{ get; protected set; }

	// IClimber
	public bool isClimbing 						{ get; protected set; }
	[SerializeField] bool _canClimb = false;
	public bool canClimb 
	{
		get { return _canClimb; }
		set { _canClimb = value; }
	}

	// Other fields and properties
	public UnityEvent Jumped 					{ get; protected set; }
	
	Check2D groundCheck;
	Check2D headCheck;
	[SerializeField] FloatReference _hp;
	//[SerializeField] FloatReference _maxHp;
	[SerializeField] bool _canJump = 			true;
	[SerializeField] bool _isInvincible = 		false;
	[SerializeField] float forceDivisor = 		5;
	[Tooltip("How long in seconds invincibility frames last.")]
	[SerializeField] float invinTime = 			2;

	[SerializeField] float _moveSpeed = 		5;
	[SerializeField] float _maxSpeed = 			15;
	[SerializeField] Vector3 jumpForce;
	public float moveSpeed 						{ get { return _moveSpeed; } }
	public bool canJump							
	{
		get { return _canJump; } 
		set { _canJump = value; }
	}

	public float hp 			
	{ 
		get { return _hp.value; }
		protected set 
		{ 
			_hp.value = 		value; 

			// Keep the HP from going below 0 or above the max
			if (hp < 0)
				hp = 			0;
			if (hp > maxHp)
				hp = 			maxHp;
		}
	}
	public float maxHp 			
	{ 
		get { return _hp.maxValue; }
		protected set 
		{ 
			_hp.maxValue = 		value; 

			// Keep max hp from being lower than 0 or current HP
			if (maxHp < 0)
				maxHp = 		0;

			if (maxHp < hp)
				hp = 			maxHp;
		}
	}

	public Animator animator 					{ get; protected set; }
	public bool isInvincible
	{
		get { return _isInvincible; }
		set { _isInvincible = value; }
	}

	bool _canMove = true;
	public bool canMove
	{
		get { return _canMove; }
		set { _canMove = value; }
	}
	bool _canMoveHorizontally = true;
	public bool canMoveHorizontally 
	{ 
		get
		{
			if (!canMove)
				return false;
			else
				return _canMoveHorizontally;
		}
		set { _canMoveHorizontally = value; }
	}
	bool _canMoveVertically = true;
	public bool canMoveVertically
	{
		get 
		{
			if (!canMove)
				return false;
			else
				return _canMoveVertically;
		}
		set { _canMoveVertically = value; }
		
	}

	public bool onGround 					{ get; protected set; }
	new public float height
	{
		get { return spriteRenderer.bounds.size.y; }
	}

	
	float invinTimer = 			0;
	IEnumerator jumpCoroutine = null;
	float baseGravityScale = 0;

	SpriteRenderer beingClimbed = null;

	void OnApplicationQuit()
	{
		this.hp = 											this.maxHp;
	}
	
	// Use this for initialization
	protected override void Awake () 
	{
		base.Awake();
		Died = 									new UnityEvent();
		Paused = 								new UnityEvent();
		Unpaused = 								new UnityEvent();
		isPaused = 								false;
		TookDamage = 							new UnityEvent();

		animator = 								GetComponent<Animator>();
		Jumped = 								new UnityEvent();
		canJump = 								true;
		isClimbing = 							false;
		canClimb = 								true;
		baseGravityScale = 						rigidbody.gravityScale;

		if (latestSpawnPoint == null)
			latestSpawnPoint = 					spawnPoint;
		//SetupChecks();
	}

	protected virtual void Start()
	{
		SetupChecks();
	}

	void SetupChecks()
	{
		Check2D[] checks = 						GetComponentsInChildren<Check2D>();
		foreach (Check2D check in checks)
		{
			switch (check.type)
			{
				case Check2D.CheckType2D.ground:
					groundCheck = check;
					break;
				case Check2D.CheckType2D.head:
					headCheck = check;
					break;
			}
		}

		// Need to respond to what the checks end up in contact with to decide the 
		// state of this character
		groundCheck.contactEvents.TriggerEnter2D.AddListener(GroundCollisionEnter);
		groundCheck.contactEvents.TriggerExit2D.AddListener(GroundCollisionExit);
		headCheck.contactEvents.TriggerEnter2D.AddListener(HeadCollisionEnter);
	}
	
	// Update is called once per frame
	protected override void Update() 
	{
		base.Update();
		
		if (isPaused)
		{
			canMove = 		false;
			return;
		}

		HandleRespawning();
        HandleInvincibility();
		UpdateGroundedStatus();
		HandleMovement();

		//Debug.Log(this.name + " can move horizontally: " + canMoveHorizontally);

	}

	/// <summary>
	/// Try to have the player take damage, returning true or false depending on whether it 
	/// was successful.
	/// </summary>
	public virtual bool TakeDamage(float damageToTake, bool triggerInvin = true)
	{

		if (isInvincible)
			return false;

		hp -= 				damageToTake;
        animator.SetTrigger("Hurt");
        
        TookDamage.Invoke();

		// If invincibility is to be applied, trigger the timer for it
		if (triggerInvin)
		{
			isInvincible = 	true;
			invinTimer = 	invinTime;
		}

        if (hp <= 0) {
            Died.Invoke();
            Respawn();
        }

		return true;
	}

	public virtual bool TakeHealing(float healingToTake)
	{
		hp += 			healingToTake;
		return true;
	}

	protected virtual void HandleMovement()
	{		
		if (!canMove)
			return;

		if (canMoveHorizontally)
			HandleWalking();
		
		if (canMoveVertically)
		{
			HandleJumping();
			HandleClimbing();
		}

		HandleTurning();
			
	}

	protected virtual void GroundCollisionEnter(Collider2D other)
	{
		// Check if the other object is walkable, be it in its tag or one of its
		// components implementing IWalkable. If so...
		bool touchedWalkable = 					(other.CompareTag("Walkable") || 
												other.GetComponent<IWalkable>() != null) &&
												groundCheck.collider.IsTouching(other);

		// ... Let the player jump, provided they're above the walkable
		float playerY = 						groundCheck.transform.position.y;
		float walkableY = 						other.transform.position.y;
		bool aboveWalkable = 					playerY > walkableY;

		if (touchedWalkable && aboveWalkable)
		{
			//Debug.Log("Touched walkable!");
			canJump = 					true;
			onGround =					true;
			canMoveHorizontally = 		true;
			isClimbing = 				false;
			//animator.SetBool("Climbing", isClimbing);
			rigidbody.gravityScale = 	baseGravityScale;

            animator.SetBool("Ground", true);
			animator.SetBool("Climbing", false);
			animator.SetBool("MovingNClimbing", false);

			
			if (jumpCoroutine != null && collider.IsTouching(other))
			{
				Debug.Log(this.name + ": Stopping jump coroutine!");
				StopCoroutine(jumpCoroutine);
			}
			
		}
	}

	protected virtual void GroundCollisionExit(Collider2D other)
	{
		// Check if the other object is walkable. If so...
		//Debug.Log(this.name + ": in GroundCollisionExit");

		bool gotOffWalkable = 					(other.CompareTag("Walkable") || 
												other.GetComponent<IWalkable>() != null) &&
												!groundCheck.collider.IsTouching(other);

		if (!gotOffWalkable)
			return;

		// ... disable the player's jump, unless the player is touching a 
		// different walkable object.
		foreach (Collider2D coll in collidersTouching)
		{
			if (coll == null)
				break;

			// Make sure that the current collider can collide with this one based on the 
			// physics settings.
			bool collIsWalkable = 				coll.CompareTag("Walkable") || 
												coll.GetComponent<IWalkable>() != null;
			if (collIsWalkable && collider.IsTouching(coll))
				return;
		}


		if (gotOffWalkable)
		{
			Debug.Log(this.name + " got off a walkable object");
			if (!isClimbing)
				canJump = 					false;
			onGround = 					false;
        	animator.SetBool("Ground", false);
			
		}
    }

	protected virtual void HeadCollisionEnter(Collider2D other)
	{
		//Debug.Log("Head check trigger enter!");
		Block2D block = 					other.GetComponent<Block2D>();
		bool hitABlock = 					block != null;
		
		if (hitABlock)
		{
			// Only activate the block if the head check is below the bottom
			// of the block, and it's not already activated
			if (!block.activated)
			{
				float bottomOfBlock = 			block.transform.position.y - (block.height / 2f);
				block.activated = 				(headCheck.transform.position.y <= bottomOfBlock);
			}
		}
	}


	protected override void OnTriggerEnter2D(Collider2D other)
	{
		base.OnTriggerEnter2D(other);

		// For respawning
		if (other.CompareTag("ForceRespawn"))
		{
			Respawn();
			return;
		}

        if (other.tag == "SpawnPoint")
        {
			SpawnPoint otherPoint = 		other.gameObject.GetComponent<SpawnPoint>();

			if (latestSpawnPoint == null || otherPoint.number > latestSpawnPoint.number) // Found the latest checkpoint
			{
				latestSpawnPoint = 			otherPoint;
			}
        }

        // When this touches a pickup, pick it up
        Pickup2D pickup = 				other.GetComponent<Pickup2D>();

		if (pickup != null)
		{
			pickup.PickUp(this);
		}
	}

	protected virtual void HandleInvincibility()
	{
		// Have the player sprite flash while invincible
		if (isInvincible)
		{
			spriteRenderer.enabled = 	!spriteRenderer.enabled;
			invinTimer -= 				Time.deltaTime;
		}

		// Have the player's sprite go back to normal when time's up
		if (invinTimer <= 0)
		{
			isInvincible = false;
			spriteRenderer.enabled = 	true;
		}
	}

	public virtual void Pause()
	{
		isPaused = 				true;
		Paused.Invoke();

		// Stop registering collisions
		rigidbody.Sleep();
	}

	public virtual void Unpause()
	{
		isPaused = 				false;
		Unpaused.Invoke();

		// Register collisions again
		rigidbody.Sleep();
	}

	/// <summary>
	/// For up to some fraction of a second, the player holding down the jump button can increase
	/// their jump height.
	/// </summary>
	IEnumerator AdjustableHeightJump(float jumpTimeLimit = 0.3f)
	{
		//Debug.Log(this.name + ": Adjustable height jump!");
		float frameRate = 			1f / Time.deltaTime;
		int timeWindow = 			(int)(frameRate * jumpTimeLimit);

		// Keep the jump heights consistent
		Vector2 newVel = 			rigidbody.velocity;
		newVel.y = 					(jumpForce.y / forceDivisor) * Time.deltaTime;
		rigidbody.velocity = 		newVel;

		for (int i = 0; i < timeWindow; i++)
		{
			if (Input.GetButton("Jump"))
			{
				//Debug.Log(this.name + ": Jumping!");
				rigidbody.AddForce(new Vector2(jumpForce.x, jumpForce.y / forceDivisor));
			}
			else
			{
				jumpCoroutine = 	null;
				yield break;
			}

			yield return null;
		}

		jumpCoroutine = 			null;
	}

    protected virtual void Respawn()
    {
        hp = maxHp;
        transform.position = latestSpawnPoint.transform.position;
    }

    void HandleWalking()
    {
        float xMovement = Input.GetAxis("Horizontal");

        if (xMovement != 0)
        {
            // Do the walking animation
            //Debug.Log(this.name + " x movement: " + xMovement);
			/* Sprite-flipping is handled in the HandleTurning function.
            if (xMovement > 0) {
                gameObject.GetComponent<SpriteRenderer>().flipX = false;
            }
            else if (xMovement < 0) {
                gameObject.GetComponent<SpriteRenderer>().flipX = true;
            }
			*/
            animator.SetBool("Walking", onGround);
            //Vector3 newScale = 		transform.localScale;
        }
        else
        { animator.SetBool("Walking", false); }

		// To avoid jitter, just alter the velocity of the rigidbody rather than 
		// just updating the transform position
		Vector2 newVel = 		rigidbody.velocity;
		newVel.x =	 			xMovement * moveSpeed;
		rigidbody.velocity = 	newVel;
		//Vector2 newPos = 		rigidbody.position + new Vector2 (xMovement * moveSpeed * Time.deltaTime, 0);
		//rigidbody.MovePosition(newPos);
		
	}

	void HandleJumping()
	{
		if (Input.GetButtonDown("Jump") && canJump && (TouchingAndAboveWalkable() || TouchingClimbable()) )
		{
	
			// In case you need to jump off a climbable that may be keeping you
			// from moving horizontally
			if (isClimbing)
			{
				canMoveHorizontally = 		true;
				rigidbody.gravityScale = 	baseGravityScale;
				//Debug.Log("Player jumped and no longer climbing");
				isClimbing = 				false;
				animator.SetBool("Climbing", isClimbing);
				
            }

			canJump = 					false;
			// ^ avoid infinite jump glitch

			// The actual jumping starts here
			jumpCoroutine = 		AdjustableHeightJump();
			StartCoroutine(jumpCoroutine);
			
			Jumped.Invoke();
		}
	}

	protected virtual void HandleClimbing()
	{
        float vMovement = 					Input.GetAxis("Vertical");

		if (isClimbing && TouchingClimbable())
			MoveAlongClimbable();
		else
		{
			// Start climbing if the player tries to
			bool climb = 					AlignWithClimbable();

            if (climb)
            {
                //Debug.Log("Started climbing");
                MoveAlongClimbable();
                isClimbing = 				true;
				
                onGround = 					false;
                canJump = 					true;
                rigidbody.gravityScale = 	0;
                canMoveHorizontally = 		false;
                animator.SetBool("Climbing", true);
				animator.SetBool("MovingNClimbing", vMovement != 0);
				animator.SetBool("Ground", false);
			}
			else
			{
				Debug.Log("Player not climbing.");
				isClimbing = 				false;
				rigidbody.gravityScale = 	baseGravityScale;
				animator.SetBool("Climbing", false);
				animator.SetBool("MovingNClimbing", false);
				
			}

		}

	}

	protected virtual void MoveAlongClimbable()
	{
		float vMovement = 		Input.GetAxis("Vertical");
		Vector2 newVel = 		rigidbody.velocity;
		newVel.y = 				moveSpeed * vMovement;
		newVel.x = 				0;
		//Vector3 newPos = 		rigidbody.position + new Vector2(0, vMovement * moveSpeed * Time.deltaTime);
		//rigidbody.MovePosition(newPos);
		rigidbody.velocity = newVel;

		// Keep the climbing animation playing, pause it if staying still
		animator.SetBool("Climbing", true);
        //animator.SetBool("MovingNClimbing",true);
        if (vMovement != 0)
        {
            animator.SetBool("MovingNClimbing", true);
        }
        if (vMovement == 0)
        {
            animator.SetBool("MovingNClimbing", false);
        }
        /*bool moving = 			vMovement != 0;
		if (moving)
			animator.speed = 	1;
		else
			animator.speed = 	0;*///problems with animation freezing



    }

	/// <summary>
	/// Tries to align this object with a climbable object this is touching, based on 
	/// the vertical movement axis.
	/// </summary>
	/// <returns>Returns true if successful, false otherwise.</returns>
    bool AlignWithClimbable()
	{
		// Helps decide which climbable to align to.
		Transform toAlignWith = 		null;
		float vMovement = 				Input.GetAxis("Vertical");
		if (vMovement == 0) // For when the player isn't trying to climb anything
			return false;

		bool alignWithAbove = 			vMovement > 0;
		bool alignWithBelow = 			vMovement < 0;

		// Get all climbables this is touching. We're using their sprite renderers here 
		// since their colliders mess up the spacial relation-calculations.
		SpriteRenderer[] climbables = 
										(from Collider2D coll in collidersTouching
										where coll.CompareTag("Climbable") || 
										coll.GetComponent<IClimbable2D>() != null
										select coll.GetComponent<SpriteRenderer>()).ToArray();	

		
		// Go through the climbables, and try to find one to align this with. Do it based on 
		// their positions relative to this object, as well as the vertical input.

		
		float climbableY = 				0;
		float thisY = 					transform.position.y;

		foreach (SpriteRenderer climbable in climbables)
		{
			climbableY = 					climbable.transform.position.y;
			bool aboveClimbable = 			thisY > climbableY;
			bool belowClimbable = 			thisY < climbableY;

			if ( (alignWithAbove && belowClimbable) ||
			     (alignWithBelow && aboveClimbable))
			{
				toAlignWith = 				climbable.transform;
				break;
			}
		
		}
		/*
		float topOfClimbable = 			0;
		float topOfThis = 				spriteRenderer.UpperEdge();

		foreach (SpriteRenderer climbable in climbables)
		{
			topOfClimbable = 				climbable.UpperEdge();
			bool aboveClimbable = 			topOfThis > topOfClimbable;
			bool belowClimbable = 			topOfThis < topOfClimbable;

			if ( (alignWithAbove && belowClimbable) ||
			     (alignWithBelow && aboveClimbable))
			{
				toAlignWith = 				climbable.transform;
				break;
			}
		
		}
		*/
		// If one was found to align with, then do so before reporting success or failure.
		if (toAlignWith != null)
		{
			Vector3 newPos = 				transform.position;
			newPos.x = 						toAlignWith.position.x;
			

			// When player's going down, move them to avoid a glitch with the two-way
			// platforms. And to help avoid them looking like they're climbing on
			// air.
			if (alignWithBelow)
				newPos.y -= 				collider.Height();



			transform.position = 			newPos;

			//beingClimbed = 					toAlignWith.GetComponent<SpriteRenderer>();
		}

		return toAlignWith != null;
	}

	

	bool TouchingClimbable()
	{
		Collider2D[] climbablesTouched = 		
										(from Collider2D coll in collidersTouching
										where coll.CompareTag("Climbable") && 
										coll.IsTouching(this.collider)
										select coll).ToArray();

		//Debug.Log("Player touching climbable: " + (climbablesTouched.Length > 0));
		return climbablesTouched.Length > 0;
	}

	void HandleRespawning()
	{
		/*
		if (transform.position.y < stageBottom)
            Respawn();
        
        else if (transform.position.x > stageRightBound)
            Respawn();
    
        else if (transform.position.x < stageLeftBound)
            Respawn();
			*/
	}

	void UpdateGroundedStatus()
	{
		if (onGround)
		{
			rigidbody.gravityScale = 	baseGravityScale;
			//canMoveHorizontally = 		true;
		}
	}

	void HandleTurning()
	{
		// Keep the player facing a certain way based on the horizontal axis
		float hMovement = 				Input.GetAxis("Horizontal");

		if (hMovement == 0)
			return;

		// To keep child objects aligned, flip the x scale rather than the sprite.
		
		bool facingLeft = 				hMovement < 0;
		Vector3 newLocalScale = 		transform.localScale;
		newLocalScale.x = 				Mathf.Abs(newLocalScale.x);

		if (facingLeft)
			newLocalScale.x *= 			-1;

		transform.localScale = 			newLocalScale;
		
        //spriteRenderer.flipX = 			hMovement < 0;
    }

	bool TouchingWalkable()
	{
		// Using a for loop to avoid an InvalidOperation exception
		Collider2D currentColl = 		null;
		for (int i = 0; i < collidersTouching.Count; i++)
		{
			currentColl = 				collidersTouching[i];
			if (currentColl.CompareTag("Walkable") || currentColl.GetComponent<IWalkable>() != null)
				return true;
		}

		return false;
	}

	bool TouchingAndAboveWalkable()
	{
		//throw new System.NotImplementedException("Implementation incomplete");

		Transform walkable = 	null;

		foreach (Collider2D coll in collidersTouching)
		{
			if (coll.CompareTag("Walkable") || coll.GetComponent<IWalkable>() != null)
			{
				walkable = 		coll.transform;
				break;
			}
		}

		if (walkable != null)
		{
			//Renderer walkableRenderer = 		walkable.GetComponent<Renderer>();
			float bottomOfThis = 				transform.position.y - (spriteRenderer.Height() / 2);
			float walkableY = 					walkable.position.y;
			// ^ The walkable might not have a sprite renderer, so I had to settle for this

			return (bottomOfThis >= walkableY);
		}

		return false;
	}
}
