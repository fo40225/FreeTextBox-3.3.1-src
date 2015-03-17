using System;
namespace FreeTextBoxControls
{
	public class Redo : ToolbarButton
	{
		public Redo() : base("Redo", "redo")
		{
			base.isBuiltIn = true;
			base.CommandIdentifier = "redo";
			this.htmlModeEnabled = false;
			base.BuiltInButtonOffset = 30;
			base.builtInScript = "this.ftb.Redo();";
			base.builtInEnabledScript = "this.disabled=!this.ftb.CanRedo();";
			base.className = "Redo";
		}
	}
}
