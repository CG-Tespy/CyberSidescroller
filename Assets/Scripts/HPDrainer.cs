using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class HPDrainer : MonoBehaviour 
{
	[SerializeField] float _drainRate;
	public float drainRate
	{
		get { return _drainRate; }
		set { _drainRate = value; }
	}

	SidescrollerCharacter player;

	// Use this for initialization
	void Awake () 
	{
		player = 								GameObject.FindObjectOfType<SidescrollerCharacter>();
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (player.hp > 0)
		{
			float damageToDeal = 				drainRate * Time.deltaTime;
			player.TakeDamage(damageToDeal, false);
		}
	}
}
