using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public interface ITextSettings<T>
{
	T Copy();
}

/// <summary>
/// Encapsulates various settings in Unity's UI Text classes.
/// </summary>
[System.Serializable]
public class TextSettings : ITextSettings<TextSettings>
{
	#region Nested Classes
	public class CharacterSettings : ITextSettings<CharacterSettings>
	{
		// Settings
		public Font _font;
		public int _fontSize = 				10;
		public FontStyle fontStyle = 		FontStyle.Normal;
		public float lineSpacing = 			1;

		// Properties
		public Font font 
		{
			get { return _font; }
			set 
			{
				// Safety.
				if (value == null)
					NullFontAlert();

				_font = 					value;
			}
		}

		public int fontSize 
		{
			get { return _fontSize; }
			set 
			{
				// Safety.
				if (value <= 0)
					InvalidFontSizeAlert();
				
				_fontSize = 			value;
			}
		}

		// Constructors
		public CharacterSettings()
		{
			// Make sure to use the default font
			font = 						Resources.GetBuiltinResource<Font>("Arial.ttf");
		}

		public CharacterSettings(Font font, int fontSize = 10, 
								FontStyle fontStyle = FontStyle.Normal, 
								float lineSpacing = 1)
		{
			this.font = 				font;
			this.fontSize = 			fontSize;
			this.fontStyle = 			fontStyle;
			this.lineSpacing = 			lineSpacing;
		}

		public CharacterSettings(Text text)
		{
			this.font = 				text.font;
			this.fontSize = 			text.fontSize;
			this.fontStyle = 			text.fontStyle;
			this.lineSpacing = 			text.lineSpacing;
		}

		public CharacterSettings(CharacterSettings toCopy)
		{
			this.font = 				toCopy.font;
			this.fontSize = 			toCopy.fontSize;
			this.fontStyle = 			toCopy.fontStyle;
			this.lineSpacing = 			toCopy.lineSpacing;
		}

		// Methods

		public override string ToString()
		{
			string format = 			"Font: {0}\n" + 
										"Font Size: {1}\n" + 
										"Font Style: {2}\n" +
										"Line Spacing: {3}";

			return string.Format(format, font, fontSize, fontStyle, lineSpacing);
		}

		public CharacterSettings Copy()
		{
			return new CharacterSettings(this);
		}


		// Safety Helpers
		void NullFontAlert()
		{
			// Can't work with a null font!
			string errMessage = 		"Cannot apply a null font to UI Text Character Settings.";
			throw new System.ArgumentException(errMessage);
		}

		void InvalidFontSizeAlert()
		{
			// Can't have a font size of 0 or less!
			string errMessage = 		"Cannot apply font sizes at or below 0 to UI Text " +
										"Character Settings.";
			throw new System.ArgumentException(errMessage);
		}

	}

	public class ParagraphSettings : ITextSettings<ParagraphSettings>
	{
		// Settings
		public TextAnchor alignment = 					TextAnchor.UpperLeft;
		public HorizontalWrapMode horizontalOverflow = 	HorizontalWrapMode.Wrap;
		public VerticalWrapMode verticalOverflow = 		VerticalWrapMode.Truncate;
		public bool bestFit = 							false;
		public bool alignByGeometry = 					false;

		// Constructors
		public ParagraphSettings(TextAnchor alignment = TextAnchor.UpperLeft, 
								HorizontalWrapMode hOverflow = HorizontalWrapMode.Wrap, 
								VerticalWrapMode vOverflow = VerticalWrapMode.Truncate, 
								bool bestFit = false, bool alignByGeometry = false)
		{
			this.alignment = 				alignment;
			this.horizontalOverflow = 		hOverflow;
			this.verticalOverflow = 		vOverflow;
			this.bestFit = 					bestFit;
			this.alignByGeometry = 			alignByGeometry;
		}

		public ParagraphSettings(Text text)
		{
			if (text == null)
				NullTextAlert();

			this.alignment = 				text.alignment;
			this.horizontalOverflow = 		text.horizontalOverflow;
			this.verticalOverflow = 		text.verticalOverflow;
			this.bestFit = 					text.resizeTextForBestFit;
			this.alignByGeometry = 			text.alignByGeometry;
		}
		
		public ParagraphSettings(ParagraphSettings toCopy)
		{
			if (toCopy == null)
				NullCopyAlert();

			this.alignment = 				toCopy.alignment;
			this.horizontalOverflow = 		toCopy.horizontalOverflow;
			this.verticalOverflow = 		toCopy.verticalOverflow;
			this.bestFit = 					toCopy.bestFit;
			this.alignByGeometry = 			toCopy.alignByGeometry;
		}

		public override string ToString()
		{
			string format = 				"Alignment: {0}\n" +
											"Horizontal Overflow: {1}\n" + 
											"Vertical Overflow: {2}\n" + 
											"Best Fit: {3}\n" + 
											"Align By Geometry: {4}";
			
			return string.Format(format, alignment, horizontalOverflow, 
								verticalOverflow, bestFit, alignByGeometry);
		}

		public ParagraphSettings Copy()
		{
			return new ParagraphSettings(this);
		}

		// Safety Helpers
		void NullTextAlert()
		{
			string errMessage = 			"Cannot create UI Text Paragraph Settings " +
											"from a null UI Text object!";
			throw new System.ArgumentException(errMessage);
		}

		void NullCopyAlert()
		{
			string errMessage = 			"Cannot copy the fields of a null UI Text " +
											"Paragraph settings to another one!";
			throw new System.ArgumentException(errMessage);
		}
	}
	#endregion

	// Customizable in Inspector
	[SerializeField] CharacterSettings _character;
	[SerializeField] ParagraphSettings _paragraph;

	// For script access
	public CharacterSettings charSettings 
	{ 
		get 							{ return _character; } 
		protected set 					
		{ 
			// Safety.
			if (value == null)
				NullInstanceSettingsAlert("Character Settings");

			_character = 				value; 
		}
	}

	public ParagraphSettings paragSettings 
	{
		get 							{ return _paragraph; }
		protected set 					
		{ 
			// Safety.
			if (value == null)
				NullInstanceSettingsAlert("Paragraph Settings");

			_paragraph = 				value; 
		}
	}

	#region Character Setting Properties
	public Font font 
	{ 
		get 							{ return charSettings.font; } 
		set 							{ charSettings.font = value; } 
	}
	public int fontSize
	{
		get 							{ return charSettings.fontSize; }
		set 							{ charSettings.fontSize = value; }
	}
	public FontStyle fontStyle
	{
		get 							{ return charSettings.fontStyle; }
		set 							{ charSettings.fontStyle = value; }
	}
	public float lineSpacing
	{
		get 							{ return charSettings.lineSpacing; }
		set 							{ charSettings.lineSpacing = value; }
	}
	#endregion
	
	#region Paragraph Setting Properties
	public TextAnchor alignment 
	{
		get 							{ return paragSettings.alignment; }
		set 							{ paragSettings.alignment = value; }
	}

	public HorizontalWrapMode horizontalOverflow
	{
		get 							{ return paragSettings.horizontalOverflow; }
		set 							{ paragSettings.horizontalOverflow = value; }
	}

	public VerticalWrapMode verticalOverflow 
	{
		get 							{ return paragSettings.verticalOverflow; }
		set 							{ paragSettings.verticalOverflow = value; }
	}

	public bool bestFit 
	{
		get 							{ return paragSettings.bestFit; }
		set 							{ paragSettings.bestFit = value; }
	}

	public bool alignByGeometry
	{
		get 							{ return paragSettings.alignByGeometry; }
		set 							{ paragSettings.alignByGeometry = value; }
	}
	#endregion
	
	#region Other UI Text Settings
	public Color color = 				Color.black;
	public bool raycastTarget = 		true;
	public Material material;
	#endregion

	#region Constructors
	public TextSettings()
	{
		charSettings = 					new CharacterSettings();
		paragSettings = 				new ParagraphSettings();
	}

	public TextSettings(TextSettings toCopy)
	{
		charSettings = 					new CharacterSettings(toCopy.charSettings);
		paragSettings = 				new ParagraphSettings(toCopy.paragSettings);
	}

	public TextSettings(Text text)
	{
		ApplyCharacterSettings(text);
		ApplyParagraphSettings(text);
	}
	#endregion

	#region Methods
	public void ApplyCharacterSettings(Text text)
	{
		charSettings = 					new CharacterSettings(text);
	}

	public void ApplyCharacterSettings(CharacterSettings settings)
	{
		charSettings = 					new CharacterSettings(settings);
	}

	public void ApplyParagraphSettings(Text text)
	{
		paragSettings = 				new ParagraphSettings(text);
	}

	public void ApplyParagraphSettings(ParagraphSettings settings)
	{
		paragSettings = 				new ParagraphSettings(settings);
	}

	public void ApplyFullSettings(Text text)
	{
		ApplyCharacterSettings(text);
		ApplyParagraphSettings(text);
	}

	public void ApplyFullSettings(TextSettings settings)
	{
		ApplyCharacterSettings(settings.charSettings);
		ApplyParagraphSettings(settings.paragSettings);
	}

	public TextSettings Copy()
	{
		return new TextSettings(this);
	}
	#endregion

	#region Safety Helpers
	void NullInstanceSettingsAlert(string instanceName)
	{
		string format = 				"Cannot assign a null {0} instance as a member of a " +
										"TextSettings object.";
		string errMessage = 			string.Format(format, instanceName);
		throw new System.ArgumentException(errMessage);

	}
	#endregion
}
