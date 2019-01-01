using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ItemUseTester : MonoBehaviour 
{
	Player player;
	CSSInventory inventory;
	[SerializeField] CSSItem item;

	// Use this for initialization
	void Awake () 
	{
		player = 			GameObject.FindObjectOfType<Player>();
		inventory = 		player.inventory;
	}
	
	// Update is called once per frame
	void Update () 
	{
		
	}

	public void UseItemOnPlayer()
	{
		// Find the copy of the item, and use it
		CSSItem itemCopy = 				inventory.Find( (CSSItem item) => item.name == this.item.name);

		if (itemCopy != null)
		{
			itemCopy.UseOn(player);
			Debug.Log("Used " + itemCopy.name + " on " + player.name + "!");
		}
		else
		{
			string format = 			"Couldn't find a {0} in {1}'s inventory!";
			string message = 			string.Format(format, item.name, player.name);
			Debug.LogWarning(message);
		}
	}
}
