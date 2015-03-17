using System;
namespace FreeTextBoxControls
{
	public class Bold : ToolbarButton
	{
		public Bold() : base("Bold", "bold")
		{
			base.isBuiltIn = true;
			base.BuiltInButtonOffset = 0;
			base.CommandIdentifier = "bold";
			base.builtInScript = string.Empty;
			base.className = "Bold";
		}
	}
}
