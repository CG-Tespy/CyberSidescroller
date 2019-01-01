using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;


[RequireComponent(typeof(Collider2D))]
public abstract class Pickup2D : CSS_MonoBehaviour2D
{
	public enum PickupType
	{
		health,
		weapon
	}
	public UnityEvent PickedUp 				{ get; protected set; }
	protected PickupType _type;
	public virtual PickupType type 			{ get { return _type; } }

	protected override void Awake()
	{
		base.Awake();
		PickedUp = 							new UnityEvent();
	}

	/// <summary>
	/// Gets picked up by the passed pickup-er. Returns true or false depending 
	/// on whether the picking-up was successful.
	/// </summary>
	public virtual bool PickUp(MonoBehaviour pickuper)
	{
		PickedUp.Invoke();
		return true;
	}

	
}
