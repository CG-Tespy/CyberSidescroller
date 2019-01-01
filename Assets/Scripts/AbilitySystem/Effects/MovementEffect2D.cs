using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Game Effects/Movement2D", fileName = "NewMovementEffect2D")]
public class MovementEffect2D : GameEffect, IHasLocalTimeScale
{
    // To customize the effect in the inspector
    [SerializeField] float _localTimeScale =        1;

    [SerializeField] MovementAttributes howToApply;

    public MovementMethod method                    { get { return howToApply.method; } }

    [Tooltip("The states it changes in the animator, and how.")]
    [SerializeField] AnimatorStateChangeReference[] animStateChanges;

    Dictionary<MonoBehaviour, IEnumerator> clients = new Dictionary<MonoBehaviour, IEnumerator>();

    // Properties
    public float localTimeScale                     { get { return _localTimeScale; } }

    // Methods
  
    public override void Apply(MonoBehaviour client)
    {
        base.Apply(client);

        // Register a new job
        if (!clients.ContainsKey(client))
        {
            IEnumerator coroutine =             ApplyByCoroutine(client);
            client.StartCoroutine(coroutine);
            clients[client] =                   coroutine;
        }  
    }

    /// <summary>
    /// Executes movement based on the movement type specified and input sequence required.
    /// </summary>
    protected virtual IEnumerator ApplyByCoroutine(MonoBehaviour client)
    {
        // Get the necessary components
        Animator animator =                 client.GetComponent<Animator>();
        
        // For pausing just before execution
        while (localTimeScale == 0 || !isActive)
            yield return null;

        HandleMovement(client);
        
        // TODO: Have animation state changes be their own effect not intrinsically-tied to movement effects
        if (animator != null)
            foreach (AnimatorStateChangeReference stateChange in animStateChanges)
                stateChange.value.Execute(animator);
        
        // For pausing just after execution
        while (localTimeScale == 0 || !isActive)
            yield return null;

        ApplyEnd.Invoke(client);
        clients.Remove(client);

    }

    
    // Helper functions
    void HandleMovement(MonoBehaviour client)
    {
        // Get the necessary components
        Transform transform =               client.GetComponent<Transform>();
        Rigidbody2D rigidbody =             client.GetComponent<Rigidbody2D>();

        if (rigidbody == null) // Safety.
            NullComponentAlert(client, "Rigidbody2D");

        Vector2 toApply =                       howToApply.movementVector * localTimeScale;
        
        switch (method)
        {
            case MovementMethod.setVelocity:
                HandlePhysicsMovement(rigidbody, method);
                break;

            case MovementMethod.addForce:
                HandlePhysicsMovement(rigidbody, method);
                break;

            case MovementMethod.translate:
                transform.Translate(toApply * Time.deltaTime);
                break;

            case MovementMethod.setPosition:
                transform.position =            howToApply.movementVector;
                break;

            default:
                Debug.LogWarning(method.ToString() + " application not implemented.");
                break;
        }
    }

    void HandlePhysicsMovement(Rigidbody2D rigidbody, MovementMethod moveMethod)
    {
        // Set the velocity (or add the force) on the axes the howToApply object dictates
        

        Vector2 toApply =                       rigidbody.velocity;

        if (howToApply.applyX)
            toApply.x =                         howToApply.movementVector.x;
        if (howToApply.applyY)
            toApply.y =                         howToApply.movementVector.y;

        toApply *=                              localTimeScale;

        switch (moveMethod)
        {
            case MovementMethod.setVelocity:
                rigidbody.velocity =            toApply;
                break;
            case MovementMethod.addForce:
                rigidbody.AddForce(toApply);
                break;
            default:
                string errMessage =             "Movement method {0} not accounted for in handling physics movement.";
                throw new System.ArgumentException(errMessage);
        }

        
    }

    void NullComponentAlert(MonoBehaviour client, string componentName)
    {
        string format =                         "Cannot apply MovementEffect2D {0} to {1}; {1} doesn't " +
                                                "have a {2} component.";
        string errMessage =                     string.Format(format, this.name, client.name, componentName);
        throw new System.ArgumentException(errMessage);
    }

}


// Movement-related enums
public enum MovementMethod
{
    setVelocity, addForce, setPosition, translate
}