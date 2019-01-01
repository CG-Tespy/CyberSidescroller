using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Game Effects/Effect Chain", fileName = "NewGameEffectChain")]
public class GameEffectChain : GameEffect
{
	// To customize in the Inspector
	[SerializeField] protected GameEffect[] effects;
	
	//[Tooltip("If false, the effects in the list execute from top to bottom. Otherwise, bottom to top.")]
	//[SerializeField] protected bool reverseExecution = 		false;
	[Tooltip("Whether to execute the effects at the same time, regardless of any delay effects involved.")]
	[SerializeField] protected bool simultaneous = 			false;

	// Helps do the job
	protected Dictionary<MonoBehaviour, IEnumerator> clients = 		
	new Dictionary<MonoBehaviour, IEnumerator>();
	// ^ To keep track of which objects are applying this effect

	// Methods
	public override void Init()
	{
		base.Init();
		InitEffects();
	}

	public override void Apply(MonoBehaviour client)
	{
		if (!isActive)
			return;

		base.Apply(client);

		// Check if this chain is already being applied to the passed client.
		if (!clients.ContainsKey(client)) // If not, start applying this to it.
		{
			IEnumerator coroutine = 		GoThroughChain(client);
			clients[client] = 				coroutine;
			client.StartCoroutine(coroutine);
		}
		
	}

	protected virtual IEnumerator GoThroughChain(MonoBehaviour client)
	{
		//Debug.Log("In GoThroughChain()");

		yield return client.StartCoroutine(PauseIfNecessary());

		// Decide how to apply the effects.
		IEnumerator applyEffects = 		null;
		if (simultaneous)
			applyEffects = 				ApplySimultaneously(client);
		else
			applyEffects = 				ApplyOneByOne(client);

		yield return client.StartCoroutine(applyEffects);
		
		// The whole chain was executed by this point, so... job's done!
		clients.Remove(client); 
		ApplyEnd.Invoke(client);
	}

	public virtual void OnDisable()
	{
		// Cancel all the jobs
		IEnumerator coroutine = 		null;
		foreach (MonoBehaviour client in clients.Keys)
		{
			coroutine = 				clients[client];
			client.StopCoroutine(coroutine);
		}

		clients.Clear();
	}

	// Helpers
	protected virtual void InitEffects()
	{
		foreach (GameEffect effect in effects)
			effect.Init();
	}

	protected virtual IEnumerator ApplySimultaneously(MonoBehaviour client)
	{
		// Applies all the effects in the same frame.
		Debug.Log(name + " executing effects simultaneously.");
		foreach (GameEffect effect in effects)
		{
			yield return client.StartCoroutine(PauseIfNecessary());
			effect.Apply(client);
		}
	}

	protected virtual IEnumerator ApplyOneByOne(MonoBehaviour client)
	{
		// Applies the effects one by one, applying the next in the list when the one 
		// before it is done.
		bool applyingEffect = 						false;
		UnityAction<MonoBehaviour> onApplyEnd = 	(MonoBehaviour mb) => 
													{ 
														if (mb == client)
															applyingEffect = false; 
													};

		foreach (GameEffect effect in effects)
		{
			yield return client.StartCoroutine(PauseIfNecessary());

			// Make sure to know when an effect is done
			effect.ApplyEnd.AddListener(onApplyEnd);
			applyingEffect = 				true;
			effect.Apply(client);

			while (applyingEffect)
				yield return null;

			// Here, we're about to move to the next effect. And thus we won't 
			// need to listen for the current effect to end anymore.
			effect.ApplyEnd.RemoveListener(onApplyEnd);
			
			yield return client.StartCoroutine(PauseIfNecessary());

		}
	}


}
