using System;
namespace FreeTextBoxControls
{
	public class Indent : ToolbarButton
	{
		public Indent() : base("Indent", "indent")
		{
			base.isBuiltIn = true;
			base.BuiltInButtonOffset = 10;
			base.CommandIdentifier = "indent";
			base.builtInScript = "";
			base.className = "Indent";
		}
	}
}
