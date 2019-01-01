using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TeaspoonTools.Utils;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
	public static GameController S 			{ get; protected set; }
	public readonly UnityEvent Paused = 	new UnityEvent();
	public readonly UnityEvent Unpaused = 	new UnityEvent();
	public bool gamePaused 					{ get; protected set;}
	public Player player;
    public SpawnPoint FirstSpawnPoint;
	static SpawnPoint latestSpawnPoint;
	[SerializeField] SpawnPoint latestSp;
    public readonly UnityEvent GameOver = new UnityEvent();

	int spawnIndex = 0;


	// Use this for initialization
	void Awake () 
	{
		if (S == null)
			S = 						this;
		else if (S != this)
		{
			Debug.LogWarning("Can't have two GameControllers in the same scene!");
			if (this != null)
				Destroy(this.gameObject);
			return;
		}

		UpdateLatestSpawnPoint();

		player = 						GameObject.FindObjectOfType<Player>();

		//player.spawnPoint = 			latestSpawnPoint;

		if (spawnIndex > 0)
			SetPlayerPosition();

		gamePaused = 					false;
		
		DontDestroyOnLoad(this);

	}

	void Start()
	{
		SetupPickupInteractions();
		
		WatchPlayer();
		SceneManager.sceneLoaded += 		OnSceneLoaded;
	}

   
	
	/// <summary>
	/// Decide how the pickups interact with things.
	/// </summary>
	void SetupPickupInteractions()
	{
		// Have all the pickups disappear once they're picked up
		Pickup2D[] pickups = 				GameObject.FindObjectsOfType<Pickup2D>();

		foreach (Pickup2D pickup in pickups)
		{
			UnityAction destroyPickup = 	() => Destroy(pickup.gameObject);
			pickup.PickedUp.AddListener(destroyPickup);
		}
	}

	/// <summary>
	/// Pauses everything in the scene that can be paused, other than this module.
	/// </summary>
	void PauseAll()
	{
		List<IPausable> pausables = 	GetPausables();

		foreach (IPausable pausable in pausables)
			pausable.Pause();
		
	}

	/// <summary>
	/// Unpauses everything in the scene that can be unpaused, other than this module.
	/// </summary>
	void UnpauseAll()
	{
		List<IPausable> pausables = 	GetPausables();

		foreach (IPausable unpausable in pausables)
			unpausable.Unpause();
	}

	/// <summary>
	/// Pauses the game.
	/// </summary>
	public void Pause()
	{
		if (gamePaused)
		{
			Debug.LogWarning(this.name + " is already paused.");
			return;
		}

		gamePaused = 			true;
		Paused.Invoke();
	}

	public void Unpause()
	{
		if (!gamePaused)
		{
			Debug.LogWarning(this.name + " is already unpaused.");
			return;
		}

		gamePaused = 			false;
		Unpaused.Invoke();
	}

	/// <summary>
	/// Look through all the components of each game object in the scene, and return the
	/// ones that are pausable
	/// </summary>
	/// <returns></returns>
	List<IPausable> GetPausables()
	{
		List<IPausable> pausables = 			new List<IPausable>();

		foreach (GameObject go in GameObject.FindObjectsOfType<GameObject>())
		{
			Component[] components = 			go.GetComponents<Component>();

			foreach (Component component in components)
				if (component is IPausable)
					pausables.Add(component as IPausable);
			
		}

		return pausables;
	}

	void SetupCallbacks()
	{
		// Make sure that all the pickups get destroyed when they get picked up
		Pickup2D[] pickups = 			GameObject.FindObjectsOfType<Pickup2D>();
		foreach (Pickup2D pickup in pickups)
		{
			UnityAction destroyPickup = () => Destroy(pickup.gameObject);
			pickup.PickedUp.AddListener(destroyPickup);
		}
	}

	void OnPlayerTriggerEnter2D(Collider2D other)
	{
		// Make sure that when the player hits a respawn point, it will spawn to it if 
		// it's further in the level.

		SpawnPoint otherPoint = 		other.gameObject.GetComponent<SpawnPoint>();

		if (otherPoint != null && otherPoint.number > latestSpawnPoint.number)
		{
			Debug.Log("Found new spawn point.");
			latestSpawnPoint = 			otherPoint;
			latestSp = 					latestSpawnPoint;
			spawnIndex = 				latestSpawnPoint.number;
		}
		
	}

	void OnPlayerDeath()
	{
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	}

	void WatchPlayer()
	{
		player.contactEvents.TriggerEnter2D.AddListener(OnPlayerTriggerEnter2D);
		player.Died.AddListener(OnPlayerDeath);
	}

	void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
	{
		Awake();
	}

	void UpdateLatestSpawnPoint()
	{
		// Decide the latest spawn point based on the spawn index. The spawn point with 
		// its number equal to the index is the latest.

		SpawnPoint[] spawnPoints = 				GameObject.FindObjectsOfType<SpawnPoint>();

		foreach (SpawnPoint spawnPoint in spawnPoints)
		{
			if (spawnPoint.number == spawnIndex)
			{
				latestSpawnPoint = 				spawnPoint;
				break;
			}
		}
	}

	void SetPlayerPosition()
	{
		SpawnPoint[] spawnPoints = 				GameObject.FindObjectsOfType<SpawnPoint>();

		foreach (SpawnPoint spawnPoint in spawnPoints)
		{
			if (spawnPoint.number == spawnIndex)
			{
				player.transform.position = 	spawnPoint.transform.position;
				break;
			}
		}
	}


}
