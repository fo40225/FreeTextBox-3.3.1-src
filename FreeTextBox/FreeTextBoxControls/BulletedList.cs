using System;
namespace FreeTextBoxControls
{
	public class BulletedList : ToolbarButton
	{
		public BulletedList() : base("Bulleted List", "bulletedlist")
		{
			base.isBuiltIn = true;
			base.BuiltInButtonOffset = 1;
			base.CommandIdentifier = "insertunorderedlist";
			base.builtInScript = string.Empty;
			base.className = "BulletedList";
		}
	}
}
