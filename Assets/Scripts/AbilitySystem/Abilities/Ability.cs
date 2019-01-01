using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TeaspoonTools.Utils;


/// <summary>
/// Abilities are effects or chains of effects that require input to execute.
/// </summary>
public abstract class Ability : GameEffectChain
{

	// Customize in the inspector
	[Tooltip("Inputs needed to execute this ability, if any.")]
    [SerializeField] protected PlayerInput[] inputSequence;

	// Fields
	protected Dictionary<MonoBehaviour, PlayerInputSequenceChecker> sequenceCheckers = 
	new Dictionary<MonoBehaviour, PlayerInputSequenceChecker>();

	// Properties
	protected List<MonoBehaviour> inputBypassers = 		new List<MonoBehaviour>();
	// ^ For those that don't need to do the input sequence to make this ability do its 
	// thing for them.

	// Methods

	public override void Apply(MonoBehaviour client)
	{
		if (!isActive)
		{
			Debug.Log(name + " is inactive");
			return;
		}

		//Debug.Log("Executing " + name + " ability.");

		// Set up the sequence checker responding to the client's inputs, if this ability has 
		// any.
		bool thereAreInputs = 				inputSequence != null && inputSequence.Length > 0;
		bool newJob = 						!sequenceCheckers.ContainsKey(client);
		bool notInputBypasser = 			!inputBypassers.Contains(client);
		if (thereAreInputs && newJob && notInputBypasser)
		{
			PlayerInputSequenceChecker seqChecker = 	SetupSequenceChecker(client);
			sequenceCheckers[client] = 					seqChecker;
			seqChecker.isChecking = 					true;
		}
		else 
		{
			ApplyEffects(client);
		}

	}

	/// <summary>
	/// Enables the client to not need to execute the input sequence to make this ability
	/// work for it.
	/// </summary>
	public void BypassInputsFor(MonoBehaviour client)
	{
		inputBypassers.Add(client);

		if (sequenceCheckers[client] != null)
		{
			sequenceCheckers[client].Dispose();
			sequenceCheckers.Remove(client);
		}
	}

	/// <summary>
	/// Makes it so the client has to execute the input sequence to make this ability work
	/// for it. Not that it is that way by default.
	/// </summary>
	public void EnforceInputsFor(MonoBehaviour client)
	{
		inputBypassers.Remove(client);
	}

	protected virtual void ApplyEffects(MonoBehaviour client)
	{
		// If this is a new job from the client, start it up
		if (!clients.ContainsKey(client) || clients[client] == null) 
		{
			IEnumerator coroutine = 		GoThroughChain(client);
			clients[client] = 				coroutine;
			client.StartCoroutine(coroutine);
		}
	}

	protected virtual PlayerInputSequenceChecker SetupSequenceChecker(MonoBehaviour client)
    {
		PlayerInputSequenceChecker seqChecker = 	null;      

        if (inputSequence != null)
        {
            // Bind the sequence to a checker, and execute the effect when it reports
            // success on inputting that sequence
            seqChecker = 			new PlayerInputSequenceChecker(client, inputSequence);
            //seqChecker.Init(user, inputSequence);

            UnityAction logSuccess = () =>
            {
                Debug.Log(this.name + " executed successfully");
            };
            
            //seqChecker.Success.AddListener(logSuccess);
            //seqChecker.Success.AddListener(applyByCoroutine);
			seqChecker.Success.AddListener(() => ApplyEffects(client));
        }

		return seqChecker;
    }	

	public override void OnDisable()
	{
		base.OnDisable();

		inputBypassers.Clear();

		// Stop any coroutines this ability got running through sequence checkers
		foreach (MonoBehaviour client in sequenceCheckers.Keys)
			sequenceCheckers[client].Dispose();
	
		sequenceCheckers.Clear();
	}	


	
}
