using System;
using FreeTextBoxControls;

namespace FreeTextBoxControls {
	/// <summary>
	/// A ToolbarButton with InsertImage JavaScript functions builtin
	/// </summary>	
	public class InsertImage : ToolbarButton {
		public InsertImage() : base("Insert Image","insertimage") {
			isBuiltIn = true;
			this.CommandIdentifier = "insertimage";
			this.BuiltInButtonOffset = 12;
			this.builtInScript = @"this.ftb.InsertImage();";
			this.className = "InsertImage";
		}
	}
}
