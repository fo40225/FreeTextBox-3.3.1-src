using System;
using FreeTextBoxControls;

namespace FreeTextBoxControls {
	/// <summary>
	/// A ToolbarButton with BulletedList JavaScript functions builtin
	/// </summary>	
	public class BulletedList : ToolbarButton {
		public BulletedList() : base("Bulleted List","bulletedlist") {
			isBuiltIn = true;
			this.BuiltInButtonOffset = 1;
			this.CommandIdentifier = "insertunorderedlist";
			this.builtInScript = String.Empty;
			this.className = "BulletedList";
		}
	}
}