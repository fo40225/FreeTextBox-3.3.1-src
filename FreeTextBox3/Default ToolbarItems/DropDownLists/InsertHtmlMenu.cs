using System;
using System.Drawing;
using System.Collections;
using FreeTextBoxControls.Support;

namespace FreeTextBoxControls {
	/// <summary>
	/// A ToolbarDropDownList to insert HTML.
	/// </summary>	
	public class InsertHtmlMenu : ToolbarDropDownList {
		public InsertHtmlMenu() : base("Insert Html") {
			this.isBuiltIn = true;
			this.builtInScript = @"this.ftb.InsertHtml(this.list.options[this.list.options.selectedIndex].value);";
			this.className = "InsertHtmlMenu";	
		}
	}
}
