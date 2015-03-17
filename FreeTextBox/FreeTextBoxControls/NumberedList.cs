using System;
namespace FreeTextBoxControls
{
	public class NumberedList : ToolbarButton
	{
		public NumberedList() : base("Numbered List", "numberedlist")
		{
			base.isBuiltIn = true;
			base.BuiltInButtonOffset = 26;
			base.CommandIdentifier = "insertorderedlist";
			base.builtInScript = string.Empty;
			base.className = "NumberedList";
		}
	}
}
