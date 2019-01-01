using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class WhipUseHandler : CSS_MonoBehaviour2D 
{
	[SerializeField] Whip2D whip;
	[SerializeField] Transform whipBase;
	[SerializeField] Transform whipEnd;

	// To reposition the whip base based on the direction the whip is being shot
	[SerializeField] Transform whipBaseUpPos;
	[Tooltip("Transform that has the position the whip base is at when shot diagonally.")]
	[SerializeField] Transform whipBaseDiagPos;

	[SerializeField] Transform sidewaysWhipBasePos;
	Player player;

	// Cached Axes
	float hAxis, vAxis;

	void Start()
	{
		whip.WhipLaunched.AddListener(OnWhipLaunched);
		whip.WhipUseDone.AddListener(OnWhipUseDone);
	}
	
	// Update is called once per frame
	protected override void Update () 
	{
		base.Update();
		UpdateAxes();
		HandleControls();
		player = GameObject.FindObjectOfType<Player>();
		
	}

	void HandleControls()
	{
		if (Input.GetButtonDown("Whip"))
			UseWhip();
	}

	void UseWhip()
	{
		// Avoid messing up whip base positioning. Only use whip when not climbing.
		if (whip.beingUsed || player.isClimbing) return;

		// Launch the whip in any direction that isn't downwards.
		Vector2 launchDir = 					Vector2.zero;

		// Make sure not to shoot diagonally when the player is pressing only the vertical axis.
		bool vertOnly = 						hAxis == 0 && vAxis > 0;

		if (!vertOnly)
		{
			bool facingRight = 					!spriteRenderer.flipX;

			if (facingRight)
				launchDir += 					Vector2.right;
			else 
				launchDir += 					Vector2.left;
		}

		launchDir.y += 							vAxis;

		// Make sure the launch dir aligns with the x scale before using the whip.
		launchDir.x *= 							Mathf.Sign(transform.localScale.x);
		MoveWhipParts(launchDir);
		whip.Use(launchDir);
	}

	void UpdateAxes()
	{
		hAxis = 								Input.GetAxis("Horizontal");
		vAxis = 								Input.GetAxis("Vertical");
	}

	void MoveWhipParts(Vector2 launchDir)
	{
		// Interpret the launch direction.
		bool sideways = 					launchDir.y <= 0;
		bool straightUp = 					launchDir.x == 0 && launchDir.y > 0;
		bool diagonal = 					launchDir.x != 0 && launchDir.y > 0;

		// Based on that interpretation, relocate the whip base and end.
		Vector3 chosenPos = 				Vector3.zero;

		if (sideways)
			chosenPos = 					sidewaysWhipBasePos.position;
		
		else if (straightUp)
			chosenPos = 					whipBaseUpPos.position;
		
		else if (diagonal)
			chosenPos = 					whipBaseDiagPos.position;
		

		whipBase.position = 				chosenPos;
		whipEnd.position = 					chosenPos;
	}

	void OnWhipLaunched()
	{
		return; 
		player.canMove = 					false;
		Vector3 stopWalking = 				player.rigidbody.velocity;
		stopWalking.x = 					0;
		player.rigidbody.velocity = 		stopWalking;
	}

	void OnWhipUseDone()
	{
		return; 
		player.canMove = 					true;
	}
}
