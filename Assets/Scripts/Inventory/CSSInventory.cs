using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using CyberSidescroller.Items;

/// <summary>
/// Base inventory class for the Cyber Sidescroller Project.
/// </summary>
[CreateAssetMenu(menuName = "Inventory/CSSInventory", fileName = "NewCSSInventory")]
public class CSSInventory : GameInventory<CSSItem>
{
	#region For Other Modules to Respond to
	public UnityEvent ContentChanged 		{ get; protected set; } = new UnityEvent();
	public Event<CSSItem> ItemAdded 		{ get; protected set; } = new Event<CSSItem>();
	public Event<CSSItem> ItemRemoved 		{ get; protected set; } = new Event<CSSItem>();
	#endregion
	protected UnityAction<UsageArgs<CSSItem>> itemUseResponse;

	#region Fields/Properties
	// The items list you see in the inspector just holds the Item Scriptable 
	// objects as blueprints; the realItems list holds copies of those, so that 
	// functionality such as usage-decreasing works as it should.
	// Otherwise, we'd also run into issues related to the shared state of Scriptable Objects.
	List<CSSItem> _realItems = 				new List<CSSItem>();
	List<CSSItem> _backupItems = 			new List<CSSItem>();
	// ^ To help reset state and such

	public override List<CSSItem> items 
	{
		get 								{ return _realItems; }
		protected set 						{ _realItems = value; }
	}

	#endregion
	#region Methods

	public override bool Add(CSSItem item)
	{
		bool itemAdded = 			base.Add(item);

		if (itemAdded)
		{
			WatchForItemUse(item);
			base.items.Add(item); // So it updates in the inspector.
		}

		return itemAdded;
	}

	public override bool Remove(CSSItem item)
	{
		bool itemRemoved = 			base.Remove(item);

		if (itemRemoved)
		{
			IgnoreItemUse(item);
			//base.items.Remove(item); // So it updates in the inspector.
		}

		return itemRemoved;
	}

	public override bool Contains(CSSItem item)
	{
		// Make sure this only applies to the real items list
		foreach (CSSItem realItem in _realItems)
			if (item.Equals(realItem))
				return true;

		return false;
	}

	protected virtual void OnItemUse(UsageArgs<CSSItem> usageArgs)
	{
		// If the item ran out of uses, remove it from the inventory
		CSSItem item = 								usageArgs.itemUsed;

		if (!item.infiniteUses && item.uses <= 0)
			Remove(item);
	}


	/// <summary>
	/// This is treated as the initializer for this inventory, for it doesn't rely on a scene 
	/// being loaded; it can just update itself as the project scripts recompile.
	/// </summary>
	public virtual void OnEnable()
	{
		itemUseResponse = 		OnItemUse;
		ResetItemLists();
		SetCallbacks();
		WatchInitialItems();

		Debug.Log(this.name + " OnEnable()!");
	}

	/// <summary>
	/// Treated as a cleanup method of sorts, this cleans up certain members of this inventory.
	/// </summary>
	public virtual void OnDisable()
	{
		ItemAdded.RemoveAllListeners();
		ItemRemoved.RemoveAllListeners();
		ContentChanged.RemoveAllListeners();
		Debug.Log(this.name + " OnDisable()!");
	}

	#region Helpers
	protected virtual void WatchForItemUse(CSSItem toWatch)
	{
		toWatch.Used.AddListener(itemUseResponse);
	}

	protected virtual void IgnoreItemUse(CSSItem toIgnore)
	{
		// The item is no longer this inventory's responsibility, so...
		toIgnore.Used.RemoveListener(itemUseResponse);
	}

	void SetCallbacks()
	{
		// Naturally, when an item is added or removed, the content is changed, so...
		UnityAction<CSSItem> contentChangeAlert = 	(CSSItem item) => ContentChanged.Invoke();

		ItemAdded.AddListener(contentChangeAlert);
		ItemRemoved.AddListener(contentChangeAlert);
	}

	void WatchInitialItems()
	{
		foreach (CSSItem item in items)
			WatchForItemUse(item);
		
	}

	void ResetItemLists()
	{
		// Empty the real items list, refilling it with the blueprints in the inspector's
		// item list. And have the blueprint items be copied into the backup.
		_realItems.Clear();
		
		List<CSSItem> blueprintItems = 		base.items;
		foreach(CSSItem item in blueprintItems)
		{
			CSSItem itemCopy = 				CSSItem.Copy(item);
			_realItems.Add(itemCopy);
			//Debug.Log("Item copied to real inventory!");
			//_backupItems.Add(item);
		}

	}

	#endregion

	#endregion

}
