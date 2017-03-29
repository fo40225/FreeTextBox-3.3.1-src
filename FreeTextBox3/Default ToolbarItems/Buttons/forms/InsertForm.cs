using System;
using FreeTextBoxControls;

namespace FreeTextBoxControls {
	/// <summary>
	/// A ToolbarButton with Insert Form JavaScript functions builtin
	/// </summary>	
	public class InsertForm : ToolbarButton {
		public InsertForm() : base("Insert Form","insertform") {
			base.isProFeature = true;
			base.isBuiltIn = true;
			this.BuiltInButtonOffset = 46;
			this.builtInScript = @"this.ftb.InsertForm();";
			this.className = "InsertForm";
		}
	}
}
