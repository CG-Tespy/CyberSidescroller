using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class NumericEvent<T> : UnityEvent<T> {}


/// <typeparam name="TNumType">Numeric type of the reference (float, int, etc)</typeparam>
/// <typeparam name="TVarObj">Variable object for that numeric type.</typeparam>
public abstract class NumericVariableReference<TNumType, TVarObj> : 
VariableReference<TNumType, TVarObj> 
where TVarObj: NumericVariableObject<TNumType>
{
    public NumericEvent<TNumType> ValueChanged      { get; protected set; } = 
    new NumericEvent<TNumType>();
    public NumericEvent<TNumType> MaxValueChanged   { get; protected set; } = 
    new NumericEvent<TNumType>();

    [SerializeField] TNumType simpleMaxValue;

    public override TNumType value 
    {
        get { return base.value; }
        set 
        {
            base.value =            value;
            ValueChanged.Invoke(value);
        }
    }
    public virtual TNumType maxValue 
    {
        get { return useSimpleValue ? simpleValue : variable.maxValue; }
        set 
        {
            if (useSimpleValue)
                simpleMaxValue =                value;
            else 
                variable.maxValue =             value;

            ValueChanged.Invoke(value);
        }
    }
}

[System.Serializable]
public class FloatReference : NumericVariableReference<float, FloatVariable> {}
