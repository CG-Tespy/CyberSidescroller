using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Inventory/Items/Effects/HealthHealing", fileName = "NewHealthHealingEffect")]
public class HealthHealingEffect : GameEffect
{
    [SerializeField] int _healAmount =                      10;

    public int healAmount                                   { get { return _healAmount; } }

    public override void Apply(MonoBehaviour client)
    {
        // Make sure the client can be healed
        bool intHealable =                  client as IHealable<int> != null;
        bool floatHealable =                client as IHealable<float> != null;

        bool canHealClient =                intHealable || floatHealable;
        if (canHealClient)
        {
            if (intHealable)
            {
                IHealable<int> toHeal =     client as IHealable<int>;
                toHeal.TakeHealing(healAmount);
            }
            else if (floatHealable)
            {
                IHealable<float> toHeal =   client as IHealable<float>;
                toHeal.TakeHealing(healAmount);
            }
        }
        else 
        {
            // Report being unable to heal the client
            string format =                 "Healing effect {0} could not be applied on {1}; {1} does " +
                                            "not implement IHealable in a way that {0} supports.";
            string errMessage =     string.Format(format, this.name, client.name);
            throw new System.ArgumentException(errMessage);
        }
    }
	
}
