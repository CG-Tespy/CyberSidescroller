using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class IntEvent : UnityEvent<int> {}

public class UIEvents
{
	public UnityEvent MouseEntered 							{ get; protected set; }
	public UnityEvent MouseExited 							{ get; protected set; }
	public IntEvent MouseClicked 							{ get; protected set; }
	public UnityEvent MouseOver 							{ get; protected set; }
	public UnityEvent MouseReleased 						{ get; protected set; }
	public UnityEvent MouseDragged 							{ get; protected set; }
	public UIEvents()
	{
		MouseEntered = 										new UnityEvent ();
		MouseExited = 										new UnityEvent ();
		MouseClicked = 										new IntEvent();
		MouseOver = 										new UnityEvent();
		MouseReleased = 									new UnityEvent();
		MouseDragged = 										new UnityEvent();
	}
	
}
