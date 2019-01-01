using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class TransitionTeleporter : CSS_MonoBehaviour2D
{
	[Tooltip("Name of scene to transition to.")]
	[SerializeField] string sceneName;

	[SerializeField] float teleportDuration = 2;

	ScreenFader screenFader;

	// Use this for initializing own members
	protected override void Awake () 
	{
		base.Awake();
		screenFader = 								GameObject.FindObjectOfType<ScreenFader>();
	}
	
	void StartTeleportation(Player toTeleport)
	{
		toTeleport.canMove = 						false;
		toTeleport.rigidbody.velocity = 			Vector2.zero;
		screenFader.FadeEnded.AddListener(TeleportToScene);
		
		screenFader.Fade(teleportDuration, 1, Color.white);
	}

	void TeleportToScene(FadeEventArgs eventArgs)
	{
		screenFader.FadeEnded.RemoveListener(TeleportToScene);
		Destroy(GameController.S.gameObject);
		SceneManager.LoadScene(sceneName);
	}

	protected override void OnCollisionEnter2D(Collision2D other)
	{
		base.OnCollisionEnter2D(other);

		// If the other player collided with this, stop it from moving, then 
		// teleport it.
		Player player = 							other.gameObject.GetComponent<Player>();

		if (player != null)
			StartTeleportation(player);
		
	}
}
