using System;
namespace FreeTextBoxControls
{
	public class InsertHtmlMenu : ToolbarDropDownList
	{
		public InsertHtmlMenu() : base("Insert Html")
		{
			base.isBuiltIn = true;
			base.builtInScript = "this.ftb.InsertHtml(this.list.options[this.list.options.selectedIndex].value);";
			base.className = "InsertHtmlMenu";
		}
	}
}
