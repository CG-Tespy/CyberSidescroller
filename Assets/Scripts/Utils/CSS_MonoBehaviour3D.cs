using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class CSS_MonoBehaviour3D : MonoBehaviour 
{
	new public Collider collider 				{ get; protected set; }
	new public Rigidbody rigidbody 				{ get; protected set; }
	public ContactEvents3D contactEvents 		{ get; protected set; }

	// Use this for initialization
	protected virtual void Awake () 
	{
		collider = 								GetComponent<Collider>();
		rigidbody = 							GetComponent<Rigidbody>();
		contactEvents = 						new ContactEvents3D();
	}
	

	// Contact event handlers
	protected virtual void OnCollisionEnter(Collision other)
	{
		contactEvents.CollisionEnter.Invoke(other);
	}

	protected virtual void OnCollisionStay(Collision other)
	{
		contactEvents.CollisionStay.Invoke(other);
	}

	protected virtual void OnCollisionExit(Collision other)
	{
		contactEvents.CollisionExit.Invoke(other);
	}

	protected virtual void OnTriggerEnter(Collider other)
	{
		contactEvents.TriggerEnter.Invoke(other);
	}

	protected virtual void OnTriggerStay(Collider other)
	{
		contactEvents.TriggerStay.Invoke(other);
	}

	protected virtual void OnTriggerExit(Collider other)
	{
		contactEvents.TriggerExit.Invoke(other);
	}
}
