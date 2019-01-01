using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;


/// <summary>
/// Just to help test shooting.
/// </summary>
public class EnemyDummy : CSS_MonoBehaviour2D, IDamageable<float>
{
	public UnityEvent TookDamage 				{ get; protected set; }
	[SerializeField] float _health = 			5;
	[SerializeField] float _maxHealth = 		5;

	public float health
	{
		get { return _health; }
		set 
		{
			_health = value; 

			if (health < 0)
				health = 		0;

			else if (health > _maxHealth)
				health =		 _maxHealth;

		}
	}

	public float maxHealth 
	{
		get { return _maxHealth; }
		set 
		{
			_maxHealth = 			value;

			if (maxHealth < 0)
				maxHealth = 		0;

			else if (maxHealth < health)
				health =		 _maxHealth;

		}
	}
	public bool isInvincible 				{ get; protected set; }

	// Use this for initialization
	protected override void Awake () 
	{
		base.Awake();
		TookDamage = 						new UnityEvent();
		isInvincible = 						false;
	}
	
	public bool TakeDamage(float damageToTake, bool triggerInvin)
	{
		if (!isInvincible)
		{
			health -= 				damageToTake;
			TookDamage.Invoke();

			if (health <= 0)
			{
				Destroy(this.gameObject);
				return true;
			}

			if (triggerInvin)
				isInvincible = 		true;

			return true;
		}
		else 
			return false;
	}

}
