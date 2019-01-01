using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class DamageOnContact2D : CSS_MonoBehaviour2D
{
	[Tooltip("Anything with these tags can be damaged by this object.")]
	[SerializeField] List<string> contactTags;
	[Tooltip("Anything with a layer among these can be damaged by this object.")]
	[SerializeField] LayerMask contactLayers;
	public float damageDealt = 		5;
	[SerializeField] Vector3 forceOnContact = 	new Vector3(0, 200, 0);

	// Use this for initialization
	
	
	protected override void OnTriggerEnter2D(Collider2D other)
	{
		base.OnTriggerEnter2D(other);

		//Debug.Log(this.name + " is colliding with " + other.name);

		// If this touches something damageable, damage it
		IDamageable<float> damageable = 		other.GetComponent<IDamageable<float>>();
		bool touchedDamageable = 				damageable != null;

		if (touchedDamageable && ShouldDamage(other.gameObject))
		{
			// Apply a force to the damageable object (if possible) and try to damage it
			Rigidbody2D otherRb = 				other.GetComponent<Rigidbody2D>();

			// Avoid refreshing the object's invincibility if it is active
			if (otherRb != null && !damageable.isInvincible)
			{
				otherRb.velocity = 				forceOnContact * Time.deltaTime;
				// ^ Make sure the force isn't negated by other forces on the rigidbody

				damageable.TakeDamage(damageDealt, true);	
			}
		}
	}
	
	protected override void OnTriggerStay2D(Collider2D other)
	{
		base.OnTriggerStay2D(other);

		OnTriggerEnter2D(other);
	}

	bool ShouldDamage(GameObject other)
	{
		bool hasRightTag = 					contactTags.Contains(other.tag);
		bool inRightLayer = 				contactLayers.ContainsLayer(other.layer);

		return hasRightTag || inRightLayer;
	}


}
