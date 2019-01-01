using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;


public class Check2D : CSS_MonoBehaviour2D
{
	[System.Serializable]
	public enum CheckType2D
	{
		ground, 
		head
	}

	[SerializeField] CheckType2D _type;

	public CheckType2D type 
	{ 
		get { return _type; } 
		protected set { _type = value; } 
	}

}
