using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class Item : ScriptableObject, System.IEquatable<Item>
{
    public enum Type
    {
        healing
    }
    
    // Customize in the Inspector
    [SerializeField] protected Item.Type _type;
    [SerializeField] protected Sprite _icon;
    [SerializeField] protected Sprite _overworldSprite;

    // To access members through script
    public Item.Type type                   { get { return _type; } }
    public Sprite icon                      { get { return _icon; } }
    public Sprite overworldSprite           { get { return _overworldSprite; } }

    // Methods

    public virtual void Use()
    {
        Debug.Log("Used " + name);
    }

    public virtual bool Equals(Item otherItem)
    {
        bool hasSameType =                     type == otherItem.type;
        bool hasSameIcon =                     icon == otherItem.icon;
        bool hasSameOwSprite =                 overworldSprite == otherItem.overworldSprite;

        return hasSameType && hasSameIcon && hasSameOwSprite;
    }
	
}
