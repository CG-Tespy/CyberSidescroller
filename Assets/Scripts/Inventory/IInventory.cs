using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


/// <typeparam name="T">What the inventory can hold.</typeparam>
public interface IInventory<T>
{
	bool Add(T toAdd);
	bool Remove(T toRemove);
	void Clear();
}
