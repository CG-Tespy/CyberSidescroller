using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Items/Healing", fileName = "NewHealingItem")]
public class HealingItem : Item, System.IEquatable<HealingItem>
{
    [SerializeField] float healAmount;

    public virtual void Use(IHasHP target)
    {
        target.TakeHealing(healAmount);
    }

    public override bool Equals(Item otherItem)
    {
        // Make sure the other item is of the same type
        bool sameType =             otherItem as HealingItem != null;
        
        if (sameType)
        {
            // Make sure its the EXACT same type
            System.Type thisType =  typeof(HealingItem);

        }

        return true;

    }

    public virtual bool Equals(HealingItem otherItem)
    {
        bool sameBaseAtts =         base.Equals(otherItem);

        if (sameBaseAtts)
        {

        }
        return true;
    }
	
}
