using System;
namespace FreeTextBoxControls
{
	public class SubScript : ToolbarButton
	{
		public SubScript() : base("SubScript", "subscript")
		{
			base.isBuiltIn = true;
			base.BuiltInButtonOffset = 35;
			base.CommandIdentifier = "subscript";
			base.builtInScript = string.Empty;
			base.className = "SubScript";
		}
	}
}
