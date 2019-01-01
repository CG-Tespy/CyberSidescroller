using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public abstract class GameItem : ScriptableObject, IItem<MonoBehaviour>, System.IEquatable<GameItem>
{

	#region Nested Classes
	
	[System.Serializable]
	public class Graphics : System.IEquatable<Graphics>
	{
		public Sprite sprite;
		public Mesh model;

		public Graphics(Graphics original)
		{
			this.sprite = 		original.sprite;
			this.model = 		original.model;
		}
		public Graphics(Sprite sprite, Mesh model)
		{
			this.sprite = 			sprite;
			this.model = 			model;
		}

		public bool Equals(Graphics other)
		{
			bool sameSprite = 			this.sprite.Equals(other.sprite);
			bool sameModel = 			this.model.Equals(other.model);

			return sameSprite && sameModel;
		}

		public static Graphics Copy(Graphics original)
		{
			return new Graphics(original.sprite, original.model);
		}
	}

	#endregion

	#region Customize in Editor
	[Tooltip("How many times this can be used before it's gone.")]
	[SerializeField] int _uses = 							1;
	[Tooltip("As it sounds. If true, this overrides the uses variable.")]
	[SerializeField] bool _infiniteUses = 					false;
	[SerializeField] GameItemCategory[] _categories = 		null;
	[SerializeField] protected Graphics graphics = 			null;
	

	//[SerializeField] CustomAttribute[] _custAttributes;
	#endregion
	

	#region Properties for programmatic access
	public virtual GameItemCategory[] categories
	{
		get 				{ return _categories; }
		protected set 		{ _categories = value; }
	}
	
	public virtual int uses 									
	{ 
		get 				{ return _uses; } 
		protected set 		{ _uses = value; } 
	}

	public virtual bool infiniteUses 					
	{ 
		get { return _infiniteUses; } 
		set { _infiniteUses = value; }
	}


	//public CustomAttribute[] custAttributes 			{ get { return _custAttributes; } }

	public Sprite sprite 								{ get { return graphics.sprite; } }
	public Mesh model 									{ get { return graphics.model; } }

	#endregion

	// Methods

	/// <summary>
	/// Has this item used on the passed object. Returns true of successful, false otherwise.
	/// </summary>
	public abstract bool UseOn(MonoBehaviour toUseOn);

	/// <summary>
	/// Returns a deep copy of the passed GameItem.
	/// </summary>
	public static GameItem Copy(GameItem original)
	{
		GameItem itemCopy = 		ScriptableObject.CreateInstance<GameItem>();
		CopyBaseAttributes(original, itemCopy);

		return itemCopy;
	}	



	public virtual bool Equals(GameItem other)
	{
		bool sameGraphics = 			this.graphics.Equals(other.graphics);

		bool sameCategoryLength = 		this.categories.Length == other.categories.Length;
		bool sameCategories = 			true;

		if (sameCategoryLength)
			for (int i = 0; i < categories.Length; i++)
			{
				if (!this.categories[i].Equals(other.categories[i]))
				{
					sameCategories = 	false;
					break;
				}
			}

		bool sameUseLimit = 			this.infiniteUses == other.infiniteUses;

		return sameGraphics && sameCategories && sameUseLimit;

	}

	// Helpers
	protected static void CopyBaseAttributes(GameItem from, GameItem to)
	{
		to.name = 				from.name;
		to.uses = 				from.uses;
		to.infiniteUses = 		from.infiniteUses;

		to.categories = 		new GameItemCategory[from.categories.Length];
		for (int i = 0; i < to.categories.Length; i++)
			to.categories[i] = from.categories[i];
		
		to.graphics = 			Graphics.Copy(from.graphics);
	}
}
