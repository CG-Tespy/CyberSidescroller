using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public interface IMover : IHasLocalTimeScale
{
	MovementHandler movementHandler { get; }
}
