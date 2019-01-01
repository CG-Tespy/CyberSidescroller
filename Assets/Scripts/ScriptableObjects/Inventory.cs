using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Inventory : ScriptableObject 
{
    [SerializeField] List<Item> items =     new List<Item>();
    [Tooltip("How many items this inventory can hold. If negative, this can hold infinite items.")]
    [SerializeField] int capacity =         -1;

    public bool infiniteCapacity            { get { return capacity < 0; } }
    // Methods

    public bool Add(Item item)
    {
        bool thereIsSpace =                 infiniteCapacity || items.Count < capacity;

        if (thereIsSpace)
        {
            items.Add(item);
            return true;
        }
        else 
        {
            string warningFormat =          "{0} couldn't be added to {1}; not enough space.";
            string warning =                string.Format(warningFormat, item.name, this.name);
            Debug.LogWarning(warning);
        }

        return thereIsSpace;
    }
	
}
