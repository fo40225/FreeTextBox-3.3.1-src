using System;
using FreeTextBoxControls;

namespace FreeTextBoxControls {
	/// <summary>
	/// A ToolbarButton with CreateLink JavaScript functions builtin
	/// </summary>	
	public class CreateLink : ToolbarButton {
		public CreateLink() : base("Create Link","createlink") {
			isBuiltIn = true;
			this.BuiltInButtonOffset = 3;
			this.CommandIdentifier = "createlink";
			this.builtInScript = @"this.ftb.CreateLink();";
			this.className = "CreateLink";
		}
	}
}
