using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TeaspoonTools.Utils;

/// <summary>
/// Base class for all enemy-controllers.
/// </summary>
public abstract class EnemyController : CSS_MonoBehaviour2D, IPausable, IDamageable<float>
{
	public UnityEvent Died 					{ get; protected set; }
	//IPausable
	public UnityEvent Paused 				{ get; protected set; }
	public UnityEvent Unpaused 				{ get; protected set; }
	public bool isPaused 					{ get; protected set; }

	// IDamageable
	public bool isInvincible				{ get; protected set; }
	public UnityEvent TookDamage 			{ get; protected set; }
	public FloatReference hp, damage;
	//public float hp, maxHp, damage;

	public virtual bool dead 				{ get { return hp.value <= 0; } }
	public bool attacking = 		false;

	[SerializeField] LootDropSet lootDropSet;
	List<GameObject> droppedItems = new List<GameObject>();
	// ^ To prevent OnDestroy problems
	

	protected override void Awake()
	{
		base.Awake();
		Died = 					new UnityEvent();
		Paused = 				new UnityEvent();
		Unpaused = 				new UnityEvent();
		isPaused = 				false;
		isInvincible = 			false;
		TookDamage = 			new UnityEvent();
	}

	public virtual void Pause()
	{
		isPaused = 		true;
		Paused.Invoke();
	}

	public virtual void Unpause()
	{
		isPaused = 		false;
		Unpaused.Invoke();
	}

	public virtual bool TakeDamage(float damageToTake, bool triggerInvin = false)
	{
		if (triggerInvin)
			isInvincible = 			true;

		if (isInvincible)
			return false;

		hp.value -= 				damageToTake;
		TookDamage.Invoke();

		if (dead)
			Died.Invoke();
			
		return true;
	}

	void OnDestroy()
	{
		if (lootDropSet != null)
			Invoke("DropItems", 0.25f);
	}

	void OnApplicationQuit()
	{
		foreach (GameObject item in droppedItems)
			if (item != null)
				Destroy(item);
	}

	void DropItems()
	{
		droppedItems = new List<GameObject>(lootDropSet.DropItem(this));
	}

}
