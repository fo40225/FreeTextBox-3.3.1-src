using System;
namespace FreeTextBoxControls
{
	public class JustifyCenter : ToolbarButton
	{
		public JustifyCenter() : base("Justify Center", "justifycenter")
		{
			base.isBuiltIn = true;
			base.BuiltInButtonOffset = 20;
			base.CommandIdentifier = "justifycenter";
			base.builtInScript = string.Empty;
			base.className = "JustifyCenter";
		}
	}
}
