using System;
namespace FreeTextBoxControls
{
	public class Delete : ToolbarButton
	{
		public Delete() : base("Delete", "delete")
		{
			base.isBuiltIn = true;
			base.BuiltInButtonOffset = 5;
			base.builtInScript = "this.ftb.DeleteContents();";
			base.className = "Delete";
		}
	}
}
