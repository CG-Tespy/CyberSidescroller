using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class ShowUpperEdge : CSS_MonoBehaviour2D
{
	[SerializeField] float upperEdgeY;


	
	// Update is called once per frame
	void Update () 
	{
		upperEdgeY = collider.UpperEdge();
	}
}
