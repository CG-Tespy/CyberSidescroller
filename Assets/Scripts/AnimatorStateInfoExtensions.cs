using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public static class AnimatorStateInfoExtensions 
{
	public static float ProgressInAnimation(this AnimatorStateInfo asi)
	{
		return asi.normalizedTime - (int) asi.normalizedTime;
	}
}
