using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

/// <summary>
/// Contains fields and such that we want all our custom UI Monobehaviours to 
/// have.
/// </summary>
public class CSS_MonoBehaviourUI : MonoBehaviour 
{
	public RectTransform rectTransform 			{ get; protected set; }
	public CanvasRenderer canvasRenderer 		{ get; protected set; }

	// Use this for initialization
	protected virtual void Awake () 
	{
		rectTransform = 						GetComponent<RectTransform>();
		canvasRenderer = 						GetComponent<CanvasRenderer>();
	}
	
}
