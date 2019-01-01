using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public enum FadeStyle
{
	none, fadeIn, fadeOut
}

public class FadeEventArgs 
{
	public FadeStyle fadeTarget = FadeStyle.none;
	public float targetOpacity = -1;

	public FadeEventArgs(FadeStyle fadeStyle = FadeStyle.none)
	{
		this.fadeTarget = 						fadeStyle;

		switch (fadeTarget)
		{
			case FadeStyle.fadeIn:
				targetOpacity = 				0;
				break;
			case FadeStyle.fadeOut:
				targetOpacity = 				1;
				break;
		}
	}
}

public class FadeEvent : UnityEvent<FadeEventArgs> {}

[RequireComponent(typeof(CanvasGroup))]
[RequireComponent(typeof(Image))]
[ExecuteInEditMode]
public class ScreenFader : MonoBehaviourUI, IHasCanvasGroup
{
	#region Events
	public FadeEvent FadeStarted 							{ get; protected set; }
	public FadeEvent FadeEnded 								{ get; protected set; }

	#endregion
	public Image image 										{ get; protected set; }
	IEnumerator coroutine;
	
	#region Canvas Group
	public CanvasGroup canvasGroup 							{ get; protected set; }

	/// <summary>
	/// The opacity of this UI element.
	/// </summary>
	public float alpha
	{
		get 												{ return canvasGroup.alpha; } 
		set 												{ canvasGroup.alpha = value; }
	}

	public bool interactable 
	{
		get 												{ return canvasGroup.interactable; }
		set 												{ canvasGroup.interactable = value; }
	}

	public bool blocksRaycasts 
	{
		get 												{ return canvasGroup.blocksRaycasts; }
		set 												{ canvasGroup.blocksRaycasts = value; }
	}
	#endregion

	protected override void Awake()
	{
		base.Awake();
		canvasGroup = 										GetComponent<CanvasGroup>();
		image = 											GetComponent<Image>();
		FadeStarted = 										new FadeEvent();
		FadeEnded = 										new FadeEvent();
		Debug.Log("Awake executed in Edit Mode!");
	}

	public void Fade(float duration, FadeStyle fadeStyle, Color color)
	{
		float targetOpacity = 								0;

		switch (fadeStyle)
		{
			case FadeStyle.fadeIn:
				targetOpacity = 							0;
				break;
			case FadeStyle.fadeOut:
				targetOpacity = 							1;
				break;
			default:
				throw new System.NotImplementedException("Fade style " + fadeStyle + "none is invalid.");
		}

		Fade(duration, targetOpacity, color);
	}

	public void Fade(float duration, float targetOpacity, Color color)
	{
		SetupBaseComponents();
		
		if (coroutine != null)
			StopCoroutine(coroutine);
		
		coroutine = 										FadeCoroutine(duration, targetOpacity, color);
		StartCoroutine(coroutine);
	}

	IEnumerator FadeCoroutine(float duration, float targetOpacity, Color color)
	{
		Debug.Log("Fading!");
		// Announce this is happening.
		FadeEventArgs fadeArgs = 							new FadeEventArgs();
		fadeArgs.targetOpacity = 							targetOpacity;

		// When this fader gets invisible, the screen is being faded in. When it gets
		// fully-opaque, it's being faded out.
		if (targetOpacity == 0)
			fadeArgs.fadeTarget = 							FadeStyle.fadeIn;
		else if (targetOpacity == 1)
			fadeArgs.fadeTarget = 							FadeStyle.fadeOut;
		
		FadeStarted.Invoke(fadeArgs);

		// Decide how the fading will be done, and apply the color.

		float timer = 										0;
		float baseOpacity = 								alpha;

		// Do the fading over time.
		while (timer < duration)
		{
			timer += 										Time.deltaTime;
			alpha = 										Mathf.Lerp(baseOpacity, targetOpacity, 
															timer / duration);
			yield return null;
		}

		alpha = 											targetOpacity; // In case the duration is 0.
		
		// Announce this is done.
		FadeEnded.Invoke(fadeArgs);
		coroutine = 										null;
	}

	void SetupBaseComponents()
	{
		canvasGroup = 										GetComponent<CanvasGroup>();
		image = 											GetComponent<Image>();

		if (FadeStarted == null)
			FadeStarted = 										new FadeEvent();

		if (FadeEnded == null)
			FadeEnded = 										new FadeEvent();
	}

}
