using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;


public class VariableHPBar : MonoBehaviour 
{
	[SerializeField] Image healthFill;
	[SerializeField] FloatReference health;
	//[SerializeField] FloatReference maxHealth;
	[Tooltip("How long, in seconds, it takes for the hp bar to get to the appropriate fill amount.")]
	[SerializeField] float timeToFill = 			1;
	float fillAmount 								
	{
		 get { return healthFill.fillAmount; } 
		 set { healthFill.fillAmount = value; }
	}
	float timer = 									0; // For the fill progress
	

	// Use this for initialization
	void Awake () 
	{
		if (healthFill == null)
			throw new System.NullReferenceException(name + " needs a health fill image!");
		
	}

	void Start()
	{
		ListenForValueChange();
	}
	
	// Update is called once per frame
	void LateUpdate () 
	{
		UpdateFill();
		
	}

	void ListenForValueChange()
	{
		health.ValueChanged.AddListener(OnHealthChange);
		health.MaxValueChanged.AddListener(OnHealthChange);
	}

	void OnHealthChange(float newValue)
	{
		// Reset the timer for the coroutine, to help keep the fill progress constant
		timer = 							0;
	}

	void UpdateFill()
	{
		float frameRate = 				1f / Time.deltaTime;
		float framesToPass = 			frameRate * timeToFill;

		float targetFill = 				health.value / health.maxValue;

		// Linearly interpolate the fill amount to the target fill over time
		if (framesToPass == 0) // Avoid dividing by 0
			fillAmount = 				targetFill;
		else
		{
			if (timer < framesToPass)
				timer	+= 				Time.deltaTime;
				
			fillAmount = 				Mathf.Lerp(fillAmount, targetFill, timer / framesToPass);
		}
	}


}
