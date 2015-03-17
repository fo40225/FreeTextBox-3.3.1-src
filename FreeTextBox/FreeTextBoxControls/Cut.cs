using System;
namespace FreeTextBoxControls
{
	public class Cut : ToolbarButton
	{
		public Cut() : base("Cut", "cut")
		{
			base.isBuiltIn = true;
			base.CommandIdentifier = "cut";
			base.BuiltInButtonOffset = 4;
			this.htmlModeEnabled = true;
			base.builtInScript = "this.ftb.Cut();";
			base.className = "Cut";
		}
	}
}
