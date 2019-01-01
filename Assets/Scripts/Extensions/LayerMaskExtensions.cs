using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public static class LayerMaskExtensions
{
	/// <summary>
 	/// Credit to Ideka from Unity Answers.
	/// </summary>
	public static bool ContainsLayer(this LayerMask layerMask, int layer)
	{
		return layerMask == (layerMask | (1 << layer));
	}
}
