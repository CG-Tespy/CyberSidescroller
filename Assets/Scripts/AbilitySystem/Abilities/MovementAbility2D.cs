using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;


[CreateAssetMenu(menuName = "Abilities/Movement2D", fileName = "NewMovementAbility2D")]
public class MovementAbility2D : Ability
{
	//[System.Serializable]
	public enum MovementType
	{
		horizontal, vertical, none
	}

	// Fields
	
	//[SerializeField] 
	//float _localTimeScale = 				1;
	
	[SerializeField] MovementType[] _movementTypes;
	//[SerializeField] new MovementEffect2D[] effects;
	
	// Properties
	public MovementType[] movementTypes 				{ get { return _movementTypes; } }
	//public float localTimeScale 			
	//{
	//	get 											{ return _localTimeScale; }
	//	protected set 									{ _localTimeScale = value; }
	//}

	protected float timer = 		0;
	// Methods

	public override void Apply(MonoBehaviour toApplyTo)
	{
		base.Apply(toApplyTo);

		if (!isActive)
			return;
		
		foreach (GameEffect effect in effects)
			effect.Apply(toApplyTo);
		
	}

}
