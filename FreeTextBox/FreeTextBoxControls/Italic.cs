using System;
namespace FreeTextBoxControls
{
	public class Italic : ToolbarButton
	{
		public Italic() : base("Italic", "italic")
		{
			base.isBuiltIn = true;
			base.BuiltInButtonOffset = 19;
			base.CommandIdentifier = "italic";
			base.builtInScript = string.Empty;
			base.className = "Italic";
		}
	}
}
