using System;
namespace FreeTextBoxControls
{
	public class Underline : ToolbarButton
	{
		public Underline() : base("Underline", "underline")
		{
			base.isBuiltIn = true;
			base.BuiltInButtonOffset = 37;
			base.CommandIdentifier = "underline";
			base.builtInScript = string.Empty;
			base.className = "Underline";
		}
	}
}
