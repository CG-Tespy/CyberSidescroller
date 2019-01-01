using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// These are to replace an actual ItemCategory enum, so any designer can easily add a 
/// new type of category if they want.
/// </summary>
[CreateAssetMenu(menuName = "Inventory/Items/Category", fileName = "NewItemCategory")]
public class GameItemCategory : ScriptableObject, System.IEquatable<GameItemCategory>
{
    [SerializeField] Sprite _icon;

    public Sprite icon                  { get { return _icon; } }

    public bool Equals(GameItemCategory other)
    {
        bool sameSprite =               this.icon.Equals(other.icon);
        bool sameName =                 this.name == other.name;

        return sameSprite && sameName;
    }
	
}
