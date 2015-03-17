using System;
namespace FreeTextBoxControls
{
	public class Outdent : ToolbarButton
	{
		public Outdent() : base("Outdent", "outdent")
		{
			base.isBuiltIn = true;
			base.BuiltInButtonOffset = 27;
			base.CommandIdentifier = "outdent";
			base.builtInScript = string.Empty;
			base.className = "Outdent";
		}
	}
}
