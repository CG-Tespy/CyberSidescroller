using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public interface IHasCanvasGroup
{
	CanvasGroup canvasGroup { get; }
	float alpha { get; }
	bool interactable { get; }
	bool blocksRaycasts { get; }

}
