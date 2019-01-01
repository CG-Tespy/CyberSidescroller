using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Game Effects/Sprite Renderer", fileName = "NewSpriteRendererEffect")]
public class SpriteRendererEffect : GameEffect
{
	// Customize in the Inspector
	// Note to self: Make an editor script to reduce the clutter at some point
	[Header("Flipping orientation of the sprite.")]
	public BoolSetting setFlipX = 				BoolSetting.None;
	public BoolSetting setFlipY = 				BoolSetting.None;


	public bool changeSprite = 					false;
	public Sprite newSprite = 					null;

	public bool changeColor = 					false;
	public Color newColor = 					Color.white;

	public bool changeDrawMode = 				false;
	public SpriteDrawMode newDrawMode = 		SpriteDrawMode.Simple;

	public bool changeMaterial = 				false;
	public Material newMaterial = 				null;

	// Fields

	// Methods

	public override void Apply(MonoBehaviour toApplyTo)
	{
		base.Apply(toApplyTo);

		SpriteRenderer spriteRenderer = 			toApplyTo.GetComponent<SpriteRenderer>();

		// Safety.
		if (spriteRenderer == null)
			NullSpriteRendererAlert(toApplyTo);

		HandleSpriteFlipping(spriteRenderer);

		if (changeSprite)
			spriteRenderer.sprite = 				newSprite;

		if (changeDrawMode)
			spriteRenderer.drawMode = 				newDrawMode;

		if (changeMaterial)
			spriteRenderer.material = 				newMaterial;

		ApplyEnd.Invoke(toApplyTo);

	}


	// Helpers

	void HandleSpriteFlipping(SpriteRenderer spriteRenderer)
	{
		switch (setFlipX)
		{
			case BoolSetting.False:
				spriteRenderer.flipX = 			false;
				break;
			case BoolSetting.True:
				spriteRenderer.flipX = 			true;
				break;
			case BoolSetting.None:
				break;
			default:
				string message = 				setFlipX.ToString() + " not accounted for in " +
												"sprite flipping.";
				throw new System.NotImplementedException(message);
				break;
		}

		switch (setFlipY)
		{
			case BoolSetting.False:
				spriteRenderer.flipY = 			false;
				break;
			case BoolSetting.True:
				spriteRenderer.flipY = 			true;
				break;
			case BoolSetting.None:
				break;
			default:
				string message = 				setFlipY.ToString() + " not accounted for in " +
												"sprite flipping.";
				throw new System.NotImplementedException(message);
				break;
		}
	}

	void NullSpriteRendererAlert(MonoBehaviour client)
	{
		string format = 					"Cannot execute effect {0} on {1}; {1} doesn't have a " +
											"SpriteRenderer component.";
		string errMessage = 				string.Format(format, name, client.name);
		throw new System.NullReferenceException(errMessage);
	}
}

[System.Serializable]
public enum BoolSetting
{
	True, False, None
}