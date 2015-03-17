using System;
namespace FreeTextBoxControls
{
	public class ParagraphMenu : ToolbarDropDownList
	{
		public ParagraphMenu() : base("Paragraph")
		{
			base.isBuiltIn = true;
			base.CommandIdentifier = "formatBlock";
			base.className = "ParagraphMenu";
		}
	}
}
