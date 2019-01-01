﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public interface ISaveState<T>
{
	void ApplyTo(T toApplyTo);
	void SetFrom(T toSetFrom);
}
