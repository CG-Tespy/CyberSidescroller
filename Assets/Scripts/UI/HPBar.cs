using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class HPBar : CSS_MonoBehaviourUI
{
	SidescrollerCharacter player;
	[SerializeField] Image healthFill;
	[SerializeField] float fillSpeed = 				2;
	float fillThresh = 								0.01f;
	public float fillAmount 						{ get { return healthFill.fillAmount; } }

	// Use this for initialization
	protected override void Awake () 
	{
		base.Awake();
		player = 									GameObject.FindObjectOfType<SidescrollerCharacter>();
		StartCoroutine(TrackHP());
	}
	

	IEnumerator TrackHP()
	{

		while (true)
		{
			float targetFill = 				player.hp / player.maxHp;

			if (Mathf.Abs(healthFill.fillAmount - targetFill) <= fillThresh)
				healthFill.fillAmount = 	targetFill;
			else
				healthFill.fillAmount = 	Mathf.Lerp(healthFill.fillAmount, 
														targetFill, 
														Time.deltaTime * fillSpeed);
			
			yield return null;
		}

	}
}
