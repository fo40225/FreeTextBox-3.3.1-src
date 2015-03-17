using System;
namespace FreeTextBoxControls
{
	public class SuperScript : ToolbarButton
	{
		public SuperScript() : base("SuperScript", "superscript")
		{
			base.isBuiltIn = true;
			base.BuiltInButtonOffset = 36;
			base.CommandIdentifier = "superscript";
			base.builtInScript = string.Empty;
			base.className = "SuperScript";
		}
	}
}
