using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


/// <summary>
/// Interface for objects that keep track of what colliders they're touching at any given time.
/// </summary>
public interface ICollidersTouchingHandler2D
{
	List<Collider2D> collidersTouching { get; }
}
