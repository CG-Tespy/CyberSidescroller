using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class ShowCollider2DHeight : CSS_MonoBehaviour2D
{
	[SerializeField] float height;
	// Use this for initialization

	// Update is called once per frame
	void Update () 
	{
		height = 				collider.UpperEdge();
	}
}
