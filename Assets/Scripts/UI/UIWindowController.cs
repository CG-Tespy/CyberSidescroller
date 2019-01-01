using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class UIWindowController : UIElementController
{
	// Events 
	public UnityEvent Opened 								{ get; protected set; }
	public UnityEvent Closed 								{ get; protected set; }

	// Fields
	[SerializeField] Text titleText;

	// Properties
	public bool isOpen 										{ get; protected set; }
	public string title
	{
		get 												
		{ 
			if (titleText != null)
				return titleText.text;
			else 
				return null;
		}
		set 												
		{ 
			if (titleText != null)
				titleText.text = value; 
			else 
				Debug.LogWarning("Tried to set title of UI Window with no TitleText set.");
		}
	}

	// Methods

	// Use this for self-initialization
	protected override void Awake () 
	{
		base.Awake();
		Opened = 											new UnityEvent();
		Closed = 											new UnityEvent();
	}
	
	public virtual void Open()
	{
		gameObject.SetActive(true);
		isOpen = 											true;
		Opened.Invoke();
	}

	public virtual void Close()
	{
		gameObject.SetActive(false);
		isOpen = 											false;
		Closed.Invoke();
	}
	
}
