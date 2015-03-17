using System;
namespace FreeTextBoxControls
{
	public class Paste : ToolbarButton
	{
		public Paste() : base("Paste", "paste")
		{
			base.isBuiltIn = true;
			base.CommandIdentifier = "paste";
			this.htmlModeEnabled = true;
			base.BuiltInButtonOffset = 28;
			base.builtInScript = "this.ftb.Paste();";
			base.className = "Paste";
		}
	}
}
