using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

/// <summary>
/// Based off the FloatReference script as shown in 
//  Unite Austin 2017 : Game Architecture with Scriptable Objects.
//  TVarType is the type of the var to reference, TVarType2 is the variable object for that type.
/// </summary>
public abstract class VariableReference<TVarType, TVarType2> 
where TVarType2: VariableObject<TVarType>
{
	[SerializeField] protected bool useSimpleValue = 			true;
	[SerializeField] protected TVarType simpleValue;
	[SerializeField] protected TVarType2 variable;

	public virtual TVarType value 
	{
		get { return useSimpleValue ? simpleValue : variable.value; }
		set
		{
			if (useSimpleValue)
				simpleValue = 								value;
			else 
				variable.value = 							value;
		}
	}

}
