using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class ShowSpriteRendererHeight : CSS_MonoBehaviour2D
{
	[SerializeField] float height;
	SpriteRenderer spriteRenderer;

	protected override void Awake()
	{
		base.Awake();
		spriteRenderer = GetComponent<SpriteRenderer>();
	}
	
	// Update is called once per frame
	void Update () 
	{
		height = spriteRenderer.bounds.size.y;
	}
}
