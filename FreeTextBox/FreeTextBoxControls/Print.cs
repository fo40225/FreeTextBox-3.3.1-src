using System;
namespace FreeTextBoxControls
{
	public class Print : ToolbarButton
	{
		public Print() : base("Print", "print")
		{
			base.isBuiltIn = true;
			base.BuiltInButtonOffset = 29;
			base.CommandIdentifier = "print";
			this.htmlModeEnabled = true;
			base.builtInScript = "this.ftb.Print();";
			base.className = "Print";
		}
	}
}
