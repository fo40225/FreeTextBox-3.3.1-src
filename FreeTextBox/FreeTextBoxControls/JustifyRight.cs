using System;
namespace FreeTextBoxControls
{
	public class JustifyRight : ToolbarButton
	{
		public JustifyRight() : base("Justify Right", "justifyright")
		{
			base.isBuiltIn = true;
			base.BuiltInButtonOffset = 23;
			base.CommandIdentifier = "justifyright";
			base.builtInScript = string.Empty;
			base.className = "JustifyRight";
		}
	}
}
