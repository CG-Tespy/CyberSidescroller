using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;


public class HealthPickup2D : Pickup2D 
{
    [SerializeField] 
    new protected PickupType _type =          PickupType.health;
    // ^ Made this new so the default set shows up in the inspector
    [SerializeField] float _healAmount =      5;
    public override PickupType type           
    { 
        get 
        { 
            return              this._type; 
            // ^To make sure this class' version of _type is returned
        } 
    }
    
    public float healAmount                   { get { return _healAmount; } }

    public override bool PickUp(MonoBehaviour pickuper)
    {
        // If something that can be healed picked this up, heal it
        IHasHP toHeal =             pickuper as IHasHP;
        bool healPickuper =         toHeal != null;

        if (healPickuper)
            toHeal.TakeHealing(healAmount);
        
        PickedUp.Invoke();
        
        return true;
    }

}
