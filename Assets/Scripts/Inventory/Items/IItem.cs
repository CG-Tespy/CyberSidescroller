using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// 
/// </summary>
/// <typeparam name="T">Type of objects the item can be used on.</typeparam>
public interface IItem<T>
{
	bool UseOn(T toUseOn);
}
