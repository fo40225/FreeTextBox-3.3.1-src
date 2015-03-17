using System;
namespace FreeTextBoxControls
{
	public class StylesMenu : ToolbarDropDownList
	{
		public StylesMenu() : base("StylesMenu", "FTB_SetStyle")
		{
			base.isBuiltIn = true;
			base.builtInScript = "this.ftb.SetStyle(this.list.options[this.list.options.selectedIndex].value); ";
			base.builtInStateScript = "return this.ftb.GetStyle();";
			base.className = "StylesMenu";
		}
	}
}
