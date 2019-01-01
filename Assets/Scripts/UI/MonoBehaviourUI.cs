using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Base class for all UI MonoBehaviours.
/// </summary>
[RequireComponent(typeof(RectTransform))]
public abstract class MonoBehaviourUI : MonoBehaviour 
{
	public RectTransform rectTransform 							{ get; protected set; }
	public CanvasRenderer canvasRenderer 						{ get; protected set; }
	public UIEvents events 										{ get; protected set; }
	const int leftClick = 										0;
	const int rightClick = 										1;

	// Use this for self-initialization
	protected virtual void Awake () 
	{
		rectTransform = 										GetComponent<RectTransform>();
		canvasRenderer = 										GetComponent<CanvasRenderer>();
		events = 												new UIEvents();
	}
	
	#region Mouse Event handlers
	protected virtual void OnMouseEnter()
	{
		events.MouseEntered.Invoke();
	}

	protected virtual void OnMouseExit()
	{
		events.MouseExited.Invoke();
	}

	protected virtual void OnMouseDown()
	{
		int mouseButtonClicked = 											0;

		if (Input.GetMouseButtonDown(leftClick))
			mouseButtonClicked = 											leftClick;
		else if (Input.GetMouseButtonDown(rightClick))
			mouseButtonClicked = 											rightClick;

		events.MouseClicked.Invoke(mouseButtonClicked);
	}

	protected virtual void OnMouseUpAsButton()
	{
		events.MouseReleased.Invoke();
	}

	protected virtual void OnMouseDrag()
	{
		events.MouseDragged.Invoke();
	}
	#endregion
	
}
