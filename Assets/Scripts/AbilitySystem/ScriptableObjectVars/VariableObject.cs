using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;


public abstract class VariableObject<T> : ScriptableObject
{
	public T value;
	
}

public abstract class NumericVariableObject<TNumeric> : VariableObject<TNumeric>
{
	public TNumeric maxValue;
	public TNumeric defaultValue;
	
}