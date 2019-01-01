using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WF_PlayerController : CSS_MonoBehaviour2D
{
	[SerializeField] float _moveSpeed = 	5;
	[SerializeField] Vector2 jumpForce = 	new Vector2(0, 200);
	[SerializeField] Whip2D whip;

	// Use this for initialization
	void Start () 
	{
		
	}
	
	// Update is called once per frame
	void Update () 
	{
		HandleMovement();
		HandleSpriteFlipping();
		HandleWhipUse();
	}

	void HandleMovement()
	{
		float hMovement = 				Input.GetAxis("Horizontal") * _moveSpeed;
		float vMovement = 				Input.GetAxis("Vertical") * _moveSpeed * 0;

		Vector2 movement = 				new Vector2(hMovement, vMovement);
		rigidbody.velocity = 			movement;

		HandleJumping();
	}

	void HandleJumping()
	{
		if (Input.GetButtonDown("Jump"))
			rigidbody.AddForce(jumpForce);

	}
	void HandleSpriteFlipping()
	{
		// Flip the scale instead of the sprite alone, so the child objects also get flipped
		float hAxis = 						Input.GetAxis("Horizontal");

		bool faceRight = 					hAxis > 0;
		bool faceLeft = 					hAxis < 0;
		Vector3 newLocalScale = 			transform.localScale;

		if (faceRight)
			newLocalScale.x = 				Mathf.Abs(newLocalScale.x);

		else if (faceLeft)
			newLocalScale.x = 				Mathf.Abs(newLocalScale.x) * -1;
		

		transform.localScale = 				newLocalScale;

	}

	void HandleWhipUse()
	{
		if (Input.GetAxis("Fire1") > 0)
		{
			// Shoot sideways, up, down...?
			float vAxis = 							Input.GetAxisRaw("Vertical");
			float hAxis = 							Input.GetAxisRaw("Horizontal");
			Vector2 launchDir = 					Vector2.zero;

			// Make sure not to shoot diagonally when the player is pressing only the vertical axis
			// and not the horizontal.
			bool vertOnly = 						hAxis == 0 && vAxis != 0;

			if (!vertOnly)
			{
				bool facingRight = 					transform.localScale.x > 0;

				if (facingRight)
					launchDir += 					Vector2.right;
				else 
					launchDir += 					Vector2.left;
			}

			launchDir.y += 							vAxis;

			whip.Use(launchDir);
		}
	}
}
