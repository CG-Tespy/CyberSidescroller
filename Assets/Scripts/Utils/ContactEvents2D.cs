using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;




public class ContactEvents2D
{
	public class CollisionEvent2D : UnityEvent<Collision2D> {}
	public class TriggerEvent2D : UnityEvent<Collider2D> {}

	// 2D Contact Events
	public CollisionEvent2D CollisionEnter2D 	{ get; protected set; }
	public CollisionEvent2D CollisionStay2D 	{ get; protected set; }
	public CollisionEvent2D CollisionExit2D 	{ get; protected set; }

	public TriggerEvent2D TriggerEnter2D 		{ get; protected set; }
	public TriggerEvent2D TriggerStay2D 		{ get; protected set; }
	public TriggerEvent2D TriggerExit2D 		{ get; protected set; }

	public ContactEvents2D()
	{
		CollisionEnter2D = 						new CollisionEvent2D();
		CollisionStay2D = 						new CollisionEvent2D();
		CollisionExit2D = 						new CollisionEvent2D();

		TriggerEnter2D = 						new TriggerEvent2D();
		TriggerStay2D = 						new TriggerEvent2D();
		TriggerExit2D = 						new TriggerEvent2D();
	}
	
}
