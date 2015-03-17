using System;
namespace FreeTextBoxControls
{
	public class Undo : ToolbarButton
	{
		public Undo() : base("Undo", "undo")
		{
			base.isBuiltIn = true;
			this.htmlModeEnabled = false;
			base.BuiltInButtonOffset = 38;
			base.CommandIdentifier = "undo";
			base.builtInScript = "this.ftb.Undo();";
			base.builtInEnabledScript = "this.disabled=!this.ftb.CanUndo();";
			base.className = "Undo";
		}
	}
}
