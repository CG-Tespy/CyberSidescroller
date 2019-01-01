using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public static class TextExtensions
{
	static List<System.Type> textSettingTypes = 		new List<System.Type>
	{
		typeof(TextSettings),
		typeof(TextSettings.CharacterSettings),
		typeof(TextSettings.ParagraphSettings)
	};

	public static T Settings<T>(this Text text) where T: ITextSettings<T>
	{
		// Make sure we're being asked for a valid type of settings
		System.Type beingAskedFor = 		typeof(T);

		if (textSettingTypes.Contains(beingAskedFor))
		{
			//return new TextSettings() as T;
			//return new 
		}

		throw new System.NotImplementedException();
	}
	public static TextSettings FullSettings(this Text text)
	{
		return new TextSettings(text);
	}

	public static TextSettings.CharacterSettings CharacterSettings(this Text text)
	{
		return new TextSettings.CharacterSettings(text);
	}

	public static TextSettings.ParagraphSettings ParagraphSettings(this Text text)
	{
		return new TextSettings.ParagraphSettings(text);
	}
}
