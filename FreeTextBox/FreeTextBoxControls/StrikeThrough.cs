using System;
namespace FreeTextBoxControls
{
	public class StrikeThrough : ToolbarButton
	{
		public StrikeThrough() : base("StrikeThrough", "strikethrough")
		{
			base.isBuiltIn = true;
			base.BuiltInButtonOffset = 34;
			base.CommandIdentifier = "strikethrough";
			base.builtInScript = string.Empty;
			base.className = "StrikeThrough";
		}
	}
}
