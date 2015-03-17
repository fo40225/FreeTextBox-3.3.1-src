using System;
namespace FreeTextBoxControls
{
	public class JustifyFull : ToolbarButton
	{
		public JustifyFull() : base("Justify Full", "justifyfull")
		{
			base.isBuiltIn = true;
			base.BuiltInButtonOffset = 21;
			base.CommandIdentifier = "justifyfull";
			base.builtInScript = string.Empty;
			base.className = "JustifyFull";
		}
	}
}
