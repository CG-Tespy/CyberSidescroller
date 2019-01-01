using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DeflectOnContact2D : MonoBehaviour 
{
	[Tooltip("Anything with these tags can be deflected by this object.")]
	[SerializeField] List<string> contactTags;
	[Tooltip("Anything with a layer among these can be deflected by this object.")]
	[SerializeField] LayerMask contactLayers;

	void Awake()
	{
		// Make sure the game object this is attached to has a collider
		Collider2D coll = 				GetComponent<Collider2D>();

		if (coll == null)
		{
			string message = 			this.name + "has no Collider2D component; it cannot deflect anything.";
			throw new System.NullReferenceException(message);
		}
	}

	void OnCollisionEnter2D(Collision2D other)
	{
		HandleDeflection(other.gameObject);
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		HandleDeflection(other.gameObject);
	}

	void HandleDeflection(GameObject toDeflect)
	{
		Rigidbody2D rb2d = 		toDeflect.GetComponent<Rigidbody2D>();

        // Simply deflect the other object by reversing its velocity
        if (rb2d != null && ShouldDeflect(toDeflect))
        {
            rb2d.velocity *= -1;

            if (toDeflect.tag == "Enemy") {
                toDeflect.tag = "PAttack";
                toDeflect.layer = 13;
            }
        }

	}

	bool ShouldDeflect(GameObject other)
	{
		bool hasRightTag = 				contactTags.Contains(other.tag);
		bool inRightLayer = 			contactLayers.ContainsLayer(other.layer);

		return hasRightTag || inRightLayer;
	}
}
