using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public static class ICollectionExtensions
{
	/// <summary>
	/// AddRange that lets you pick up to how many elements to add.
	/// </summary>
	/// <param name="elementsToAdd">If this is below 0, all the elements are added.</param>
	public static void AddRange<T>(this ICollection<T> collection, ICollection<T> toAddFrom, 
										int elementAddLimit = -1)
	{
		int elementsAdded = 			0;

		foreach (T item in toAddFrom)
		{
			if (elementsAdded == elementAddLimit)
				break;

			collection.Add(item);
			elementsAdded++;
		}
	}

	/// <summary>
	/// Looks through the collection, searching for any items of the passed type.
	/// </summary>
	/// /// <param name="exactType">Whether or not to check only for the exact type passed.</param>
	/// <returns>True if any items were found, false otherwise.</returns>
	public static bool ContainsObjectOfType<T>(this ICollection<T> collection, 
												System.Type typeToCheckFor, bool onlyExactType = false)
												where T: System.Type
	{
		// In case the collection is empty.
		if (collection.Count == 0)
			return false;

		// All items in any generic collection are the specified generic type, so get that.
		System.Type	itemType = 			null;

		foreach (T item in collection)
		{
			itemType = 					item.GetType();
			break;
		}		

		// Compare the types.
		if (onlyExactType)
			if (itemType.Equals(typeToCheckFor))
				return true;

		else
			if (typeToCheckFor.IsAssignableFrom(itemType))
				return true;

		return false;
	}

	/// <summary>
	/// From the current collection, returns another with elements that are assignable
	/// from the passed type.
	/// </summary>
	/// <param name="familyTreeOnly">Whether or not to only count types in the same family tree as the passed one.</param>
	public static ICollection<T> GetContentsOfType<T>(this ICollection<T> collection, 
													System.Type typeToCheckFor,
													bool familyTreeOnly = false)
	{
		ICollection<T> contents = 		new List<T>();

		// In case the collection has no items.
		if (collection.Count == 0)
			return contents;

		// All items in any generic collection are the specified generic type, so get that.
		System.Type	itemType = 			null;

		foreach (T item in collection)
		{
			itemType = item.GetType();
			break;
		}	

		// Extract the contents, if any.
		foreach (T item in collection)
		{
			if (familyTreeOnly)
				if (itemType.Equals(typeToCheckFor) || itemType.IsSubclassOf(typeToCheckFor))
					contents.Add(item);
			
			else
				if (typeToCheckFor.IsAssignableFrom(itemType))
					contents.Add(item);
		}

		return contents;
	}

}
