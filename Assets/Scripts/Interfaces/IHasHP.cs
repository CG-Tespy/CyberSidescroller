using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public interface IHasHP
{
	float hp { get; }
	float maxHp { get; }

	bool TakeHealing(float healingAmount);
}
