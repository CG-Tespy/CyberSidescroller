using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class StationaryEnemy : EnemyController
{
	protected override void Update()
	{
		base.Update();
		
		if (dead)
			Destroy(this.gameObject);
	}
}
