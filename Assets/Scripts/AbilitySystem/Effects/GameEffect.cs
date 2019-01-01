using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class MonoBehaviourEvent : UnityEvent<MonoBehaviour> {}

public abstract class GameEffect : ScriptableObject
{

	// To customize in the Inspector
	[Tooltip("If false, this effect cannot execute.")]
	[SerializeField]
	protected bool _isActive = 				true;

	/*
	[Tooltip("Decides whether each object has its own instance of this.")]
	[SerializeField] 
	protected bool global = 				false;
	*/

	// Fields
	public readonly UnityEvent Deactivated = 		new UnityEvent();
	public readonly MonoBehaviourEvent ApplyEnd = 	new MonoBehaviourEvent();

	// Properties
	public virtual bool isActive 
	{ 
		get 										{ return _isActive; } 
		protected set 								{ _isActive = value; }
	}

	// Methods
	public virtual void Init()
	{
		Debug.Log("Initializing " + name);
	}

	public virtual void Apply(MonoBehaviour client)
	{
		if (!isActive)
		{
			string message = 				"Failed to execute Ability Effect " + name;
			message += 					 	"; it is inactive.";
			Debug.LogWarning(message);
			return;
		}
		else
		{ 
			Debug.Log("Executing ability effect " + name);
		}
	}

	public virtual void SetActive(bool active)
	{
		if (active == false && _isActive != false)
			Deactivated.Invoke();

		_isActive = 							active;
	}

	//public abstract void Copy();

	protected virtual IEnumerator PauseIfNecessary()
	{
		while (!isActive)
			yield return null;
	}

}
