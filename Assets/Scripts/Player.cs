using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class Player : SidescrollerCharacter
{
	[SerializeField] CSSInventory _inventory;
	[SerializeField] public Gun2D _gun;

    //SpriteRenderer playerRender; // this already has a spriteRenderer field called spriteRenderer

	public bool isFiring 									{ get; protected set; }
	[HideInInspector] public bool canShoot = 				true;
    [HideInInspector] public bool UpShoot =        			false;
	[HideInInspector] public bool beingDragged = 			false;

	public CSSInventory inventory 							{ get { return _inventory; } }

	public Gun2D gun 										{ get { return _gun; } }

	Rigidbody2DSaveState rbState;

	// Use this for initialization
	protected override void Awake () 
	{
		base.Awake();
		
		isFiring = 		false;

        //playerRender = 	this.gameObject.GetComponent<SpriteRenderer>();

		if (gun != null)
			gun.Init(this);
		
		else
			throw new System.NullReferenceException(this.name + " needs a Gun.");
	}
	
	// Update is called once per frame
	protected override void Update () 
	{
		if (isPaused)
			return;

		base.Update();
		PositionGun();
		HandleShooting();
		
		if (onGround && !isFiring && !beingDragged)
		{
			canJump = 						true;
			canMoveHorizontally = 			true;
		}

		UpdateAnimatorStates();
	}

	protected override void MoveAlongClimbable()
	{
		if (isFiring)
		{
			rigidbody.velocity = 			Vector2.zero;
			return;
		}

		base.MoveAlongClimbable();
	}

	protected virtual void HandleShooting()
	{
		// Fire in the right direction based on the local X scale
		if (Input.GetButton("Fire1") && canShoot)
		{
            
            Vector3 direction = 		Vector3.zero;

			bool facingRight = 			transform.localScale.x > 0;
            if (facingRight) 
                direction = 			Vector3.right;
            else
                direction =		 		Vector3.left;

			bool shootingUp = 			Input.GetAxis("Vertical") > 0;

            if (shootingUp && Input.GetAxis("Horizontal") == 0)
            {
                // Do the shooting-up animation here
                direction = Vector3.up;
                UpShoot = true;
            }
            else {
                UpShoot = false;
            }
            
			gun.Fire(direction);
			isFiring = 					true;

			if (onGround)
			{
				canMoveHorizontally = 		false;
				canJump = 					false;
				Vector2 newVel = 			rigidbody.velocity;
				newVel.y = 					0;
				//rigidbody.velocity = 		newVel;	
				rigidbody.velocity = 		Vector2.zero;
			}

		}
		else
		{
			isFiring = 						false;
			canMoveHorizontally = 			true;
            UpShoot = false;
        }
	}

	public override bool TakeDamage(float damageToTake, bool triggerInvin = true)
	{
		bool tookDamage = 			base.TakeDamage(damageToTake, triggerInvin);

		if (tookDamage) StartCoroutine(StunCoroutine(1f));

		return tookDamage;
	}

	public IEnumerator StunCoroutine(float duration)
	{
		yield return null;

		// The shooting and movement states before the stun
		bool origCanMove = 				canMove;
		bool origCanShoot = 			canShoot;

		canMove = 						false;
		canShoot = 						false;

		// Count down the stun timer
		while (duration > 0)
		{
			while (isPaused)
				yield return null;

			duration -= 				Time.deltaTime;
			yield return null;
		}

		// Revert the shooting and movement states
		canMove = 						origCanMove;
		canShoot = 						origCanShoot;
	}

	public override void Pause()
	{
		base.Pause();

		// Freeze the movement, pause any animations.
		rbState = 				Rigidbody2DSaveState.CreateFrom(rigidbody);
		rigidbody.velocity = 	Vector2.zero;

		animator.enabled = 		false;
	}

	public override void Unpause()
	{
		base.Unpause();

		// Unfreeze the movement, unpause animations.
		rbState.position = 		rigidbody.position;
		rbState.ApplyTo(rigidbody);

		animator.enabled = 		true;
	}

	void PositionGun()
	{
		if (onGround) 
            gun.transform.localPosition = 	new Vector2 (0, -0.2f);
        else 
            gun.transform.localPosition = 	new Vector2(0, 0.0f);
	}

	void UpdateAnimatorStates()
	{
		animator.SetBool("Firing", isFiring);
		animator.SetBool("Up", UpShoot);
		animator.SetBool("Ground", onGround);
		if (beingDragged) animator.SetBool("Walking", false);
	}
	
}
