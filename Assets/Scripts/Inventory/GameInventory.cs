using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Base class containing just about all functionality common among video game 
/// inventories.
/// </summary>
/// <typeparam name="TItem">Type of item it holds.</typeparam>
public abstract class GameInventory<TItem> : ScriptableObject, IInventory<TItem> where TItem: GameItem
{

	#region Customize in Inspector
	[SerializeField] protected List<TItem> _items = 			new List<TItem>();
	[Tooltip("How many items this inventory can hold.")]
	[SerializeField] protected int _capacity = 					10;
	[Tooltip("What thou wouldst expecteth. If true, overrides regular capacity.")]
	[SerializeField] protected bool _infiniteCapacity = 		false;
	#endregion
	
	#region Properties for programmatic access
	public virtual List<TItem> items 
	{
		get 					{ return _items; }
		protected set 			{ _items = value; }
	}

	public int capacity 
	{
		get 								{ return _capacity; }
		protected set 						{ _capacity = value; }
	}

	public int itemCount 					{ get { return items.Count; } }

	public virtual bool infiniteCapacity 	
	{ 
		get { return _infiniteCapacity; } 
		set { _infiniteCapacity = value; }
	}
	#endregion

	#region Methods
	public virtual void Init()
	{
		Debug.Log("Initializing " + name);
	}

	#region Basic Inventory Functionality
	/// <summary>
	/// Adds an item to the inventory. May raise ArgumentException.
	/// </summary>
	/// <returns>Returns true if successful, false otherwise.</returns>
	public virtual bool Add(TItem item)
	{
		// Safety.
		if (item == null)
			NullItemAlert();

		bool haveSpace = 			infiniteCapacity || itemCount < capacity;
		bool newItem = 				Contains(item);
		if (haveSpace && newItem)
		{
			items.Add(item);
			Debug.Log("Added " + item.name + " to inventory " + this.name);
			return true;
		}
		else 
		{
			// Let user know why the item wasn't added
			string format = 		"Failed to add item {0} to inventory {1}.\n";
			if (!haveSpace)
				format += 			"Not enough space.\n";
			if (!newItem)
				format += 			"Item already in inventory.";

			string message = 		string.Format(format, item.name, this.name);
			Debug.Log(message);	
			return false;
		}

	}

	/// <summary>
	/// Removes an item from the inventory. May raise ArgumentException.
	/// </summary>
	/// <returns>Returns true if successful, false otherwise.</returns>
	public virtual bool Remove(TItem item)
	{
		// Protection. Never leave home without it.
		if (item == null)
			NullItemAlert();

		bool haveThisItem = 				Contains(item);

		if (haveThisItem)
		{
			items.Remove(item);
			Debug.Log("Removed item " + item.name + " from inventory " + this.name + ".");
			return true;
		}
		else
		{
			string format = 				"Failed to remove item {0} from inventory {1}; {0} was not in {1} at the time of attempted removal.";
			string message = 				string.Format(format, item.name, this.name);
			Debug.LogWarning(message);
			return false;
		}
	}

	public virtual bool Contains(TItem item)
	{
		foreach (TItem invItem in items)
			if (invItem.Equals(item))
				return true;

		return false;
	}

	public virtual TItem Find(System.Predicate<TItem> match)
	{
		return items.Find(match);
	}
	public virtual void Clear()
	{
		items.Clear();
	}
	#endregion
	

	#region Helpers
	void NullItemAlert()
	{
		string errMessage = 		"Cannot add null Game Item to inventory " + this.name + ".";
		throw new System.ArgumentException(errMessage);
	}
	#endregion

	#endregion
}
