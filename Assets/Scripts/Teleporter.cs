using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameObjectEvent : UnityEvent<GameObject> {}

public class Teleporter : CSS_MonoBehaviour2D
{
	[Tooltip("Time it takes for the teleporter to do its thing.")]
	[SerializeField] float delay = 			1f;

	[SerializeField] Teleporter destination;
	[Tooltip("Where other objects end up when sent to this teleporter.")]
	public Transform receiver;

	public bool canTeleport = 					true; // May be deprecated soon

	public Animator animator 					{ get; protected set; }

	// Helps avoid teleportation loops
	List<GameObject> teleporting = 				new List<GameObject>(); 
	public GameObjectEvent TeleportingObject 	{ get; protected set; }
	public GameObjectEvent ReceivedObject 		{ get; protected set; }

	// Methods
	protected override void Awake()
	{
		base.Awake();
		TeleportingObject = 				new GameObjectEvent();
		ReceivedObject = 					new GameObjectEvent();
		animator = 							GetComponent<Animator>();
		animator.enabled = 					false;
	}

	void Start()
	{
		SetCallbacks();
	}

	protected override void OnTriggerExit2D(Collider2D otherCollider)
	{
		base.OnTriggerExit2D(otherCollider);
		GameObject otherGo = otherCollider.gameObject;
		if (teleporting.Contains(otherGo)) teleporting.Remove(otherGo);

	}

	protected override void OnTriggerEnter2D(Collider2D otherCollider)
	{
		base.OnTriggerEnter2D(otherCollider);

		GameObject otherGo = 				otherCollider.gameObject;

		// Avoid teleporting what's already in the process of being so.
		if (teleporting.Contains(otherGo)) return;
		
		// If the thing that touched this is the player, teleport it.
		Player player = 					otherGo.GetComponent<Player>();

		if (player != null)
		{
			Debug.Log("Teleporting " + otherCollider.name + " using " + this.name);
			StartCoroutine(Teleport(player));	
		}
	}

	IEnumerator Teleport(Player toTeleport)
	{
		// Let anything that cares (certainly the destination) know this is teleporting something.
		teleporting.Add(toTeleport.gameObject);
		TeleportingObject.Invoke(toTeleport.gameObject);
		
		// Keep the player from moving.
		Rigidbody2DSaveState rbSaveState = 	new Rigidbody2DSaveState(toTeleport.rigidbody);
		rbSaveState.velocity.x = 			0;
		toTeleport.Pause();
		toTeleport.animator.enabled = 		false;
		rbSaveState.ApplyTo(toTeleport.rigidbody);

		// Play the animation, and let the destination receive the player.
		yield return PlayAnimation();

		destination.ReceiveObject(toTeleport);
		canTeleport = 						false; // Without this, we get an infinite teleportation loop.
	}

	/// <summary>
	/// For when another teleporter sends something to this teleporter.
	/// </summary>
	public void ReceiveObject(Player toReceive)
	{
		StartCoroutine(ObjectReceiving(toReceive));
	}

	IEnumerator ObjectReceiving(Player toReceive)
	{
		// Relocate the player, and after a delay...
		Vector3 targetPos = receiver.position;
		targetPos.y += 		toReceive.height * 1.25f;

		toReceive.rigidbody.position = 		targetPos;

		yield return PlayAnimation();

		// ... Let the player move again, before announcing this received it.
		toReceive.Unpause();
		toReceive.canMove = 				true;
		toReceive.animator.enabled = 		true;

		ReceivedObject.Invoke(toReceive.gameObject);
	}

	IEnumerator PlayAnimation()
	{
		// The animator plays the animation continuously while enabled
		animator.enabled = 					true;
		yield return new WaitForSeconds(delay);
	}

	void SetCallbacks()
	{
		destination.ReceivedObject.AddListener(OnDestinationReceivedObject);
		destination.TeleportingObject.AddListener(OnDestinationTeleportingObject);
	}	

	void OnDestinationReceivedObject(GameObject sent)
	{
		// It becomes safe to teleport the sent object again once it enters this teleporter's trigger.
		teleporting.Remove(sent);
	}

	void OnDestinationTeleportingObject(GameObject sent)
	{
		// Avoid this teleporter returning what was teleported to it until the right time comes.
		teleporting.Add(sent); 
	}
}
