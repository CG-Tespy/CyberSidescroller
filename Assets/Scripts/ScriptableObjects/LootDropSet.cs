using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Items/Loot/DropSet", fileName = "NewLootDropSet")]
public class LootDropSet : ScriptableObject 
{
	public LootDrop[] lootDrops;
	public int amountDroppable = 1;

	public GameObject[] DropItem(MonoBehaviour dropper)
	{
		GameObject item = 					null;
		float dropChance = 					0.0f;
		int amountDropped = 				0;
		float xDropOffset = 				0.0f;

		List<GameObject> droppedItems = 	new List<GameObject>();

		// Go through each item, using random numbers to decide whether they get dropped
		foreach (LootDrop lootDrop in lootDrops)
		{
			item = 					lootDrop.item;
			dropChance = 			lootDrop.dropChance;

			float randNum = 		Random.Range(0.0f, 100.0f);
			bool dropThisItem = 	randNum <= dropChance;

			if (dropThisItem)
			{
				Vector3 dropPos = 			dropper.transform.position;
				dropPos.x += 				xDropOffset;
				GameObject droppedItem =  	MonoBehaviour.Instantiate<GameObject>(item, dropPos, Quaternion.identity);

				xDropOffset += 		0.25f;
				amountDropped++;
				droppedItems.Add(droppedItem);
			}

			// Make sure not to drop more than this set allows
			if (amountDropped >= amountDroppable)
				return droppedItems.ToArray();

		}

		return droppedItems.ToArray();
	}
	
}

[System.Serializable]
public class LootDrop
{
	public GameObject item;

	[Range(0.0f, 100.0f)]
	public float dropChance = 		10f;
}
