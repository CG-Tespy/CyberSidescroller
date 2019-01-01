using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class DeactivateOnEnemyDeath : MonoBehaviour 
{
	[SerializeField] EnemyController enemy;


	// Use this for initializing own members
	void Awake () 
	{
		if (enemy == null)
		{
			Debug.LogWarning(this.name + " has no enemy to watch for.");
			return;
		}
		
	}

	void Start()
	{
		enemy.Died.AddListener(Deactivate);
	}

	void Deactivate()
	{
		gameObject.SetActive(false);
	}
}
