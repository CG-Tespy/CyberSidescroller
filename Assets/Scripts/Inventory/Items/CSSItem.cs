using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using CyberSidescroller.Items;

/// <summary>
/// Base item class for the Cyber Sidescroller Project.
/// </summary>
[System.Serializable]
[CreateAssetMenu(menuName = "Inventory/Items/BaseItem", fileName = "NewBaseItem")]
public class CSSItem : GameItem
{
	
	// Announces when this item is used, for other modules to respond accordingly
	public UseEvent<UsageArgs<CSSItem>, CSSItem> Used 		{ get; protected set; } =			
			new UseEvent<UsageArgs<CSSItem>, CSSItem>();

	
	#region To customize in the inspector
	[Tooltip("These activate when the item is used.")]
	[SerializeField] GameEffect[] _activeEffects = 		null;
	[Tooltip("These activate as the item is carried.")]
	[SerializeField] GameEffect[] _passiveEffects = 	null;
	#endregion
	
	#region Properties for programmatic access
	public virtual GameEffect[] activeEffects 			
	{ 
		get 						{ return _activeEffects; } 
		protected set 				{ _activeEffects = value; }
	}
	public GameEffect[] passiveEffects 					
	{ 
		get 						{ return _passiveEffects; }
		protected set 				{ _passiveEffects = value; } 
	}
	#endregion

	#region Methods
	public override bool UseOn(MonoBehaviour toUseOn)
	{
		ApplyActiveEffects(toUseOn);

		if (!infiniteUses)
			uses--;
			
		AnnounceUsage(toUseOn);

		// We've not defined anything yet for an item use's failure, so...
		return true;
	}

	/// <summary>
	/// For the option to apply the active effects outside normal usage.
	/// </summary>
	public bool ApplyActiveEffects(MonoBehaviour toApplyTo)
	{
		foreach (GameEffect effect in activeEffects)
			effect.Apply(toApplyTo);
		
		AnnounceUsage(toApplyTo);
		return true;
	}

	public bool ApplyPassiveEffects(MonoBehaviour toApplyTo)
	{
		foreach (GameEffect effect in passiveEffects)
			effect.Apply(toApplyTo);
		
		return true;
	}

	public virtual void OnEnable()
	{
		//Debug.Log(this.name + " OnEnable()!");
	}
	public virtual void OnDisable()
	{
		// ScriptableObjects' OnDisable() methods are called when scripts are done 
		// recompiling (which happens when you save code, go back to the editor, and wait). 
		// Then, an OnEnable call is made.

		// This makes OnDisable a good cleanup method for Scriptable Objects. And you can use 
		// OnEnable as a sort of initialization method.
		// Debug.Log(this.name + " removing all listeners!");
		Used.RemoveAllListeners();
	}

	public override bool Equals(GameItem other)
	{
		// TODO: See how to make sure two objects are of the exact same type, regardless
		// of casting
		bool sameBaseAttributes = 	base.Equals(other);
		bool sameClass = 			other is CSSItem;

		bool sameExactClass = 		false;

		System.Type thisType = 		typeof(CSSItem);

		return sameBaseAttributes;
	}

	/// <summary>
	/// Returns a mostly-deep copy of the passed CSSItem.
	/// </summary>
	public static CSSItem Copy(CSSItem original)
	{
		//CSSItem itemCopy = 			MonoBehaviour.Instantiate<CSSItem>(original);
		CSSItem itemCopy = 		ScriptableObject.CreateInstance<CSSItem>();
		CopyBaseAttributes(original, itemCopy);
		CopyCSSItemAttributes(original, itemCopy);

		return itemCopy;
	}

	#region Helpers
	void AnnounceUsage(MonoBehaviour usedOn)
	{
		UsageArgs<CSSItem> usageArgs = 		new UsageArgs<CSSItem>(this, usedOn);
		Used.Invoke(usageArgs);

		//Debug.Log("Used " + this.name);
	}

	protected static void CopyCSSItemAttributes(CSSItem from, CSSItem to)
	{
		to.activeEffects = 			from.activeEffects;
		to.passiveEffects = 		from.passiveEffects;
	}

	#endregion

	#endregion

}
