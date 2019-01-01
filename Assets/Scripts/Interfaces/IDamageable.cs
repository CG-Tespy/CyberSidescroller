using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;


public interface IDamageable<T>
{
	UnityEvent TookDamage 			{ get; }
	bool isInvincible 				{ get; }
	bool TakeDamage(T damageToTake, bool triggerInvin);

}
