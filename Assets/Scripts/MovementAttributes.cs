using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MovementAttributes 
{
	
	[Header("Axes to apply this change to.")]
	public bool applyX = 						true;
	public bool applyY = 						true;
	public bool applyZ = 						true;

	[Tooltip("Speed of movement (per second) on each axis.")]
	[SerializeField] Vector3 _movementVector;

	[Header("Way the movement vector will be applied.")]
	[SerializeField] MovementMethod _method = 	MovementMethod.setVelocity;

	public Vector2 movementVector 				{ get { return _movementVector; } }
	public MovementMethod method				{ get { return _method; } }
	
}
