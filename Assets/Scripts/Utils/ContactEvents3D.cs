using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class ContactEvents3D
{
	public class CollisionEvent : UnityEvent<Collision> {}
	public class TriggerEvent : UnityEvent<Collider> {}

	public CollisionEvent CollisionEnter 		{ get; protected set; }
	public CollisionEvent CollisionStay 		{ get; protected set; }
	public CollisionEvent CollisionExit 		{ get; protected set; }

	public TriggerEvent TriggerEnter 			{ get; protected set; }
	public TriggerEvent TriggerStay 			{ get; protected set; }
	public TriggerEvent TriggerExit 			{ get; protected set; }

	public ContactEvents3D()
	{
		CollisionEnter = 						new CollisionEvent();
		CollisionStay = 						new CollisionEvent();
		CollisionExit = 						new CollisionEvent();

		TriggerEnter = 							new TriggerEvent();
		TriggerStay = 							new TriggerEvent();
		TriggerExit = 							new TriggerEvent();
	}
}
