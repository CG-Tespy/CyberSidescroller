using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


[CreateAssetMenu(menuName = "Conditions/Collision", fileName = "NewCollisionCondition")]
public class CollisionCondition : Condition
{
    // Customize in the Inspector
    
    [Tooltip("Tags of what objects this condition requires collision with or avoidance thereof.")]
    [SerializeField] List<string> tags;

    [Tooltip("Layers of what objects this condition requires collision with or avoidance thereof.")]
    [SerializeField] LayerMask layerMask;

    [Tooltip("Whether or not this condition is for avoiding a collision.")]
    [SerializeField] bool avoidCollision =                      false;
    
    [Tooltip("If true, this ignores the tags array when evaluating.")]
    [SerializeField] bool ignoreTags =                         false;
    [Tooltip("If true, this ignores the layer mask when evaluating.")]
    [SerializeField] bool ignoreLayers =                       false;

    // Methods

    public override bool Evaluate<T>(T toEvaluate)
    {
        // Only evaluate objects that keep track of what colliders they're touching
        ICollidersTouchingHandler2D gameObject =    toEvaluate as ICollidersTouchingHandler2D;
        bool evaluateThis =                         gameObject != null;

        if (!evaluateThis)
        {
            string errMessage =             name + " can only evaluate objects that ";
            errMessage +=                   "implement the interface ICollidersTouchingHandler2D.";
            throw new System.ArgumentException(errMessage);
        }
        else 
        {
            if (avoidCollision)
                return CollisionAvoided(gameObject.collidersTouching);
            else 
                return !CollisionAvoided(gameObject.collidersTouching);
        }
    }
    
    protected virtual bool CollisionAvoided(List<Collider2D> colliders)
    {
        // If the colliders being touched have tags within the list, or layers within the layer mask, 
        // then the collision was NOT avoided.
        // Note that all depends whether the tags or layers in this condition instance are being 
        // ignored.
        foreach (Collider2D coll in colliders)
        {
            if (!ignoreTags)
                if (tags.Contains(coll.tag))
                    return false;

            if (!ignoreLayers)
                if (layerMask.ContainsLayer(coll.gameObject.layer))
                    return false;
            
        }

        return true;
    }
	
}
