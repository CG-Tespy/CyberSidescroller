using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Events;

public abstract class UIElementController : MonoBehaviourUI
{
	public virtual bool mouseOverThis 						{ get; protected set; }
	public virtual bool isFocused 							{ get; protected set; }

	// Callbacks
	UnityAction mouseEnter, mouseExit;
	UnityAction<int> mouseClicked;			

	protected override void Awake()
	{
		base.Awake();
		mouseOverThis = 									false;
		isFocused = 										false;
		SetupCallbacks();
	}

	protected virtual void UpdateFocus()
	{
		bool leftClicked = 									Input.GetMouseButtonDown(0);

		if (leftClicked && !mouseOverThis)
			isFocused = 									false;
	}

	protected override void OnMouseDown()
	{
		base.OnMouseDown();
		if (this.gameObject.activeSelf)
			isFocused = 									true;
	}

	void SetupCallbacks()
	{
		// Make sure that the right state vars are set based on the mouse's interactions 
		// (or lack thereof) with this UI element.
		mouseEnter = 										() => mouseOverThis = true;
		mouseExit = 										() => mouseOverThis = false;
		mouseClicked = 										(int i) => isFocused = mouseOverThis;
		
		events.MouseEntered.AddListener(mouseEnter);
		events.MouseExited.AddListener(mouseExit);
		events.MouseClicked.AddListener(mouseClicked);
	}
}
