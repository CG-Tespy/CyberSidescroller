using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Whip2D : MonoBehaviour 
{
	public UnityEvent WhipLaunched {get; protected set; } = new UnityEvent();
	public UnityEvent WhipUseDone { get; protected set; } = new UnityEvent();
	[SerializeField] SliderJoint2D playerJoint;
	[SerializeField] LineRenderer whipLine;
	[SerializeField] Transform whipBase;
	[SerializeField] Transform whipEnd;
	[SerializeField] float damage = 						10;
	[SerializeField] float length = 						5;
	[SerializeField] float launchSpeed = 					3;
	[SerializeField] float retractSpeed = 					3;
	[Tooltip("How fast the whip drags the player top its position when clinging to something.")]
	[SerializeField] float dragSpeed = 3;
	
	[Tooltip("How long it takes before the whip starts retracting after extending to its max length.")]
	[SerializeField] float whipRetractDelay =		 		0.5f;
	[Tooltip("This whip can damage things on these layers.")]
	[SerializeField] LayerMask damageLayers;
	[Tooltip("This whip can cling onto things on these layers.")]
	[SerializeField] LayerMask clingLayers;

	public bool beingUsed = false;
	IEnumerator whipCoroutine;
	Rigidbody2D playerRb;
	Transform playerTrans;
	CSS_MonoBehaviour2D playerBehaviour;

	// Cached stuff
	float prevGravScale = 0;
	RigidbodyType2D prevBodyType;
	Rigidbody2D endRb;

	// Use this for initialization
	void Awake () 
	{
		playerRb = 							playerJoint.GetComponent<Rigidbody2D>();
		playerBehaviour = 					playerRb.GetComponent<CSS_MonoBehaviour2D>();
		endRb = 							whipEnd.GetComponent<Rigidbody2D>();
		CheckForMissingComponents();
		playerTrans = 						playerRb.transform;
		playerJoint.enabled = 				false;
		whipLine.positionCount = 			2;
		whipLine.enabled = 					false;

	}

	void Start()
	{
		WatchForWhipCollisions();
	}
	
	// Update is called once per frame
	void Update () 
	{
		
	}

	void LateUpdate()
	{
		UpdateLineRenderer();
	}

	public void Use(Vector2 direction)
	{
		if (beingUsed) return;

		whipLine.enabled = 				true;
		beingUsed = 					true;
		whipCoroutine = 				LaunchWhip(direction);
		StartCoroutine(whipCoroutine);
		WhipLaunched.Invoke();
	}

	public void PutAway()
	{
		beingUsed = 						false;
		playerJoint.enabled = 				false;
		whipLine.enabled = 					false;
		playerRb.gravityScale = 			prevGravScale;
		endRb.bodyType = 					prevBodyType;
		whipCoroutine = 					null;
		WhipUseDone.Invoke();
	}

	#region Whip Movement

	IEnumerator LaunchWhip(Vector2 direction)
	{
		// Get the whip end some distance away from the base.
		Vector3 normDir = 				direction.normalized;
		Vector3 targetPos = 			whipBase.position;
		targetPos += 					normDir * length;

		// Decide how fast that will happen, and keep track of it through a timer.
		float speedMultiplier = 		launchSpeed / 10;
		float frameRate = 				1f / Time.deltaTime;
		float timer = 					0;
		float timeToPass = 				(1 / speedMultiplier);

		// Make it happen through position-lerping.
		while (whipEnd.position != targetPos)
		{
			targetPos.x = 				Mathf.Abs(targetPos.x) * Mathf.Sign(playerBehaviour.transform.localScale.x);
			Vector2 newPos = 			Vector2.Lerp(whipBase.position, targetPos, 
										timer / timeToPass);
			whipEnd.position = 			newPos;
			timer += 					Time.deltaTime;
			yield return null;
		}

		yield return null;
		yield return new WaitForSeconds(whipRetractDelay);
		whipCoroutine = 				RetractWhip();
		StartCoroutine(whipCoroutine);
	}

	IEnumerator RetractWhip()
	{
		// Lerp the whip end's position to the whip base's position, making sure no obstacles can get in the way.
		Collider2D endCollider = 		whipEnd.GetComponent<Collider2D>();
		bool prevEnabled = 				endCollider.enabled;
		endCollider.enabled = 			false;

		float timeToPass = 				1f / retractSpeed;
		float timer = 					0;

		while (timer < timeToPass)
		{
			Vector2 newPos = 			Vector2.Lerp(whipEnd.position, whipBase.position,
										timer / timeToPass);
			whipEnd.position = 			newPos;
			timer+= Time.deltaTime;
			yield return null;
		}

		Debug.Log("Done retracting!");
		endCollider.enabled = 			prevEnabled;
		beingUsed = 					false;
		whipLine.enabled = 				false;
		whipCoroutine = 				null;

	}

	IEnumerator DragPlayer()
	{
		// Make the end's body type non-dynamic, so only the player moves during the dragging.
		prevBodyType = 						endRb.bodyType;
		endRb.bodyType = 					RigidbodyType2D.Static;

		// Make sure that pesky gravity doesn't get in the way
		prevGravScale = 					playerRb.gravityScale;
		playerRb.gravityScale = 			0;

		// Make sure we're moving at the right angle.
		playerJoint.enabled = 				true;
		float dragAngle = 					Vector2.Angle(playerTrans.position, whipEnd.position);
		playerJoint.angle = 				dragAngle;

		// At the right speed.
		playerJoint.useMotor = 				true;
		JointMotor2D newMotor = 			playerJoint.motor;
		newMotor.motorSpeed = 				-dragSpeed;
		playerJoint.motor = 				newMotor;

		float distThresh = 					1;

		// When the whip base and get get close enough, stop dragging.
		while (Vector2.Distance(whipBase.position, whipEnd.position) > distThresh)
		{
			dragAngle = 					Vector2.Angle(playerTrans.position, whipEnd.position);
			playerJoint.angle = 			dragAngle;
			yield return null; // Until then, just let the motor do its thing. 
		}

		Debug.Log("Done dragging!");
		
		// And we're done!
		PutAway();
		
	}

	#endregion

	void OnWhipCollisionEnter2D(Collision2D other)
	{
		OnWhipTriggerEnter2D(other.collider);
	}

	void OnWhipTriggerEnter2D(Collider2D other)
	{
		int otherLayer = 						other.gameObject.layer;

		// Deal damage to contacted enemies.
		if (damageLayers.ContainsLayer(otherLayer))
		{
			IDamageable<float> toDamage = 		other.gameObject.GetComponent<IDamageable<float>>();
			if (toDamage != null)
				toDamage.TakeDamage(damage, false);
			return;
		}

		// Drag player towards clingable surfaces.
		if (clingLayers.ContainsLayer(otherLayer))
		{
			if (whipCoroutine != null)
				StopCoroutine(whipCoroutine);

			whipCoroutine = 	DragPlayer();
			StartCoroutine(whipCoroutine);
			return;
		}

	}

	// Helpers

	void CheckForMissingComponents()
	{
		bool hasplayerJointJoint = 					playerJoint != null;
		bool hasWhipBase = 							whipBase != null;
		bool hasWhipEnd = 							whipEnd != null;
		bool hasWhipLine = 							whipLine != null;
		bool hasplayerJointRb = 					playerRb != null;
		bool hasPlayerBehav = 						playerBehaviour != null;
		bool missingSomething = 					!hasplayerJointJoint || !hasWhipBase || !hasWhipEnd || 
													!hasWhipLine || !hasplayerJointRb || !hasPlayerBehav;

		if (!missingSomething)
			return;

		string errorMessage = "{0} is missing these components:\n";

		if (!hasplayerJointJoint)
			errorMessage += "SliderJoint2D for the playerJoint\n";
		if (!hasWhipBase)
			errorMessage += "Transform for the whip base\n";
		if (!hasWhipEnd)
			errorMessage += "Transform for the whip end\n";
		if (!hasWhipLine)
			errorMessage += "LineRenderer for the whip itself";
		if (!hasplayerJointRb)
			errorMessage += "Rigidbody for the playerJoint\n";
		if (!hasPlayerBehav)
			errorMessage += "CSS_MonoBehaviour2D for the player\n";

		throw new System.NullReferenceException(errorMessage);
	}

	void WatchForWhipCollisions()
	{
		GameObject whipGo = 				whipEnd.gameObject;
		CSS_MonoBehaviour2D watchable = 	whipEnd.GetComponent<CSS_MonoBehaviour2D>();

		if (watchable == null)
			watchable = 					whipGo.AddComponent<CSS_MonoBehaviour2D>();

		watchable.contactEvents.CollisionEnter2D.AddListener(OnWhipCollisionEnter2D);
		watchable.contactEvents.TriggerEnter2D.AddListener(OnWhipTriggerEnter2D);
	}

	void UpdateLineRenderer()
	{
		if (!whipLine.enabled) return;

		Vector2 basePos = 				whipBase.position;
		Vector2 endPos = 				whipEnd.position;

		// Make sure the pos coords are in the right space
		if (!whipLine.useWorldSpace)
		{
			basePos = 					playerTrans.InverseTransformPoint(basePos);
			endPos = 					playerTrans.InverseTransformPoint(endPos);
		}

		whipLine.SetPosition(0, basePos);
		whipLine.SetPosition(1, endPos);
	}
	
}
