using System;
namespace FreeTextBoxControls
{
	public class RemoveFormat : ToolbarButton
	{
		public RemoveFormat() : base("Remove All Formatting", "removeformat")
		{
			base.isBuiltIn = true;
			base.BuiltInButtonOffset = 31;
			base.CommandIdentifier = "removeformat";
			base.builtInScript = string.Empty;
			base.className = "RemoveFormat";
		}
	}
}
