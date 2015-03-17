using System;
namespace FreeTextBoxControls
{
	public class JustifyLeft : ToolbarButton
	{
		public JustifyLeft() : base("Justify Left", "justifyleft")
		{
			base.isBuiltIn = true;
			base.BuiltInButtonOffset = 22;
			base.CommandIdentifier = "justifyleft";
			base.builtInScript = string.Empty;
			base.className = "JustifyLeft";
		}
	}
}
