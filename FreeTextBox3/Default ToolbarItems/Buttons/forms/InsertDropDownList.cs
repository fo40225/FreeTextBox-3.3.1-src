using System;
using FreeTextBoxControls;

namespace FreeTextBoxControls {
	/// <summary>
	/// A ToolbarButton with Insert DropDownList JavaScript functions builtin
	/// </summary>	
	public class InsertDropDownList : ToolbarButton {
		public InsertDropDownList() : base("Insert DropDownList","insertdropdownlist") {
			base.isProFeature = true;
			base.isBuiltIn = true;
			this.BuiltInButtonOffset = 49;
			this.builtInScript = @"this.ftb.InsertDropDownList();";
			this.builtInEnabledScript = @"this.disabled=!this.ftb.IsInForm();";
			this.className = "InsertDropDownList";
		}
	}
}
